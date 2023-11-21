using HalcyonCore.Interfaces;
using HalcyonManager.ViewModels;

namespace HalcyonManager.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class ErrorLogPage : ContentPage
{
    ErrorLogViewModel _viewModel;
    public ErrorLogPage()
    {
        InitializeComponent();
        var service = DependencyService.Get<IHalcyonManagementClient>();
        BindingContext = _viewModel = new ErrorLogViewModel(service);
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.OnAppearing();
    }
}