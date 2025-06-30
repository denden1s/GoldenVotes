using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;
using System.Threading.Tasks;

namespace Golden_votes.Utils.MessageBox;

public class InputMessageBox : Window
{
  private readonly TextBox _inputBox;
  public string InputText { get; private set; } = string.Empty;

  public InputMessageBox(string title, string watermark)
  {
    Title = title;
    Width = 350;
    Height = 160;
    WindowStartupLocation = WindowStartupLocation.CenterOwner;
    Settings.SetWindowsColor(this);
    CanResize = false;

    _inputBox = new TextBox
    {
      Watermark = watermark,
      Margin = new Thickness(10),
      HorizontalAlignment = HorizontalAlignment.Stretch
    };

    var okButton = new Button
    {
      Content = "OK",
      Width = 80,
      Margin = new Thickness(0, 0, 5, 0),
      Background = Settings.CreateColor("#F0DCCA")
    };

    okButton.Click += Ok_Click;

    var cancelButton = new Button
    {
      Content = "Cancel",
      Width = 80,
      Background = Settings.CreateColor("#F0DCCA")
    };

    cancelButton.Click += Cancel_Click;

    var buttonPanel = new StackPanel
    {
      Orientation = Orientation.Horizontal,
      HorizontalAlignment = HorizontalAlignment.Right,
      Margin = new Thickness(10),
      Spacing = 10
    };

    buttonPanel.Children.AddRange(new[] { okButton, cancelButton });

    var mainPanel = new StackPanel
    {
      Margin = new Thickness(10),
      Spacing = 10,
      Children = { _inputBox, buttonPanel }
    };

    Content = mainPanel;
  }

  private void Ok_Click(object sender, RoutedEventArgs e)
  {
    InputText = _inputBox.Text ?? string.Empty;
    Close(true);
  }

  private void Cancel_Click(object sender, RoutedEventArgs e) => Close(false);

  public static Task<string> Show(Window parent,
                                  string title = "Input",
                                  string watermark = "Write text",
                                  string message = null)
  {
    var dialog = new InputMessageBox(title, watermark);

    if (!string.IsNullOrEmpty(message))
    {
      var messageBlock = new TextBlock
      {
        Text = message,
        Margin = new Thickness(10, 0, 10, 0),
        TextWrapping = TextWrapping.Wrap
      };

      ((StackPanel)dialog.Content).Children.Insert(0, messageBlock);
    }

    var tcs = new TaskCompletionSource<string>();
    dialog.Closed += (_, _) => tcs.SetResult(dialog.InputText);

    if (parent != null)
      dialog.ShowDialog(parent);
    else
      dialog.Show();

    return tcs.Task;
  }
}
