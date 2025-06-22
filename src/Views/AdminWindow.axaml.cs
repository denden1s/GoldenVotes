using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Golden_votes.Entities;
using Golden_votes.Utils;
using HarfBuzzSharp;
using Microsoft.IdentityModel.Tokens;
using Avalonia.Controls;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using System.Collections.Generic;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;

namespace Golden_votes.Views;

public partial class AdminWindow : Window
{
  private int VoteNameLength = 50;
  private VoteCreateWindow vote_window;
  private List<User> users;
  private List<Vote> votes;

  private Encryption? encryption;

  private void LoadPieChart(List<Answer> answers)
  {
    var pieSeries = new List<ISeries>();
    foreach (var answer in answers)
    {
      PieSeries<int> series = new PieSeries<int>
      {
        Values = new int[] { answer.Users.Count },
        Name = answer.Name
      };
      pieSeries.Add(series);
    }
    PieStat.Series = pieSeries;
    PieStat.CoreChart.Update();
  }

  public AdminWindow()
  {
    InitializeComponent();
    Settings.ConfigureWindow(this);
    VotesList.Height = UsersList.Height = this.Height * 0.8;
    PieStat.Width = this.Width * 0.6;
    PieStat.Height = this.Height * 0.8;
    encryption = null;
    DeleteUserButton.IsEnabled = false;
    UserCreateButton.IsEnabled = false;

    UsersList.Items.Add("Загрузите ключ для просмотра пользователей");
    users = ApplicationContext.LoadUsers();
    votes = ApplicationContext.LoadVotes();
    foreach (var vote in votes)
      vote.LoadAnswers();
    LoadVotesInListBox();
  }

  private void LoadVotesInListBox()
  {
    VotesList.Items.Clear();
    foreach (var vote in votes)
    {
      int len = VoteNameLength < vote.Name.Length ? VoteNameLength : vote.Name.Length;
      string name = len == VoteNameLength ? vote.Name.Substring(0, VoteNameLength) + "..." :
                                            vote.Name;
      VotesList.Items.Add(name);

    }
  }

  private void LoadUsersInListBox()
  {
    if (encryption == null)
      return;

    users = ApplicationContext.LoadUsers();
    UsersList.Items.Clear();
    List<string> users_logins = new List<string>();
    foreach (var user in users)
    {
      string name = encryption.Decrypt(user.Login);
      users_logins.Add(name);
    }
    users_logins = users_logins.OrderBy(x => x, StringComparer.OrdinalIgnoreCase).ToList();
    foreach (var user in users_logins)
    {
      UsersList.Items.Add(user);
    }
    DeleteUserButton.IsEnabled = UsersList.Items.Count() != 0;
  }
  private void OnVoteCreateClick(object? sender, RoutedEventArgs e)
  {
    VoteCreateWindow vote_window = new VoteCreateWindow(this);
    vote_window.Show();
    this.Hide();
  }

  private async void OnLoadKeyClick(object? sender, RoutedEventArgs e)
  {
    var fileService = new FileService(this);
    string? privKeyPath = await fileService.OpenFilePickerAsync();

    if (privKeyPath == null)
    {
      InfoMessageBox.Show(this, "Ошибка", "Вы не выбрали файл!");
      return;
    }
    encryption = new Encryption(privKeyPath);
    LoadUsersInListBox();
    LoadKeyButton.IsEnabled = false;
    UserCreateButton.IsEnabled = true;
  }


  private async void OnDeleteUserButton(object? sender, RoutedEventArgs e)
  {
    if (UsersList.SelectedItem == null)
    {
      InfoMessageBox.Show(this, "Ошибка", "Пользователь не выбран");
      return;
    }

    var result = await VariantMessageBox.Show(this, "Предупреждение",
        $"Вы действительно хотите удалить пользователя {UsersList.SelectedItem.ToString()}");
    if (result == false)
    {
      InfoMessageBox.Show(this, "Информация", "Удаление пользователя отменено");
      return;
    }

    User user = new User(UsersList.SelectedItem.ToString());
    if (ApplicationContext.DeleteUser(user.ID) == false)
    {
      InfoMessageBox.Show(this, "Ошибка", "Удаление пользователя завершилось неудачно");
      return;
    }
    UsersList.Items.Remove(UsersList.SelectedItem);
    DeleteUserButton.IsEnabled = UsersList.Items.Count() != 0;
    InfoMessageBox.Show(this, "Информация", "Пользователь успешно удален");

  }

  private void OnUserCreateClick(object? sender, RoutedEventArgs e)
  {
    if (UserLoginTextBox.Text.IsNullOrEmpty())
    {
      InfoMessageBox.Show(this, "Ошибка", "Введите логин пользователя");
      return;
    }
    if (UserPasswordTextBox.Text.IsNullOrEmpty())
    {
      InfoMessageBox.Show(this, "Ошибка", "Введите пароль пользователя");
      return;
    }
    User.UserRole role =
      UserRoleComboBox.SelectedValue.ToString() == "Администратор" ? User.UserRole.kAdmin :
                                                                     User.UserRole.kBaseUser;
    if (ApplicationContext.AddUser(new User(UserLoginTextBox.Text, UserPasswordTextBox.Text, role)))
      InfoMessageBox.Show(this, "Информация", "Регистрация прошла успешно");
    else
      InfoMessageBox.Show(this, "Ошибка", $"Пользователь {UserLoginTextBox.Text} уже существует");

    UserLoginTextBox.Clear();
    UserPasswordTextBox.Clear();
    LoadUsersInListBox();
  }
  private void OnWindowClosing(object? sender, WindowClosingEventArgs e)
  {
    Environment.Exit(0);
  }

  public void CloseVoteWindow(VoteCreateWindow vote_win)
  {
    this.Show();
    vote_win.Hide();
  }

  private void VotesList_SelectionChanged(object? sender, SelectionChangedEventArgs e)
  {
    if (VotesList.SelectedItem is string selected)
    {
      string name = selected.Contains("...") ? selected.Substring(0, VoteNameLength) : selected;
      var vote = votes.Where(vote => vote.Name.Contains(name)).FirstOrDefault();
      if (vote != null)
      {
        VoteQuestion.Text = vote.Name;
        LoadPieChart(vote.Answers);
      }
    }
  }
}