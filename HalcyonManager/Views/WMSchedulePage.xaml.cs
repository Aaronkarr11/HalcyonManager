using HalcyonCore.Interfaces;
using HalcyonManager.ViewModels;

namespace HalcyonManager.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class WMSchedulePage : ContentPage
{
    WMScheduleViewModel _viewModel;
    public WMSchedulePage()
    {
        InitializeComponent();
        var service = DependencyService.Get<IHalcyonManagementClient>();
        BindingContext = _viewModel = new WMScheduleViewModel(service);
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.OnAppearing();
    }
}