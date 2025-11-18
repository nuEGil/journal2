using journal2.Services;
namespace journal2
{

    public partial class App : Application
    {
        public App(IFileSeeder fileSeeder, IDataBaseInitializer dbi)
        {
            InitializeComponent();
            Task.Run(async () => await dbi.DBInitAsync());
            Task.Run(async () => await fileSeeder.SeedAsync());
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}