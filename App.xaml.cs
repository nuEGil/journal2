namespace journal2;

public partial class App : Application
{
    public App() : base() 
	{
        InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        var window = base.CreateWindow(activationState);
        window.Page = new AppShell();   // reassign your app shell
        return window;
    }
}