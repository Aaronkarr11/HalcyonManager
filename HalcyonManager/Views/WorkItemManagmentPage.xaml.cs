using HalcyonSoft.Clients;
using HalcyonManager.ViewModels;

namespace HalcyonManager.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WorkItemManagmentPage : ContentPage
    {
        WorkItemManagmentViewModel _viewModel;
        public WorkItemManagmentPage()
        {
            InitializeComponent();
            var service = DependencyService.Get<IHalcyonManagementClient>();
            BindingContext = _viewModel = new WorkItemManagmentViewModel(service);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }

        private void HelpButton_Clicked(object sender, EventArgs e)
        {
            Shell.Current.GoToAsync($"HelpPage");
        }
    }
}