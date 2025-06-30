using Avalonia.Controls;
using Avalonia.Interactivity;
using System.Linq;

using Golden_votes.Entities;
using Golden_votes.Utils;
using Golden_votes.Utils.MessageBox;

namespace Golden_votes.Views;

public partial class LoginWindow : Window
{
  public LoginWindow()
  {
    InitializeComponent();
    Settings.ConfigureWindow(this);
    LoginPanel.Width = Width / 3;
    LoginPanel.Height = Height / 2.8;
    LoginButton.Width = LoginPanel.Width;
  }

  private void OnLoginClick(object? sender, RoutedEventArgs e)
  {
    if (string.IsNullOrEmpty(LoginTextBox.Text) ||
        string.IsNullOrEmpty(PasswordTextBox.Text))
    {
      InfoMessageBox.Show(this, "Информация", "Необходимо ввести логин и пароль!");
      return;
    }

    User user = new User(LoginTextBox.Text, PasswordTextBox.Text);
    User? tpmUser;

    using (ApplicationContext db = new ApplicationContext())
    {
      tpmUser = db.Users.Where(u => u.ID == user.ID).FirstOrDefault();
      if (tpmUser == null)
      {
        InfoMessageBox.Show(this, "Ошибка", $"Пользователь {LoginTextBox.Text} отсутствует в системе");
        return;
      }
      user.Role = tpmUser.Role;
    }
    if (user.Password != tpmUser.Password)
    {
      InfoMessageBox.Show(this, "Ошибка", "Введен неверный пароль");
      return;
    }
    Window childWindow = user.Role == User.UserRole.kBaseUser ? new UserWindow(user) :
                                                                new AdminWindow();
    childWindow.Show();
    Hide();
  }

  private void OnRegisterClick(object? sender, RoutedEventArgs e)
  {
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
