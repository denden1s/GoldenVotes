using System;
using System.ComponentModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Golden_votes.Utils;

namespace Golden_votes.Views;

public partial class AdminWindow : Window
{
    private static AdminWindow instance;
    private VoteCreateWindow vote_window;

    public AdminWindow()
    {
        InitializeComponent();
        Settings.ConfigureWindow(this);
        VotesList.Height = UsersList.Height = this.Height * 0.8;
    }

    private void OnVoteCreateClick(object? sender, RoutedEventArgs e)
    {
        // TODO: realize
        VoteCreateWindow vote_window = new VoteCreateWindow(this);
        vote_window.Show();
        this.Hide();
    }

    private void OnWindowClosing(object? sender, WindowClosingEventArgs e)
    {
        Environment.Exit(0);
    }

    public void CloseVoteWindow(VoteCreateWindow vote_win)
    {
        this.Show();
        vote_win.Hide();
    }
}