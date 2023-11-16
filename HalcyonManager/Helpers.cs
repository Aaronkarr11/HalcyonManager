using HalcyonCore.SharedEntities;

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

        public static ErrorLogModel ReturnErrorMessage(Exception ex, string className, string nethodName)
        {
            ErrorLogModel error = new ErrorLogModel();
            error.Message = ex.Message;
            error.ClassName = className;
            error.MethodName = nethodName;
            error.DeviceName = DeviceInfo.Name.RemoveSpecialCharacters();
            error.PartitionKey = System.Guid.NewGuid().ToString();
            error.RowKey = System.Guid.NewGuid().ToString();
            return error;
        }

    }
}
