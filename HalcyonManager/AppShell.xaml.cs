using HalcyonManager.ViewModels;
using HalcyonManager.Views;

namespace HalcyonManager;

public partial class AppShell : Shell
{
	public AppShell()
	{
        InitializeComponent();
        BindingContext = new AppShellViewModel();
        Routing.RegisterRoute(nameof(ProjectPage), typeof(ProjectPage));
        Routing.RegisterRoute(nameof(OperationPage), typeof(OperationPage));
        Routing.RegisterRoute(nameof(WorkTaskPage), typeof(WorkTaskPage));
        Routing.RegisterRoute(nameof(WorkTaskPrioritiesPage), typeof(WorkTaskPrioritiesPage));
        Routing.RegisterRoute(nameof(ItemRequestPage), typeof(ItemRequestPage));
        Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
        Routing.RegisterRoute(nameof(WorkItemManagmentPage), typeof(WorkItemManagmentPage));
        Routing.RegisterRoute(nameof(ConfigurationPage), typeof(ConfigurationPage));
        Routing.RegisterRoute(nameof(HouseHoldManagmentPage), typeof(HouseHoldManagmentPage));
        Routing.RegisterRoute(nameof(HouseHoldMemberPage), typeof(HouseHoldMemberPage));
        Routing.RegisterRoute(nameof(HelpPage), typeof(HelpPage));
    }
}
