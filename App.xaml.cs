namespace journal2;

public partial class App : Application
{
    public App() : base() 
	{
        InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new AppShell());
    }
}