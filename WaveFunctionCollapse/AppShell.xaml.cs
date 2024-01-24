namespace WaveFunctionCollapse
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(Views.MapOptionPage), typeof(Views.MapOptionPage));
            Routing.RegisterRoute(nameof(Views.MapTileSettingsPage), typeof(Views.MapTileSettingsPage));
            Routing.RegisterRoute(nameof(Views.MapPage), typeof(Views.MapPage));
        }
    }
}
