using Halcyon.Clients;
using HalcyonManager.ViewModels;

namespace HalcyonManager.Views
{
    public partial class ProjectPage : ContentPage
    {
        public ProjectPage()
        {
            InitializeComponent();
            var service = DependencyService.Get<IHalcyonManagementClient>();
            BindingContext = new ProjectViewModel(service);
        }
    }
}