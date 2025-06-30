using Avalonia.Controls;
using Avalonia.Interactivity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Linq;
using System.Collections.Generic;

using Golden_votes.Entities;
using Golden_votes.Utils;

namespace Golden_votes.Views;

public partial class AdminWindow : Window
{
  private List<User> _users;
  private List<Vote> _votes;
  private Encryption? _encryption;
  private bool _firstLoad = true;
  private Chart _pieChart;

  private void LoadPieChart(List<Answer> answers)
  {
    if (_pieChart == null || answers.Count() == 0)
      return;

    _pieChart.LoadData(answers);
  }

  public AdminWindow()
  {
    InitializeComponent();
    Settings.ConfigureWindow(this);
    VotesList.Height = UsersList.Height = Height * 0.8;
    PieStat.Width = Width * 0.6;
    PieStat.Height = Height * 0.8;
    _encryption = null;
    DeleteUserButton.IsEnabled = false;
    UserCreateButton.IsEnabled = false;
    _pieChart = new Chart(ref PieStat);

    UsersList.Items.Add("Загрузите ключ для просмотра пользователей");
    _users = ApplicationContext.LoadUsers();
    _votes = ApplicationContext.LoadVotes();

    foreach (var vote in _votes)
      vote.LoadAnswers();

    LoadVotesInListBox();
    _firstLoad = false;
  }

  private void LoadVotesInListBox()
  {
    int len;
    string name;
    VotesList.Items.Clear();
    foreach (var vote in _votes)
    {
      len = Settings.kVoteNameLength < vote.Name.Length ? Settings.kVoteNameLength : vote.Name.Length;
      name = len == Settings.kVoteNameLength ? vote.Name.Substring(0, Settings.kVoteNameLength) + "..." :
                                               vote.Name;
      VotesList.Items.Add(name);
    }
  }

  private void LoadUsersInListBox()
  {
    if (_encryption == null)
      return;

    UsersList.Items.Clear();
    _users = ApplicationContext.LoadUsers();
    foreach (var user in _users)
      UsersList.Items.Add(_encryption.Decrypt(user.Login));

    DeleteUserButton.IsEnabled = UsersList.Items.Count() != 0;
  }

  private void OnVoteCreateClick(object? sender, RoutedEventArgs e)
  {
    VoteCreateWindow vote_window = new VoteCreateWindow(this);
    vote_window.Show();
    Hide();
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
    _encryption = new Encryption(privKeyPath);
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
    if (!result)
    {
      InfoMessageBox.Show(this, "Информация", "Удаление пользователя отменено");
      return;
    }

    User user = new User(UsersList.SelectedItem.ToString());
    if (!ApplicationContext.DeleteUser(user.ID))
    {
      InfoMessageBox.Show(this, "Ошибка", "Удаление пользователя завершилось неудачно");
      return;
    }
    UsersList.Items.Remove(UsersList.SelectedItem);
    DeleteUserButton.IsEnabled = UsersList.Items.Count() != 0;
    InfoMessageBox.Show(this, "Информация", "Пользователь успешно удален");
    _votes = ApplicationContext.LoadVotes();
    foreach (var vote in _votes)
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
    string selectedUserRole = UserRoleComboBox.SelectedValue.ToString();
    User.UserRole role = selectedUserRole == "Администратор" ? User.UserRole.kAdmin :
                                                               User.UserRole.kBaseUser;

    if (ApplicationContext.AddUser(new User(UserLoginTextBox.Text, UserPasswordTextBox.Text, role)))
      InfoMessageBox.Show(this, "Информация", "Регистрация прошла успешно");
    else
      InfoMessageBox.Show(this, "Ошибка", $"Пользователь {UserLoginTextBox.Text} уже существует");

    UserLoginTextBox.Clear();
    UserPasswordTextBox.Clear();
    LoadUsersInListBox();
  }
  private void OnWindowClosing(object? sender, WindowClosingEventArgs e) => Environment.Exit(0);

  protected override void OnOpened(EventArgs e)
  {
    base.OnOpened(e);
    if (_firstLoad)
      return;

    _votes = ApplicationContext.LoadVotes();
    foreach (var vote in _votes)
      vote.LoadAnswers();

    LoadVotesInListBox();
  }
  public void CloseVoteWindow(VoteCreateWindow vote_win)
  {
    Show();
    vote_win.Hide();
  }

  private void VotesList_SelectionChanged(object? sender, SelectionChangedEventArgs e)
  {
    if (VotesList.SelectedItem is string selected)
    {
      string name = selected.Contains("...") ? selected.Substring(0, Settings.kVoteNameLength) : selected;
      var vote = _votes.Where(vote => vote.Name.Contains(name)).FirstOrDefault();
      if (vote != null)
      {
        VoteQuestion.Text = vote.Name;
        LoadPieChart(vote.Answers);
      }
    }
  }
}
