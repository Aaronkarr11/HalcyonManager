using HalcyonSoft.Clients;
using HalcyonManager.ViewModels;

namespace HalcyonManager.Views
{
    public partial class WorkTaskPage : ContentPage
    {
        public WorkTaskPage()
        {
            InitializeComponent();
            var service = DependencyService.Get<IHalcyonManagementClient>();
            BindingContext = new WorkTaskViewModel(service);
            
        }

    }
}