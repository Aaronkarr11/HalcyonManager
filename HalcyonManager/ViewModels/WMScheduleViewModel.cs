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
    public class WMScheduleViewModel : BaseViewModel
    {
        private IHalcyonManagementClient _transactionServices;
        //public Command LoadItemsCommand { get; }


        public WMScheduleViewModel(IHalcyonManagementClient transactionServices)
        {
            _transactionServices = transactionServices;

            //  LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            string uri = "https://halcyontransactions.azurewebsites.net/api/GetWMSchedule?code=AUGSN4BuZI56GDahZZIS5WawTIQH3XcINMzgTxT8NLDUAzFueie4ow==";
        }



        //async void OnItemChecked(object obj)
        //{
        //    try
        //    {
        //        RequestItemsModel item = (RequestItemsModel)obj;

        //        RequestItemsTableTemplate request = new RequestItemsTableTemplate();
        //        request.PartitionKey = item.PartitionKey;
        //        request.RowKey = item.RowKey;
        //        request.DesiredDate = item.DesiredDate;
        //        request.IsFulfilled = 1;
        //        request.DeviceName = DeviceInfo.Name.RemoveSpecialCharacters();
        //        await _transactionServices.CreateRequestItem(request);
        //        await ExecuteLoadItemsCommand();
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorLogModel error = Helpers.ReturnErrorMessage(ex, "ItemRequestViewModel", "OnItemChecked");
        //        await _transactionServices.AzureFunctionPostTransaction("https://halcyontransactions.azurewebsites.net/api/CreateOrUpdateErrorLog?code=fXB5yroHKAH8GBb3M9VouDv2WTNjOR0AeBa_McAn6i6bAzFuJ2yxJg%3D%3D", JsonConvert.SerializeObject(error));
        //        App._alertSvc.ShowAlert("Exception!", $"{ex.Message}");
        //    }
        //}



        public async Task OnAppearing()
        {
            RequestItems = await _transactionServices.GetRequestItems(DeviceInfo.Name.RemoveSpecialCharacters());
            IsBusy = true;
        }


        private List<RequestItemsModel> _requestItems;
        public List<RequestItemsModel> RequestItems
        {
            get => _requestItems;
            set => SetProperty(ref _requestItems, value);
        }



    }
}
