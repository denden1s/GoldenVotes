using Avalonia.Controls;
using Avalonia.Media;

namespace Golden_votes.Utils;

public static class Settings
{
  public const int WindowWidth = 1200;
  public const int WindowHeight = 800;

  public const bool IsResizableWindow = false;

  public const int MinWindowWidth = 1200;
  public const int MinWindowHeight = 800;

  public const int MaxWindowWidth = 1920;
  public const int MaxWindowHeight = 1080;

  // color pallete: https://coolors.co/696d7d-6f9283-9ead99-cdc6a5-f0dcca
  public static SolidColorBrush CreateColor(string hex_color)
  {
     var color = Avalonia.Media.Color.Parse(hex_color);
     return new SolidColorBrush(color);
  }
  public static void SetWindowsColor(Window window)
  {
    window.Background = CreateColor("#696D7D");
  }
  public static void ConfigureWindow(Window window)
  {
    window.Width = WindowWidth;
    window.Height = WindowHeight;
    SetWindowsColor(window);

    if (IsResizableWindow)
    {
      window.MinWidth = MinWindowWidth;
      window.MinHeight = MinWindowHeight;

      window.MaxWidth = MaxWindowWidth;
      window.MaxHeight = MaxWindowHeight;
    }
    window.CanResize = IsResizableWindow;
  }
}

