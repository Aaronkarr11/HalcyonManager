using HalcyonManager.ViewModels;
using HalcyonCore.Interfaces;

namespace HalcyonManager.Views
{
    public partial class ErrorLogPage : ContentPage
    {
        ErrorLogViewModel _viewModel;
        public ErrorLogPage()
        {
            InitializeComponent();
            var service = DependencyService.Get<IHalcyonManagementClient>();
            BindingContext = new ErrorLogViewModel(service);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}