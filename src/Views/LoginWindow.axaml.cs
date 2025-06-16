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
      InfoMessageBox.Show(this, "Information", "Необходимо ввести логин и пароль!");
      return;
    }
    User user = new User(LoginTextBox.Text, PasswordTextBox.Text);
    using (ApplicationContext db = new ApplicationContext())
    {
      if (db.Users.Where(u => u.ID == user.ID).FirstOrDefault() == null)
      {
        InfoMessageBox.Show(this, "Information", $"Пользователь {LoginTextBox.Text} отсутствует в системе!");
        return;
      }
    }
    Window childWindow = user.Role == User.UserRole.kBaseUser ? new AdminWindow() : // TODO: realize user window which take user object in constructor
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
      InfoMessageBox.Show(this, "Information", "Необходимо ввести логин и пароль!");
      return;
    }
    User user = new User(LoginTextBox.Text, PasswordTextBox.Text);
    using (ApplicationContext db = new ApplicationContext())
    {
      if (db.Users.Where(u => u.ID == user.ID).FirstOrDefault() != null)
      {
        InfoMessageBox.Show(this, "Information", $"Пользователь {LoginTextBox.Text} уже существует!");
        return;
      }
      db.Users.Add(user);
      db.SaveChanges();
    }
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