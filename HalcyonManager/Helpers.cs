namespace HalcyonManager
{
    public static class Helpers
    {


        public static string ReturnDeviceFontSize()
        {
            if (DeviceInfo.Platform == DevicePlatform.WinUI)
            {
                return "Small";
            }
            else
            {
               return "Medium";
            }
        }

        public static double ReturnDeviceButtonWidth()
        {
            if (DeviceInfo.Platform == DevicePlatform.WinUI)
            {
                return 200;
            }
            else
            {
                return 130;
            }
        }

        public static bool ReturnDeviceNavigationDesktop()
        {
            if (DeviceInfo.Platform == DevicePlatform.WinUI)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool ReturnDeviceNavigationMobile()
        {
            if (DeviceInfo.Platform == DevicePlatform.WinUI)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

    }
}
