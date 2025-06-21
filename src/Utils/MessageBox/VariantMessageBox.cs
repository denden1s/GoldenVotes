using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;
using System.Threading.Tasks;

namespace Golden_votes.Utils;

public class VariantMessageBox : Window
{
  public bool Result { get; private set; } = false;

  public VariantMessageBox(string title, string message)
  {
    this.Title = title;
    this.WindowStartupLocation = WindowStartupLocation.CenterOwner;
    Settings.SetWindowsColor(this);

    var panel = new StackPanel
    {
      Margin = new Thickness(20),
      Spacing = 10
    };

    panel.Children.Add(new TextBlock
    {
      Text = message,
      TextWrapping = TextWrapping.Wrap,
      MaxWidth = 300
    });

    var buttonPanel = new StackPanel
    {
      Orientation = Orientation.Horizontal,
      HorizontalAlignment = HorizontalAlignment.Right,
      Spacing = 10
    };

    var yesButton = new Button
    {
      Content = "Yes",
      Width = 80,
      Background = Settings.CreateColor("#F0DCCA")
    };
    yesButton.Click += (s, e) => { Result = true; Close(); };

    var noButton = new Button
    {
      Content = "No",
      Width = 80,
      Background = Settings.CreateColor("#F0DCCA")
    };
    noButton.Click += (s, e) => { Result = false; Close(); };

    buttonPanel.Children.Add(yesButton);
    buttonPanel.Children.Add(noButton);

    panel.Children.Add(buttonPanel);

    Content = panel;
    SizeToContent = SizeToContent.WidthAndHeight;
    CanResize = false;
  }

  public static async Task<bool> Show(Window parent, string title,
                                      string message)
  {
    var msgBox = new VariantMessageBox(title, message);
    await msgBox.ShowDialog(parent);
    return msgBox.Result;
  }
}