using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Golden_votes.Utils;

namespace Golden_votes.Views;

public partial class UserWindow : Window
{
    public UserWindow()
    {
        InitializeComponent();
        Settings.ConfigureWindow(this);
        VotesList.Height = this.Height * 0.8;
    }
      private void OnWindowClosing(object? sender, WindowClosingEventArgs e)
    {
        Environment.Exit(0);
    }
}