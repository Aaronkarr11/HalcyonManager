using HalcyonSoft.Clients;
using HalcyonManager.ViewModels;

namespace HalcyonManager.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WorkTaskPrioritiesPage : ContentPage
    {
        WorkTaskPrioritiesViewModel _viewModel;
        public WorkTaskPrioritiesPage()
        {
            InitializeComponent();
            var service = DependencyService.Get<IHalcyonManagementClient>();
            BindingContext = _viewModel = new WorkTaskPrioritiesViewModel(service);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}