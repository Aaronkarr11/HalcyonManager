using HalcyonCore.Clients;

using HalcyonManager.Views;
using HalcyonCore.SharedEntities;
using System.Diagnostics;
using System.Windows.Input;
using HalcyonCore.Interfaces;
using Newtonsoft.Json;


namespace HalcyonManager.ViewModels
{
    //[QueryProperty(nameof(RequestItem), nameof(RequestItem))]
    public class ItemRequestViewModel : BaseViewModel
    {
        private IHalcyonManagementClient _transactionServices;
        public Command LoadItemsCommand { get; }
        public Command AddItemCommand { get; }

        public Command OnRefreshCommand { get; }

        public Command<RequestItemResponse> ItemTapped { get; }

        public ICommand CompleteCommand { get; private set; }

        public ItemRequestViewModel(IHalcyonManagementClient transactionServices)
        {
            _transactionServices = transactionServices;

            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            OnRefreshCommand = new Command(async () => await ExecuteLoadItemsCommand());
            AddItemCommand = new Command(OnAddItem);

            //ItemTapped = new Command<RequestItemResponse>(OnItemChecked);

            CompleteCommand = new Command((obj) =>
            {
                OnItemChecked(obj);
            });

        }

        async void OnItemChecked(object obj)
        {
            try
            {
                RequestItemsModel item = (RequestItemsModel)obj;

                RequestItemsTableTemplate request = new RequestItemsTableTemplate();
                request.PartitionKey = item.PartitionKey;
                request.RowKey = item.RowKey;
                request.DesiredDate = item.DesiredDate;
                request.IsFulfilled = 1;
                request.DeviceName = DeviceInfo.Name.RemoveSpecialCharacters();
                await _transactionServices.CreateRequestItem(request);
                await ExecuteLoadItemsCommand();
            }
            catch (Exception ex)
            {
                ErrorLogModel error = Helpers.ReturnErrorMessage(ex, "ItemRequestViewModel", "OnItemChecked");
                await _transactionServices.AzureFunctionPostTransaction("https://halcyontransactions.azurewebsites.net/api/CreateOrUpdateErrorLog?code=L9qTodcWmd_SyBsd5tGJucvCYhEY0gCzn4EMW0BM5rpXAzFuwcCuBQ==", JsonConvert.SerializeObject(error));
                App._alertSvc.ShowAlert("Exception!", $"{ex.Message}");
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

        public async void OnAppearing()
        {
            RequestItems = await _transactionServices.GetRequestItems(DeviceInfo.Name.RemoveSpecialCharacters());
            IsBusy = true;
        }

        private async void OnAddItem(object obj)
        {
            await Shell.Current.GoToAsync(nameof(NewItemPage));
        }


        private List<RequestItemsModel> _requestItems;
        public List<RequestItemsModel> RequestItems
        {
            get => _requestItems;
            set => SetProperty(ref _requestItems, value);
        }

    }
}
