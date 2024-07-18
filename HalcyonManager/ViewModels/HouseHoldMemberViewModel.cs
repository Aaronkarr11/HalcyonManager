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
            });


            this.PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();

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

        public async void LoadItemId(HouseHoldMember mem)
        {
            try
            {
                SelectedHouseHoldMember = mem;
                if (String.IsNullOrEmpty(mem.PartitionKey) && String.IsNullOrEmpty(mem.RowKey))
                {
                    mem.PartitionKey = System.Guid.NewGuid().ToString();
                    mem.RowKey = System.Guid.NewGuid().ToString();
                    PageName = $"Create a New HouseHold Member";
                    ShowDeleteButton = false;
                }
                else
                {
                    PageName = $"Edit HouseHold Member: {SelectedHouseHoldMember.Name}";
                    ShowDeleteButton = true;
                }
            }
            catch (Exception ex)
            {
                ErrorLogModel error = Helpers.ReturnErrorMessage(ex, "HouseHoldMemberViewModel", "LoadItemId");
                await _transactionServices.AzureFunctionPostTransaction("https://halcyontransactions.azurewebsites.net/api/CreateOrUpdateErrorLog?code=fXB5yroHKAH8GBb3M9VouDv2WTNjOR0AeBa_McAn6i6bAzFuJ2yxJg%3D%3D", JsonConvert.SerializeObject(error));
                App._alertSvc.ShowAlert("Exception!", $"{ex.Message}");
            }
        }

        private HouseHoldMember _selectedHouseHoldMember;
        public HouseHoldMember SelectedHouseHoldMember
        {
            get => _selectedHouseHoldMember;
            set => SetProperty(ref _selectedHouseHoldMember, value);
        }

        private string _pagename;
        public string PageName
        {
            get => _pagename;
            set => SetProperty(ref _pagename, value);
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
                HouseHoldMember houseHold = rawHouseHoldViewModel.SelectedHouseHoldMember;
                if (Helpers.IsPhoneValid(houseHold))
                {       
                    houseHold.DeviceName = DeviceInfo.Name.RemoveSpecialCharacters();
                    string uri = "https://halcyontransactions.azurewebsites.net/api/CreateOrUpdateHouseHold?code=fXB5yroHKAH8GBb3M9VouDv2WTNjOR0AeBa_McAn6i6bAzFuJ2yxJg%3D%3D";
                    await _transactionServices.AzureFunctionPostTransaction(uri, JsonConvert.SerializeObject(houseHold));
                    await Shell.Current.GoToAsync("..");
                }
                else
                {
                    App._alertSvc.ShowAlert("Warning!", "Phone must be valid and contain 10 digits");
                }


            }
            catch (Exception ex)
            {
                ErrorLogModel error = Helpers.ReturnErrorMessage(ex, "HouseHoldMemberViewModel", "OnSave");
                await _transactionServices.AzureFunctionPostTransaction("https://halcyontransactions.azurewebsites.net/api/CreateOrUpdateErrorLog?code=fXB5yroHKAH8GBb3M9VouDv2WTNjOR0AeBa_McAn6i6bAzFuJ2yxJg%3D%3D", JsonConvert.SerializeObject(error));
                App._alertSvc.ShowAlert("Exception!", $"{ex.Message}");
            }
        }
    }
}
