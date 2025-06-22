using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Golden_votes.Utils;

namespace Golden_votes.Views;

public partial class VoteCreateWindow : Window
{
  private AdminWindow parentWindow;
  private int answers_count = 1;
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

  private void OnAddTextBoxClick(object? sender, RoutedEventArgs e)
  {
    var textBox = new TextBox
    {
      Watermark = $"Вариант {answers_count}",
      Margin = new Thickness(0, 0, 0, 5)
    };
    AnswersPanel.Children.Add(textBox);
    answers_count++;
  }
  
  private void OnDelTextBoxClick(object? sender, RoutedEventArgs e)
  {
    if (AnswersPanel.Children.Count > 0)
    {
      AnswersPanel.Children.RemoveAt(AnswersPanel.Children.Count - 1);
      answers_count--;
    }
  }
}