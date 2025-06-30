using Avalonia.Controls;
using Avalonia.Media;

namespace Golden_votes.Utils;

public static class Settings
{
  public const int kWindowWidth = 1200;
  public const int kWindowHeight = 800;

  public const bool kIsResizableWindow = false;

  public const int kMinWindowWidth = 1200;
  public const int kMinWindowHeight = 800;

  public const int kMaxWindowWidth = 1920;
  public const int kMaxWindowHeight = 1080;
  
  public const int kVoteNameLength = 50;

  // color pallete: https://coolors.co/696d7d-6f9283-9ead99-cdc6a5-f0dcca
  public static SolidColorBrush CreateColor(string hex_color) => new SolidColorBrush(Color.Parse(hex_color));
  public static void SetWindowsColor(Window window) => window.Background = CreateColor("#696D7D");
  public static void ConfigureWindow(Window window)
  {
    window.Width = kWindowWidth;
    window.Height = kWindowHeight;
    window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
    SetWindowsColor(window);

    if (kIsResizableWindow)
    {
      window.MinWidth = kMinWindowWidth;
      window.MinHeight = kMinWindowHeight;

      window.MaxWidth = kMaxWindowWidth;
      window.MaxHeight = kMaxWindowHeight;
    }
    window.CanResize = kIsResizableWindow;
  }
}
