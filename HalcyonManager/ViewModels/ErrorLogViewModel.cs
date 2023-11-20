using HalcyonCore.Clients;
using HalcyonManager.Views;
using HalcyonCore.SharedEntities;
using System.Diagnostics;
using System.Windows.Input;
using HalcyonCore.Interfaces;
using Newtonsoft.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace HalcyonManager.ViewModels
{
    //[QueryProperty(nameof(RequestItem), nameof(RequestItem))]
    public class ErrorLogViewModel : BaseViewModel
    {
        private IHalcyonManagementClient _transactionServices;

        public ErrorLogViewModel(IHalcyonManagementClient transactionServices)
        {
            _transactionServices = transactionServices;
        }

        public async void OnAppearing()
        {
            IsBusy = true;

            try
            {
                ErrorLogModel model = new ErrorLogModel();
                model.DeviceName = DeviceInfo.Name.RemoveSpecialCharacters();
                var rawResponse = await _transactionServices.AzureFunctionPostTransaction("https://halcyontransactions.azurewebsites.net/api/GetErrorLogs?code=2BJp2INil2qgEePhcCDp7EMnOo1-xbf81WZ7vTxgb050AzFuDarH6A==", JsonConvert.SerializeObject(model));
                ErrorLogList = JsonConvert.DeserializeObject<List<ErrorLogModel>>(rawResponse);

                if (ErrorLogList.Count == 0)
                {
                    ErrorPageTitle = "No Errors Found or Logged!";
                }
                else
                {
                    ErrorPageTitle = $"Showing The First {ErrorLogList.Count} Errors";
                }
            }
            catch (Exception ex)
            {
                ErrorLogModel error = Helpers.ReturnErrorMessage(ex, "HouseHoldManagmentViewModel", "OnAppearing");
                await _transactionServices.AzureFunctionPostTransaction("https://halcyontransactions.azurewebsites.net/api/CreateOrUpdateErrorLog?code=L9qTodcWmd_SyBsd5tGJucvCYhEY0gCzn4EMW0BM5rpXAzFuwcCuBQ==", JsonConvert.SerializeObject(error));
                App._alertSvc.ShowAlert("Exception!", $"{ex.Message}");
            }
        }



        private List<ErrorLogModel> _errorLogList;
        public List<ErrorLogModel> ErrorLogList
        {
            get => _errorLogList;
            set => SetProperty(ref _errorLogList, value);
        }



        private string _errorPageTitle;
        public string ErrorPageTitle
        {
            get => _errorPageTitle;
            set => SetProperty(ref _errorPageTitle, value);
        }

    }
}
