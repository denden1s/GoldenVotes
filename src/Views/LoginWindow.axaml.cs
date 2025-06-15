using System.Data.Common;
using Avalonia.Controls;
using Avalonia.Interactivity;
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
    this.LoginPanel.Height = this.Height / 3.5;
    this.LoginButton.Width = this.LoginPanel.Width;
  }

  private async void OnLoginClick(object? sender, RoutedEventArgs e)
  {
    // setup ip of server from json
    // TODO: realize
    if (string.IsNullOrEmpty(LoginTextBox.Text) ||
        string.IsNullOrEmpty(PasswordTextBox.Text))
    {
      // InfoMessageBox.Show(this, "Information", "This is a message box in Avalonia!");
      // return;

      // var result = await VariantMessageBox.Show(this, "Confirmation", "Do you want to proceed?");
    }
    Encryption enc = new Encryption();
    string hash_password = enc.Hash(LoginTextBox.Text + PasswordTextBox.Text);
    string login = enc.Encrypt(LoginTextBox.Text);

    AdminWindow win = new AdminWindow();
    win.Show();
    this.Hide();
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