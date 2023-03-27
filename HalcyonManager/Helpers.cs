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

    }
}
