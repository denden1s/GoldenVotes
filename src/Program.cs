using Avalonia;
using System;
using System.Threading;

namespace Golden_votes;

class Program
{
  private static readonly Mutex AppMutex = new Mutex(true, "Global\\GoldenVotes.mu");

  [STAThread]
  public static void Main(string[] args)
  {
    // Check if another instance is already running
    if (!AppMutex.WaitOne(TimeSpan.Zero, true))
      return;

    try
    {
      BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
    }
    finally
    {
      AppMutex.ReleaseMutex();
    }
  }

  public static AppBuilder BuildAvaloniaApp()
    => AppBuilder.Configure<App>()
      .UsePlatformDetect()
      .WithInterFont()
      .LogToTrace();
}
