using CommunityToolkit.Mvvm.Input;
using HalcyonCore.Interfaces;
using HalcyonCore.SharedEntities;
using Newtonsoft.Json;

namespace HalcyonManager.ViewModels
{
    public class ErrorLogViewModel: BaseViewModel
    {

        private IHalcyonManagementClient _transactionServices;
        public Command ExecuteNewMemberCommand { get; }

        public ErrorLogViewModel(IHalcyonManagementClient transactionServices)
        {
            _transactionServices = transactionServices;

            ExecuteNewMemberCommand = new Command(() =>
            {
                ExecuteNewMember();
            });

        }
    


        public async Task OnAppearing()
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

        public async void ExecuteNewMember()
        {
            await Shell.Current.GoToAsync("..");
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
