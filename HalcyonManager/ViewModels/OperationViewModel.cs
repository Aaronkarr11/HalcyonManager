using Halcyon.Clients;
using HalcyonManagement.Entities;
using HalcyonSoft.SharedEntities;
using Newtonsoft.Json;

namespace HalcyonManager.ViewModels
{
    [QueryProperty(nameof(Operation), nameof(Operation))]
    public class OperationViewModel : BaseViewModel
    {

        private IHalcyonManagementClient _transactionServices;

        public OperationViewModel(IHalcyonManagementClient transactionServices)
        {
            _transactionServices = transactionServices;
            CancelCommand = new Command(OnCancel);

            SaveCommand = new Command((obj) =>
            {
                OnSave(obj);
                ValidateSave();
            });

            DeleteCommand = new Command((obj) =>
            {
                OnDelete(obj);
            });

            CompleteCommand = new Command((obj) =>
            {
                OnComplete(obj);
            });


            this.PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();

        }

        private bool ValidateSave()
        {
            return !String.IsNullOrWhiteSpace(_selectedOperation.Title);
        }

        private string _operation;
        public string Operation
        {
            get
            {
                return _operation;
            }
            set
            {
                _operation = value;
                LoadItemId(value);
            }
        }

        public void LoadItemId(string operation)
        {
            try
            {


                SelectedOperation = JsonConvert.DeserializeObject<OperationModel>(operation);

                if (String.IsNullOrEmpty(SelectedOperation.PartitionKey) && String.IsNullOrEmpty(SelectedOperation.RowKey))
                {
                    SelectedOperation.PartitionKey = System.Guid.NewGuid().ToString();
                    SelectedOperation.RowKey = System.Guid.NewGuid().ToString();
                    Name = $"Create a New Operation";
                    ShowDeleteButton = false;
                }
                else
                {
                    Name = $"Edit Operation: {SelectedOperation.Title}";
                    ShowDeleteButton = true;
                }
            }
            catch (Exception ex)
            {

            }
        }

        private OperationModel _selectedOperation;
        public OperationModel SelectedOperation
        {
            get => _selectedOperation;
            set => SetProperty(ref _selectedOperation, value);
        }

        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private bool _showDeleteButton;
        public bool ShowDeleteButton
        {
            get => _showDeleteButton;
            set => SetProperty(ref _showDeleteButton, value);
        }

        public Command SaveCommand { get; }
        public Command CancelCommand { get; }
        public Command CompleteCommand { get; }
        public Command DeleteCommand { get; }

        private async void OnCancel()
        {
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }

        private void OnComplete(object obj)
        {
            App._alertSvc.ShowConfirmation("Warning", "Are you sure you want to complete this operation? All child items will be marked as completed as well.", (async result =>
            {
                if (result)
                {
                    try
                    {
                        OperationViewModel rawOperationViewModel = (OperationViewModel)obj;
                        OperationModel operation = rawOperationViewModel.SelectedOperation;
                        operation.Completed = 1;
                        operation.DeviceName = DeviceInfo.Name.RemoveSpecialCharacters();
                        string uri = "https://projecthalcyonmanagmenttransactions.azurewebsites.net/api/DeleteOperation?code=l7I4qs1BtTY4AMeBNX2o2TKn25tvyrGUcLG1ZqCksDALcpZfg8RH0Q==";
                        await _transactionServices.AzureFunctionPostTransaction(uri, JsonConvert.SerializeObject(operation));
                        await Shell.Current.GoToAsync("..");
                    }
                    catch (Exception ex)
                    {
                        App._alertSvc.ShowConfirmation("Error", $"{ex.Message}", (result => { App._alertSvc.ShowAlert("Result", $"{result}"); }));
                    }
                }

            }));
        }

        private void OnDelete(object obj)
        {
            App._alertSvc.ShowConfirmation("Warning", "Are you sure you want to delete?", (async result =>
            {
                if (result)
                {
                    try
                    {
                        OperationViewModel rawOperationViewModel = (OperationViewModel)obj;
                        OperationModel operation = rawOperationViewModel.SelectedOperation;
                        operation.Completed = 0;
                        operation.DeviceName = DeviceInfo.Name.RemoveSpecialCharacters();
                        string uri = "https://projecthalcyonmanagmenttransactions.azurewebsites.net/api/DeleteOperation?code=l7I4qs1BtTY4AMeBNX2o2TKn25tvyrGUcLG1ZqCksDALcpZfg8RH0Q==";
                        await _transactionServices.AzureFunctionPostTransaction(uri, JsonConvert.SerializeObject(operation));
                        await Shell.Current.GoToAsync("..");
                    }
                    catch (Exception ex)
                    {
                        App._alertSvc.ShowConfirmation("Error", $"{ex.Message}", (result => { App._alertSvc.ShowAlert("Result", $"{result}"); }));
                    }
                }

            }));
        }

        private async void OnSave(object obj)
        {

            OperationViewModel rawOperationViewModel = (OperationViewModel)obj;
            OperationModel operation = rawOperationViewModel.SelectedOperation;
            operation.Completed = 0;
            operation.DeviceName = DeviceInfo.Name.RemoveSpecialCharacters();
            string uri = "https://projecthalcyonmanagmenttransactions.azurewebsites.net/api/CreateOperation?code=irK9ChkTDXiaTDYITRMk4SSDwKpYMnXZEQEPyFoY8yT4Rhpy8Q6aDQ==";
            await _transactionServices.AzureFunctionPostTransaction(uri, JsonConvert.SerializeObject(operation));
            await Shell.Current.GoToAsync("..");
        }
    }
}
