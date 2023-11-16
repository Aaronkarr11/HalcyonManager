using HalcyonCore.Clients;
using HalcyonCore.Interfaces;
using HalcyonCore.SharedEntities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalcyonManager.ViewModels
{
    public class NewItemViewModel : BaseViewModel
    {
        private IHalcyonManagementClient _transactionServices;

        public NewItemViewModel(IHalcyonManagementClient transactionServices)
        {
            _transactionServices = transactionServices;
            RequestedDate = DateTime.Now;
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            this.PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();
        }

        private bool ValidateSave()
        {
            return !String.IsNullOrWhiteSpace(_name);
        }

        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private DateTime _description;
        public DateTime RequestedDate
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        public Command SaveCommand { get; }
        public Command CancelCommand { get; }

        private async void OnCancel()
        {
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }

        private async void OnSave()
        {
            try
            {
                RequestItemsTableTemplate requestItemRequest = new RequestItemsTableTemplate();
                requestItemRequest.DesiredDate = RequestedDate;
                requestItemRequest.Title = Name;
                requestItemRequest.IsFulfilled = 0;
                requestItemRequest.DeviceName = DeviceInfo.Name.RemoveSpecialCharacters();

                await _transactionServices.CreateRequestItem(requestItemRequest);

                // This will pop the current page off the navigation stack
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                ErrorLogModel error = Helpers.ReturnErrorMessage(ex, "NewItemViewModel", "OnSave");
                await _transactionServices.AzureFunctionPostTransaction("https://halcyontransactions.azurewebsites.net/api/CreateOrUpdateErrorLog?code=L9qTodcWmd_SyBsd5tGJucvCYhEY0gCzn4EMW0BM5rpXAzFuwcCuBQ==", JsonConvert.SerializeObject(error));
                App._alertSvc.ShowAlert("Exception!", $"{ex.Message}");
            }
        }
    }
}
