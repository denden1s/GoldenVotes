using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace Golden_votes.Views;

public partial class AdminWindow : Window
{
    public AdminWindow()
    {
        InitializeComponent();
        this.Width = Settings.WindowWidth;
        this.Height = Settings.WindowHeight;
    }

    private void OnVoteCreateClick(object? sender, RoutedEventArgs e)
    {
        // TODO: realize
        VoteCreateWindow win = new VoteCreateWindow();
        win.Show();
        this.Hide();
    }
}