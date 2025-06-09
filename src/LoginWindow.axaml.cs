using Avalonia.Controls;

namespace Golden_votes;

public partial class LoginWindow : Window
{

    public LoginWindow()
    {
        InitializeComponent();
        this.Width = Configuration.WindowWidth;
        this.Height = Configuration.WindowHeight;

    }
}