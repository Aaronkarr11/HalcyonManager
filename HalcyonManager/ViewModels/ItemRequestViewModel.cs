﻿using Halcyon.Clients;
using HalcyonManagement.Entities;
using HalcyonManager.Views;
using HalcyonSoft.SharedEntities;
using System.Diagnostics;
using System.Windows.Input;

namespace HalcyonManager.ViewModels
{
    public class ItemRequestViewModel : BaseViewModel
    {
        private IHalcyonManagementClient _transactionServices;
        public Command LoadItemsCommand { get; }
        public Command AddItemCommand { get; }

        public Command OnRefreshCommand { get; }

        public Command<RequestItemResponse> ItemTapped { get; }

        public ICommand DeletePersonCommand { get; private set; }

        public ItemRequestViewModel(IHalcyonManagementClient transactionServices)
        {
            _transactionServices = transactionServices;

            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            OnRefreshCommand = new Command(async () => await ExecuteLoadItemsCommand());
            AddItemCommand = new Command(OnAddItem);

            //ItemTapped = new Command<RequestItemResponse>(OnItemChecked);

            DeletePersonCommand = new Command((name) =>
            {
                OnItemChecked((RequestItemResponse)name);
            });
        }

        async void OnItemChecked(RequestItemResponse item)
        {
            try
            {
                RequestItemRequest request = new RequestItemRequest();
                request.Key1 = item.PartitionKey;
                request.Key2 = item.RowKey;
                request.Operation = "D";
                request.DesiredDate = item.DesiredDate;
                request.DeviceName = DeviceInfo.Name.RemoveSpecialCharacters();
                await _transactionServices.CreateRequestItem(request);
                await ExecuteLoadItemsCommand();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                RequestItems = await _transactionServices.GetRequestItems(DeviceInfo.Name.RemoveSpecialCharacters());
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void OnAppearing()
        {
            IsBusy = true;
        }

        private async void OnAddItem(object obj)
        {
            await Shell.Current.GoToAsync(nameof(NewItemPage));
        }


        private List<RequestItemResponse> _requestItems;
        public List<RequestItemResponse> RequestItems
        {
            get => _requestItems;
            set => SetProperty(ref _requestItems, value);
        }

    }
}