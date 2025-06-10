using Avalonia.Controls;

namespace Golden_votes;

public partial class LoginWindow : Window
{
    private static LoginWindow instance;
    private LoginWindow()
    {
        InitializeComponent();
        this.Width = Settings.WindowWidth;
        this.Height = Settings.WindowHeight;
    }
    public static LoginWindow getInstance()
    {
        if (instance == null)
            instance = new LoginWindow();
        return instance;
    }
}