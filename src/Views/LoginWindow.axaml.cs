using Avalonia.Controls;
using Avalonia.Interactivity;

namespace Golden_votes.Views;

public partial class LoginWindow : Window
{
    private static LoginWindow instance;
    private LoginWindow()
    {
        InitializeComponent();
        this.Width = Settings.WindowWidth;
        this.Height = Settings.WindowHeight;
        this.LoginPanel.Width = this.Width / 3;
        this.LoginPanel.Height = this.Height / 3.5;
        this.LoginButton.Width = this.LoginPanel.Width;
    }
    public static LoginWindow getInstance()
    {
        if (instance == null)
            instance = new LoginWindow();
        return instance;
    }
    private void OnLoginClick(object? sender, RoutedEventArgs e)
    {
        // TODO: realize
        AdminWindow win = new AdminWindow();
        win.Show();
        this.Hide();
    }
}