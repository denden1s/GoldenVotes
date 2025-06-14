using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Golden_votes.Utils;

namespace Golden_votes.Views;

public partial class VoteCreateWindow : Window
{
  private AdminWindow parentWindow;
  public VoteCreateWindow(AdminWindow win)
  {
    InitializeComponent();
    Settings.ConfigureWindow(this);
    parentWindow = win;
  }
  private void CloseWindow()
  {
    parentWindow.Show();
    this.Hide();
  }
  private void OnWindowClosing(object? sender, WindowClosingEventArgs e)
  {
    CloseWindow();
  }

  private void OnSubmitVote(object? sender, RoutedEventArgs e)
  {
    // TODO: realize logic
    CloseWindow();
  }
}