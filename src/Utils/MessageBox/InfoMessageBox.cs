using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Layout;
using System.Threading.Tasks;
using System;

namespace Golden_votes.Utils;
public class InfoMessageBox : Window
{
  public InfoMessageBox(string title, string message)
  {
    this.Title = title;
    WindowStartupLocation = WindowStartupLocation.CenterOwner;
    Settings.SetWindowsColor(this);
    var panel = new StackPanel
    {
      Margin = new Thickness(10)
    };

    panel.Children.Add(new TextBlock
    {
      Text = message,
      Margin = new Thickness(0, 0, 0, 10),
      TextWrapping = TextWrapping.Wrap
    });

    var okButton = new Button
    {
      Content = "OK",
      HorizontalAlignment = HorizontalAlignment.Center,
      Width = 100,
      Background = Settings.CreateColor("#F0DCCA")
      
    };
    okButton.Click += (s, e) => Close();

    panel.Children.Add(okButton);

    Content = panel;
    SizeToContent = SizeToContent.WidthAndHeight;
    CanResize = false;
  }

  // Helper method to show the message box
public static void Show(Window parent, string title, string message, Action? onClose = null)
{
    var msgBox = new InfoMessageBox(title, message);
    msgBox.ShowDialog(parent).ContinueWith(_ =>
    {
        // Code here runs after the dialog is closed
        onClose?.Invoke();
    }, TaskScheduler.FromCurrentSynchronizationContext());
}
}