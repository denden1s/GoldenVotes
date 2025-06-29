using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Golden_votes.Entities;
using Golden_votes.Utils;
using Golden_votes.Utils.MessageBox;

using Tmds.DBus.Protocol;

namespace Golden_votes.Views;

public partial class LoginWindow : Window
{
  public LoginWindow()
  {
    InitializeComponent();
    Settings.ConfigureWindow(this);
    this.LoginPanel.Width = this.Width / 3;
    this.LoginPanel.Height = this.Height / 2.8;
    this.LoginButton.Width = this.LoginPanel.Width;
  }

  private async void OnLoginClick(object? sender, RoutedEventArgs e)
  {
    // setup ip of server from json
    // TODO: realize
    if (string.IsNullOrEmpty(LoginTextBox.Text) ||
        string.IsNullOrEmpty(PasswordTextBox.Text))
    {
      InfoMessageBox.Show(this, "Информация", "Необходимо ввести логин и пароль!");
      return;
    }
    User user = new User(LoginTextBox.Text, PasswordTextBox.Text);
    User? tpm_user;
    using (ApplicationContext db = new ApplicationContext())
    {
      tpm_user = db.Users.Where(u => u.ID == user.ID).FirstOrDefault();
      if (tpm_user == null)
      {
        InfoMessageBox.Show(this, "Ошибка", $"Пользователь {LoginTextBox.Text} отсутствует в системе");
        return;
      }
      user.Role = tpm_user.Role;
    }
    if (user.Password != tpm_user.Password)
    {
      InfoMessageBox.Show(this, "Ошибка", "Введен неверный пароль");
      return;
    }
    Window childWindow = user.Role == User.UserRole.kBaseUser ? new UserWindow(user) :
                                                                new AdminWindow();
    childWindow.Show();
    this.Hide();
  }

  private async void OnRegisterClick(object? sender, RoutedEventArgs e)
  {
    // setup ip of server from json
    // TODO: realize
    if (string.IsNullOrEmpty(LoginTextBox.Text) ||
        string.IsNullOrEmpty(PasswordTextBox.Text))
    {
      InfoMessageBox.Show(this, "Ошибка", "Необходимо ввести логин и пароль");
      return;
    }
    if (ApplicationContext.AddUser(new User(LoginTextBox.Text, PasswordTextBox.Text)))
      InfoMessageBox.Show(this, "Информация", "Регистрация прошла успешно");
    else
      InfoMessageBox.Show(this, "Ошибка", $"Пользователь {LoginTextBox.Text} уже существует");

    LoginTextBox.Clear();
    PasswordTextBox.Clear();
  }

  private async void SetupServerLocation(object? sender, RoutedEventArgs e)
  {
    string messageBoxTitle = "Server IP";
    var serverAddress = await InputMessageBox.Show(this, messageBoxTitle, "192.168.100.1",
                                                   "Пожалуйста введите адрес сервера БД:");

    if (!DBServer.IsValidIP(serverAddress))
    {
      InfoMessageBox.Show(this, messageBoxTitle, "Данные введены некорректно");
      return;
    }
    DBServer dBServer = new DBServer();
    dBServer.Setup(serverAddress);
  }
}