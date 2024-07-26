using HalcyonCore.Clients;
using HalcyonManager.ViewModels;
using HalcyonCore.Interfaces;

namespace HalcyonManager.Views
{
    [XamlCompilation(XamlCompilationOptions.Skip)]
    public partial class MainPage : ContentPage
    {
        HomeViewModel _viewModel;
        public MainPage()
        {
            InitializeComponent();
            var service = DependencyService.Get<IHalcyonManagementClient>();
            BindingContext = _viewModel = new HomeViewModel(service);
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

        private void AboutButton_Clicked(object sender, EventArgs e)
        {
            DisplayAlert("Version 1.3.10; Build 10", $"Copyright {DateTime.Now.Year} - Aaron Karr - made with love <3", "OK");
        }
    }
}