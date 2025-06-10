using Avalonia;
using System;
using System.Threading;

namespace Golden_votes;

class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    private static readonly Mutex AppMutex = new Mutex(true, "Global\\GoldenVotes.mu");

    [STAThread]
    public static void Main(string[] args)
    {
        // Check if another instance is already running
        if (!AppMutex.WaitOne(TimeSpan.Zero, true))
            return;

        try
        {
            BuildAvaloniaApp()
                .StartWithClassicDesktopLifetime(args);
        }
        finally
        {
            AppMutex.ReleaseMutex();
        }
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
}
