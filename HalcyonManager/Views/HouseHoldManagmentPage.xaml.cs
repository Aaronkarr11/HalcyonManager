using HalcyonSoft.Clients;
using HalcyonManager.ViewModels;

namespace HalcyonManager.Views
{
    public partial class HouseHoldManagmentPage : ContentPage
    {
        HouseHoldManagmentViewModel _viewModel;
        public HouseHoldManagmentPage()
        {
            InitializeComponent();
            var service = DependencyService.Get<IHalcyonManagementClient>();
            BindingContext = _viewModel = new HouseHoldManagmentViewModel(service);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}