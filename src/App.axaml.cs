using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;

using Golden_votes.Views;

namespace Golden_votes;

public partial class App : Application
{
  public override void Initialize() => AvaloniaXamlLoader.Load(this);

  public override void OnFrameworkInitializationCompleted()
  {
    if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
    {
      ApplicationContext.GenerateAdmin();
      ApplicationContext.GenerateContent();
      desktop.MainWindow = new LoginWindow();
    }
    base.OnFrameworkInitializationCompleted();
  }
}
