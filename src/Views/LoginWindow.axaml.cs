using Avalonia.Controls;
using Avalonia.Interactivity;

namespace Golden_votes.Views;

public partial class LoginWindow : Window
{
    private static LoginWindow instance;
    public LoginWindow()
    {
        InitializeComponent();
        Settings.ConfigureWindow(this);
        this.LoginPanel.Width = this.Width / 3;
        this.LoginPanel.Height = this.Height / 3.5;
        this.LoginButton.Width = this.LoginPanel.Width;
    }
    private void OnLoginClick(object? sender, RoutedEventArgs e)
    {
        // TODO: realize
        AdminWindow win = new AdminWindow();
        win.Show();
        this.Hide();
    }
}