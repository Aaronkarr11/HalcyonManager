﻿using HalcyonCore.Clients;
using HalcyonCore.Interfaces;
using HalcyonCore.SharedEntities;
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
            DeviceFontSize = Helpers.ReturnDeviceFontSize();

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

        private OperationModel _operation;
        public OperationModel Operation
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

        public async void LoadItemId(OperationModel operation)
        {
            try
            {
                SelectedOperation = operation;
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
                ErrorLogModel error = Helpers.ReturnErrorMessage(ex, "OperationViewModel", "LoadItemId");
                await _transactionServices.AzureFunctionPostTransaction("https://halcyontransactions.azurewebsites.net/api/CreateOrUpdateErrorLog?code=fXB5yroHKAH8GBb3M9VouDv2WTNjOR0AeBa_McAn6i6bAzFuJ2yxJg%3D%3D", JsonConvert.SerializeObject(error));
                App._alertSvc.ShowAlert("Exception!", $"{ex.Message}");
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
                        string uri = "https://halcyontransactions.azurewebsites.net/api/DeleteOrCompleteOperation?code=s3IFPSS8xIUCUwYRdMJ58ci4IDXjJxP6NHCsyIEPNe3XAzFu2yGOIg==";
                        await _transactionServices.AzureFunctionPostTransaction(uri, JsonConvert.SerializeObject(operation));
                        await Shell.Current.GoToAsync("..");
                    }
                    catch (Exception ex)
                    {
                        ErrorLogModel error = Helpers.ReturnErrorMessage(ex, "OperationViewModel", "OnComplete");
                        await _transactionServices.AzureFunctionPostTransaction("https://halcyontransactions.azurewebsites.net/api/CreateOrUpdateErrorLog?code=fXB5yroHKAH8GBb3M9VouDv2WTNjOR0AeBa_McAn6i6bAzFuJ2yxJg%3D%3D", JsonConvert.SerializeObject(error));
                        App._alertSvc.ShowAlert("Exception!", $"{ex.Message}");
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
                        string uri = "https://halcyontransactions.azurewebsites.net/api/DeleteOrCompleteOperation?code=s3IFPSS8xIUCUwYRdMJ58ci4IDXjJxP6NHCsyIEPNe3XAzFu2yGOIg==";
                        await _transactionServices.AzureFunctionPostTransaction(uri, JsonConvert.SerializeObject(operation));
                        await Shell.Current.GoToAsync("..");
                    }
                    catch (Exception ex)
                    {
                        ErrorLogModel error = Helpers.ReturnErrorMessage(ex, "OperationViewModel", "OnDelete");
                        await _transactionServices.AzureFunctionPostTransaction("https://halcyontransactions.azurewebsites.net/api/CreateOrUpdateErrorLog?code=fXB5yroHKAH8GBb3M9VouDv2WTNjOR0AeBa_McAn6i6bAzFuJ2yxJg%3D%3D", JsonConvert.SerializeObject(error));
                        App._alertSvc.ShowAlert("Exception!", $"{ex.Message}");
                    }
                }

            }));
        }

        private async void OnSave(object obj)
        {

            try
            {
                OperationViewModel rawOperationViewModel = (OperationViewModel)obj;
                OperationModel operation = rawOperationViewModel.SelectedOperation;
                operation.Completed = 0;
                operation.DeviceName = DeviceInfo.Name.RemoveSpecialCharacters();
                string uri = "https://halcyontransactions.azurewebsites.net/api/CreateOrUpdateOperation?code=fXB5yroHKAH8GBb3M9VouDv2WTNjOR0AeBa_McAn6i6bAzFuJ2yxJg%3D%3D";
                await _transactionServices.AzureFunctionPostTransaction(uri, JsonConvert.SerializeObject(operation));
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {

                ErrorLogModel error = Helpers.ReturnErrorMessage(ex, "OperationViewModel", "OnSave");
                await _transactionServices.AzureFunctionPostTransaction("https://halcyontransactions.azurewebsites.net/api/CreateOrUpdateErrorLog?code=fXB5yroHKAH8GBb3M9VouDv2WTNjOR0AeBa_McAn6i6bAzFuJ2yxJg%3D%3D", JsonConvert.SerializeObject(error));
                App._alertSvc.ShowAlert("Exception!", $"{ex.Message}");
            }
        }
    }
}
