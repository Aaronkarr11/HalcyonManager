using HalcyonCore.Clients;
using HalcyonCore.Interfaces;
using HalcyonCore.SharedEntities;
using Newtonsoft.Json;


namespace HalcyonManager.ViewModels
{
    public class HouseHoldManagmentViewModel : BaseViewModel
    {
        private IHalcyonManagementClient _transactionServices;
        public Command LoadItemsCommand { get; }
        public Command AddMemberCommand { get; }

        public Command OnRefreshCommand { get; }

        public Command<RequestItemResponse> ItemTapped { get; }

        public Command DeleteCommand { get; }
        public Command EditCommand { get; }
        public Command ExecuteNewMemberCommand { get; }

        public HouseHoldManagmentViewModel(IHalcyonManagementClient transactionServices)
        {
            _transactionServices = transactionServices;

            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            OnRefreshCommand = new Command(async () => await ExecuteLoadItemsCommand());


            ExecuteNewMemberCommand = new Command(() =>
            {
                ExecuteNewMember();
            });

            EditCommand = new Command((sender) =>
            {
                ExecuteEditHouseHoldCommand(sender);
            });

            DeleteCommand = new Command((obj) =>
            {
                OnDelete(obj);
            });


        }

        async void ExecuteNewMember()
        {
            HouseHoldMember member = new HouseHoldMember();
            var navigationParameter = new Dictionary<string, object>
                    {
                            { "Member", member }
                    };
            await Shell.Current.GoToAsync($"HouseHoldMemberPage", navigationParameter);
        }

        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;
            ShowMessage = false;

            try
            {

                HouseHoldList = await _transactionServices.GetHouseHoldMembers(DeviceInfo.Name.RemoveSpecialCharacters());
                if (HouseHoldList.Count() == 0)
                {
                    ShowMessage = true;
                }
            }
            catch (Exception ex)
            {
                ErrorLogModel error = Helpers.ReturnErrorMessage(ex, "HouseHoldManagmentViewModel", "ExecuteLoadItemsCommand");
                await _transactionServices.AzureFunctionPostTransaction("https://halcyontransactions.azurewebsites.net/api/CreateOrUpdateErrorLog?code=L9qTodcWmd_SyBsd5tGJucvCYhEY0gCzn4EMW0BM5rpXAzFuwcCuBQ==", JsonConvert.SerializeObject(error));
                App._alertSvc.ShowAlert("Exception!", $"{ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async void OnAppearing()
        {
            IsBusy = true;
            ShowMessage = false;

            try
            {
                HouseHoldList = await _transactionServices.GetHouseHoldMembers(DeviceInfo.Name.RemoveSpecialCharacters());
                if (HouseHoldList.Count() == 0 || HouseHoldList == null)
                {
                    ShowMessage = true;
                }
            }
            catch (Exception ex)
            {
                ErrorLogModel error = Helpers.ReturnErrorMessage(ex, "HouseHoldManagmentViewModel", "OnAppearing");
                await _transactionServices.AzureFunctionPostTransaction("https://halcyontransactions.azurewebsites.net/api/CreateOrUpdateErrorLog?code=L9qTodcWmd_SyBsd5tGJucvCYhEY0gCzn4EMW0BM5rpXAzFuwcCuBQ==", JsonConvert.SerializeObject(error));
                App._alertSvc.ShowAlert("Exception!", $"{ex.Message}");
            }
        }

        async void ExecuteEditHouseHoldCommand(object sender)
        {
            try
            {
                var houseHold = (HouseHoldMember)sender;
                HouseHoldMember sentHouseHold = new HouseHoldMember
                {
                    PartitionKey = houseHold.PartitionKey,
                    RowKey = houseHold.RowKey,
                    Name = houseHold.Name,
                    Email = houseHold.Email,
                    PhoneNumber = houseHold.PhoneNumber.RemoveSpecialCharacters()
                };
                var navigationParameter = new Dictionary<string, object>
                    {
                            { "WorkTask", sentHouseHold }
                    };
                await Shell.Current.GoToAsync($"HouseHoldMemberPage", navigationParameter);
            }
            catch (Exception ex)
            {
                ErrorLogModel error = Helpers.ReturnErrorMessage(ex);
                await _transactionServices.AzureFunctionPostTransaction("https://halcyontransactions.azurewebsites.net/api/CreateOrUpdateErrorLog?code=L9qTodcWmd_SyBsd5tGJucvCYhEY0gCzn4EMW0BM5rpXAzFuwcCuBQ==", JsonConvert.SerializeObject(error));
                App._alertSvc.ShowAlert("Exception!", $"{ex.Message}");
            }
        }

        private void OnDelete(object obj)
        {

            App._alertSvc.ShowConfirmation("Error", "Are you sure you want to delete?", (async result =>
            {
                if (result)
                {
                    try
                    {
                        HouseHoldMember member = (HouseHoldMember)obj;
                        member.DeviceName = DeviceInfo.Name.RemoveSpecialCharacters();
                        string uri = "https://halcyontransactions.azurewebsites.net/api/DeleteHouseHold?code=n7EHSk--bRA4C7USHoYZvGTNNNMonQjReMqODyD9ViYHAzFuVA5zpA==";
                        await _transactionServices.AzureFunctionPostTransaction(uri, JsonConvert.SerializeObject(member));
                        HouseHoldList = await _transactionServices.GetHouseHoldMembers(DeviceInfo.Name.RemoveSpecialCharacters());
                    }
                    catch (Exception ex)
                    {
                        ErrorLogModel error = Helpers.ReturnErrorMessage(ex, "HouseHoldManagmentViewModel", "OnDelete");
                        await _transactionServices.AzureFunctionPostTransaction("https://halcyontransactions.azurewebsites.net/api/CreateOrUpdateErrorLog?code=L9qTodcWmd_SyBsd5tGJucvCYhEY0gCzn4EMW0BM5rpXAzFuwcCuBQ==", JsonConvert.SerializeObject(error));
                        App._alertSvc.ShowAlert("Exception!", $"{ex.Message}");
                    }
                }

            }));
        }


        private List<HouseHoldMember> _houseHoldList;
        public List<HouseHoldMember> HouseHoldList
        {
            get => _houseHoldList;
            set => SetProperty(ref _houseHoldList, value);
        }

        private HouseHoldMember _selectedHouseHoldMember;
        public HouseHoldMember SelectedHouseHoldMember
        {
            get => _selectedHouseHoldMember;
            set => SetProperty(ref _selectedHouseHoldMember, value);
        }



        private bool _showMessage;
        public bool ShowMessage
        {
            get => _showMessage;
            set => SetProperty(ref _showMessage, value);
        }


    }
}
