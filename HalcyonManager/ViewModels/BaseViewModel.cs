using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HalcyonManager.ViewModels
{
    public class BaseViewModel : ObservableObject
    {
        bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        string title = string.Empty;
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        private string _deviceFontSize;
        public string DeviceFontSize
        {
            get => _deviceFontSize;
            set => SetProperty(ref _deviceFontSize, value);
        }

    }
}
