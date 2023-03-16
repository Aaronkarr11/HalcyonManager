using HalcyonManager.Views;

namespace HalcyonManager;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
        Routing.RegisterRoute(nameof(ItemRequestPage), typeof(ItemRequestPage));
        Routing.RegisterRoute(nameof(ConfigurationPage), typeof(ConfigurationPage));
    }
}
