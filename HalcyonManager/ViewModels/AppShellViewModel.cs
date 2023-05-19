
namespace HalcyonManager.ViewModels
{
    public class AppShellViewModel : BaseViewModel
    {
        public AppShellViewModel()
        {
            DeviceNavigationMobile = Helpers.ReturnDeviceNavigationMobile();
            DeviceNavigationDesktop = Helpers.ReturnDeviceNavigationDesktop();
        }

        private bool _deviceNavigationMobile;
        public bool DeviceNavigationMobile
        {
            get => _deviceNavigationMobile;
            set => SetProperty(ref _deviceNavigationMobile, value);
        }

        private bool _deviceNavigationDesktop;
        public bool DeviceNavigationDesktop
        {
            get => _deviceNavigationDesktop;
            set => SetProperty(ref _deviceNavigationDesktop, value);
        }
    }
}
