using HalcyonCore.Clients;
using HalcyonCore.Interfaces;
using HalcyonCore.SharedEntities;
using Newtonsoft.Json;

namespace HalcyonManager.ViewModels
{
    [QueryProperty(nameof(Member), nameof(Member))]
    public class HouseHoldMemberViewModel : BaseViewModel
    {

        private IHalcyonManagementClient _transactionServices;

        public HouseHoldMemberViewModel(IHalcyonManagementClient transactionServices)
        {
            _transactionServices = transactionServices;
            CancelCommand = new Command(OnCancel);

            SaveCommand = new Command((obj) =>
            {
                OnSave(obj);
                ValidateSave();
            });


            this.PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();

        }

        private bool ValidateSave()
        {
            return !String.IsNullOrWhiteSpace(_selectedHouseHoldMember.Name);
        }

        private HouseHoldMember _member;
        public HouseHoldMember Member
        {
            get
            {
                return _member;
            }
            set
            {
                _member = value;
                LoadItemId(value);
            }
        }

        public async void LoadItemId(HouseHoldMember SelectedHouseHoldMember)
        {
            try
            {

                if (String.IsNullOrEmpty(SelectedHouseHoldMember.PartitionKey) && String.IsNullOrEmpty(SelectedHouseHoldMember.RowKey))
                {
                    SelectedHouseHoldMember.PartitionKey = System.Guid.NewGuid().ToString();
                    SelectedHouseHoldMember.RowKey = System.Guid.NewGuid().ToString();
                    Name = $"Create a New HouseHold Member";
                    ShowDeleteButton = false;
                }
                else
                {
                    Name = $"Edit HouseHold Member: {SelectedHouseHoldMember.Name}";
                    ShowDeleteButton = true;
                }
            }
            catch (Exception ex)
            {
                ErrorLogModel error = Helpers.ReturnErrorMessage(ex, "HouseHoldMemberViewModel", "LoadItemId");
                await _transactionServices.AzureFunctionPostTransaction("https://halcyontransactions.azurewebsites.net/api/CreateOrUpdateErrorLog?code=L9qTodcWmd_SyBsd5tGJucvCYhEY0gCzn4EMW0BM5rpXAzFuwcCuBQ==", JsonConvert.SerializeObject(error));
                App._alertSvc.ShowAlert("Exception!", $"{ex.Message}");
            }
        }

        private HouseHoldMember _selectedHouseHoldMember;
        public HouseHoldMember SelectedHouseHoldMember
        {
            get => _selectedHouseHoldMember;
            set => SetProperty(ref _selectedHouseHoldMember, value);
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

        private async void OnCancel()
        {
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }

        private async void OnSave(object obj)
        {

            try
            {
                HouseHoldMemberViewModel rawHouseHoldViewModel = (HouseHoldMemberViewModel)obj;
                HouseHoldMember operation = rawHouseHoldViewModel.SelectedHouseHoldMember;
                operation.DeviceName = DeviceInfo.Name.RemoveSpecialCharacters();
                string uri = "https://halcyontransactions.azurewebsites.net/api/CreateOrUpdateHouseHold?code=aif1grIlMPqD97ETmutltxOPRYUx0YB0kKjWK_M73V0zAzFuqr8g5w==";
                await _transactionServices.AzureFunctionPostTransaction(uri, JsonConvert.SerializeObject(operation));
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                ErrorLogModel error = Helpers.ReturnErrorMessage(ex, "HouseHoldMemberViewModel", "OnSave");
                await _transactionServices.AzureFunctionPostTransaction("https://halcyontransactions.azurewebsites.net/api/CreateOrUpdateErrorLog?code=L9qTodcWmd_SyBsd5tGJucvCYhEY0gCzn4EMW0BM5rpXAzFuwcCuBQ==", JsonConvert.SerializeObject(error));
                App._alertSvc.ShowAlert("Exception!", $"{ex.Message}");
            }
        }
    }
}
