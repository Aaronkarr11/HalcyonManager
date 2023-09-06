using HalcyonSoft.Clients;
using HalcyonManager.ViewModels;

namespace HalcyonManager.Views
{
    public partial class OperationPage : ContentPage
    {
        public OperationPage()
        {
            InitializeComponent();
            var service = DependencyService.Get<IHalcyonManagementClient>();
            BindingContext = new OperationViewModel(service);
        }
    }
}