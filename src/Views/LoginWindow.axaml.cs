using Avalonia.Controls;
using Avalonia.Interactivity;
using Golden_votes.Utils;
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

  private async void  OnLoginClick(object? sender, RoutedEventArgs e)
  {
    // TODO: realize
    if (string.IsNullOrEmpty(LoginTextBox.Text) ||
        string.IsNullOrEmpty(PasswordTextBox.Text))
    {
      // InfoMessageBox.Show(this, "Information", "This is a message box in Avalonia!");
      // return;

      // var result = await VariantMessageBox.Show(this, "Confirmation", "Do you want to proceed?");
    }
    Encryption enc = new Encryption();
    // TODO: maybe salt = login
    
    AdminWindow win = new AdminWindow();
    win.Show();
    this.Hide();
  }
}