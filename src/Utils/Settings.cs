using System;
using Avalonia.Controls;

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

  public static void ConfigureWindow(Window window)
  {
    window.Width = WindowWidth;
    window.Height = WindowHeight;

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

