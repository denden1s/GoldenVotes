using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Golden_votes.Entities;
using Golden_votes.Utils;
using Microsoft.IdentityModel.Tokens;

namespace Golden_votes.Views;

public partial class AdminWindow : Window
{
  private VoteCreateWindow vote_window;
  private List<User> users;
  private List<Vote> votes;

  private Encryption? encryption;

  private bool first_load = true;

  private Chart pieChart;

  private void LoadPieChart(List<Answer> answers)
  {
    if (pieChart == null || answers.Count() == 0)
      return;

    pieChart.LoadData(answers);
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
    pieChart = new Chart(ref PieStat);

    UsersList.Items.Add("Загрузите ключ для просмотра пользователей");
    users = ApplicationContext.LoadUsers();
    votes = ApplicationContext.LoadVotes();
    foreach (var vote in votes)
      vote.LoadAnswers();
    LoadVotesInListBox();
    first_load = false;
  }

  private void LoadVotesInListBox()
  {
    VotesList.Items.Clear();
    foreach (var vote in votes)
    {
      int len = Settings.VoteNameLength < vote.Name.Length ? Settings.VoteNameLength : vote.Name.Length;
      string name = len == Settings.VoteNameLength ? vote.Name.Substring(0, Settings.VoteNameLength) + "..." :
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
    votes = ApplicationContext.LoadVotes();
    foreach (var vote in votes)
      vote.LoadAnswers();
    LoadVotesInListBox();
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

  protected override void OnOpened(EventArgs e)
  {
    base.OnOpened(e);
    if (first_load)
      return;
    votes = ApplicationContext.LoadVotes();
    foreach (var vote in votes)
      vote.LoadAnswers();
    LoadVotesInListBox();
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
      string name = selected.Contains("...") ? selected.Substring(0, Settings.VoteNameLength) : selected;
      var vote = votes.Where(vote => vote.Name.Contains(name)).FirstOrDefault();
      if (vote != null)
      {
        VoteQuestion.Text = vote.Name;
        LoadPieChart(vote.Answers);
      }
    }
  }
}