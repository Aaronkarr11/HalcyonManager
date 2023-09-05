using Halcyon.Clients;
using HalcyonSoft.SharedEntities;
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

        private HouseHoldMemberTableTemplate _member;
        public HouseHoldMemberTableTemplate Member
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

        public void LoadItemId(HouseHoldMemberTableTemplate SelectedHouseHoldMember)
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
                App._alertSvc.ShowConfirmation("Error", $"{ex.Message}", (result =>
                {
                    App._alertSvc.ShowAlert("Result", $"{result}");
                }));
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

            HouseHoldMemberViewModel rawHouseHoldViewModel = (HouseHoldMemberViewModel)obj;
            HouseHoldMember operation = rawHouseHoldViewModel.SelectedHouseHoldMember;
            operation.DeviceName = DeviceInfo.Name.RemoveSpecialCharacters();
            string uri = "https://projecthalcyonmanagmenttransactions.azurewebsites.net/api/CreateHouseHold?code=0V1VDO79GoHZmK9ay5V8Ds6MUeFCKHHY15FLbKa6l4t5az1BaEUPcQ==";
            await _transactionServices.AzureFunctionPostTransaction(uri, JsonConvert.SerializeObject(operation));
            await Shell.Current.GoToAsync("..");
        }
    }
}
