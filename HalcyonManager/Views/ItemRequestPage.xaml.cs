using HalcyonCore.Interfaces;
using HalcyonManager.ViewModels;

namespace HalcyonManager.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemRequestPage : ContentPage
    {
        ItemRequestViewModel _viewModel;
        public ItemRequestPage()
        {
            InitializeComponent();
            var service = DependencyService.Get<IHalcyonManagementClient>();
            BindingContext = _viewModel = new ItemRequestViewModel(service);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}