using HalcyonSoft.Clients;
using HalcyonManager.ViewModels;
using HalcyonSoft.Interfaces;

namespace HalcyonManager.Views
{
    public partial class NewItemPage : ContentPage
    {
        public NewItemPage()
        {
            InitializeComponent();
            var service = DependencyService.Get<IHalcyonManagementClient>();
            BindingContext = new NewItemViewModel(service);
        }
    }
}