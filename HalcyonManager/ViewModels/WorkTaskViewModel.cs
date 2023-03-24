using CommunityToolkit.Mvvm.ComponentModel;
using Halcyon.Clients;
using HalcyonManagement.Entities;
using HalcyonSoft.SharedEntities;
using Newtonsoft.Json;

namespace HalcyonManager.ViewModels
{
    [QueryProperty(nameof(WorkTask), nameof(WorkTask))]
    public class WorkTaskViewModel : BaseViewModel
    {
        private IHalcyonManagementClient _transactionServices;

        public WorkTaskViewModel(IHalcyonManagementClient transactionServices)
        {
            HouseHoldMembers = new List<string>();

            _transactionServices = transactionServices;
            CancelCommand = new Command(OnCancel);

            SaveCommand = new Command((obj) =>
            {
                OnSave(obj);
            });

            DeleteCommand = new Command((obj) =>
            {
                OnDelete(obj);
            });

            CompleteCommand = new Command((obj) =>
            {
                OnComplete(obj);
            });


            ReselectStateColorCommand = new Command((name) =>
            {
                Picker picker = (Picker)name;
                StateColor = SetStateColor(picker.SelectedItem.ToString());
            });

            this.PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();
            BuildDropDownLists();
        }

        private string _workTask;
        public string WorkTask
        {
            get
            {
                return _workTask;
            }
            set
            {
                _workTask = value;
                LoadItemId(value);
            }
        }

        public async void LoadItemId(string rawWorkTask)
        {
            try
            {  
                HouseHoldMembers = await GetHouseHold();
                SelectedWorkTask = JsonConvert.DeserializeObject<WorkTaskModel>(rawWorkTask);
                if (String.IsNullOrEmpty(SelectedWorkTask.Name))
                {
                    SelectedWorkTask.Name = "N/A";
                }

                if (String.IsNullOrEmpty(SelectedWorkTask.PartitionKey) && String.IsNullOrEmpty(SelectedWorkTask.RowKey))
                {
                    SelectedWorkTask.PartitionKey = System.Guid.NewGuid().ToString();
                    SelectedWorkTask.RowKey = System.Guid.NewGuid().ToString();
                    Name = $"Create a New Work Task";
                    ShowDeleteButton = false;
                }
                else
                {
                    Name = $"Edit WorkTask: {SelectedWorkTask.Title}";
                    ShowDeleteButton = true;
                }

                StateColor = SetStateColor(SelectedWorkTask.State);


            }
            catch (Exception ex)
            {

            }
        }

        private WorkTaskModel _selectedWorkTask;
        public WorkTaskModel SelectedWorkTask
        {
            get => _selectedWorkTask;
            set => SetProperty(ref _selectedWorkTask, value);
        }

        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private List<int> _priorityList;
        public List<int> PriorityList
        {
            get => _priorityList;
            set => SetProperty(ref _priorityList, value);
        }

        private List<string> _riskList;
        public List<string> RiskList
        {
            get => _riskList;
            set => SetProperty(ref _riskList, value);
        }

        private Color _stateColor;
        public Color StateColor
        {
            get => _stateColor;
            set => SetProperty(ref _stateColor, value);
        }

        private List<int> _effortList;
        public List<int> EffortList
        {
            get => _effortList;
            set => SetProperty(ref _effortList, value);
        }

        private List<string> _stateList;
        public List<string> StateList
        {
            get => _stateList;
            set => SetProperty(ref _stateList, value);
        }

        private List<string> _houseHoldMembers;
        public List<string> HouseHoldMembers
        {
            get => _houseHoldMembers;
            set => SetProperty(ref _houseHoldMembers, value);
        }

        private List<HouseHoldMember> _houseHoldMembersList;
        public List<HouseHoldMember> HouseHoldMembersList
        {
            get => _houseHoldMembersList;
            set => SetProperty(ref _houseHoldMembersList, value);
        }

        private bool _showDeleteButton;
        public bool ShowDeleteButton
        {
            get => _showDeleteButton;
            set => SetProperty(ref _showDeleteButton, value);
        }

        public Color SetStateColor(string state)
        {
            return state switch
            {
                "New" => Color.FromRgb(255, 204, 0),
                "In Progress" => Color.FromRgb(0, 72, 255),
                "Done" => Color.FromRgb(0, 102, 0),
                _ => Color.FromRgb(0, 72, 255),
            };
        }

        public async Task<List<string>> GetHouseHold()
        {
            List<string> newList = new List<string>();

            List<HouseHoldMember> RawHouseHoldMembersList;
            RawHouseHoldMembersList = await _transactionServices.GetHouseHoldMembers(DeviceInfo.Name.RemoveSpecialCharacters());
            HouseHoldMembersList = RawHouseHoldMembersList.ToList();
            if (RawHouseHoldMembersList.Count() != 0)
            {
                foreach (var member in RawHouseHoldMembersList)
                {
                    newList.Add(member.Name.Trim());
                }
            }
            newList.Add("N/A");
            return newList;
        }




        public void BuildDropDownLists()
        {

            StateList = new List<string>
            {
                "New",
                "In Progress",
                "Done"
            };


            PriorityList = new List<int>
            { 1, 2, 3, 4};

            EffortList = new List<int>
            { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10};

            RiskList = new List<string>
            {
                "1 - High",
                "2 - Medium",
                "3 - Low"
            };

        }

        public Command SaveCommand { get; }
        public Command DeleteCommand { get; }
        public Command CancelCommand { get; }
        public Command CompleteCommand { get; }

        public Command ReselectStateColorCommand { get; }

        private async void OnCancel()
        {
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }

        private async void OnDelete(object obj)
        {

            App._alertSvc.ShowConfirmation("Warning", "Are you sure you want to delete?", (async result =>
            {
                if (result)
                {
                    try
                    {
                        WorkTaskViewModel rawWorkTaskViewModel = (WorkTaskViewModel)obj;
                        WorkTaskModel workTask = rawWorkTaskViewModel.SelectedWorkTask;
                        workTask.Completed = 0;
                        workTask.DeviceName = DeviceInfo.Name.RemoveSpecialCharacters();
                        string uri = "https://projecthalcyonmanagmenttransactions.azurewebsites.net/api/DeleteWorkTask?code=6IW3yugTiy72LYXfsFZ9NKrTpTaLwBKSKflKxZUbI/7KUxto1d9Egw==";
                        await _transactionServices.AzureFunctionPostTransaction(uri, JsonConvert.SerializeObject(workTask));
                        await Shell.Current.GoToAsync("..");
                    }
                    catch (Exception ex)
                    {
                        App._alertSvc.ShowConfirmation("Error", $"{ex.Message}", (result => { App._alertSvc.ShowAlert("Result", $"{result}"); }));
                    }
                }

            }));
        }

        private void OnComplete(object obj)
        {
            App._alertSvc.ShowConfirmation("Warning", "Are you sure you want to complete this worktask?", (async result =>
            {
                if (result)
                {
                    try
                    {
                        WorkTaskViewModel rawWorkTaskViewModel = (WorkTaskViewModel)obj;
                        WorkTaskModel workTask = rawWorkTaskViewModel.SelectedWorkTask;
                        workTask.Completed = 1;
                        workTask.DeviceName = DeviceInfo.Name.RemoveSpecialCharacters();
                        string uri = "https://projecthalcyonmanagmenttransactions.azurewebsites.net/api/DeleteWorkTask?code=6IW3yugTiy72LYXfsFZ9NKrTpTaLwBKSKflKxZUbI/7KUxto1d9Egw==";
                        await _transactionServices.AzureFunctionPostTransaction(uri, JsonConvert.SerializeObject(workTask));
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
            WorkTaskViewModel rawWorkTaskViewModel = (WorkTaskViewModel)obj;
            WorkTaskModel workTask = rawWorkTaskViewModel.SelectedWorkTask;
            workTask.Name = String.IsNullOrEmpty(workTask.Name) ? "N/A" : workTask.Name;

            var selectedMember = HouseHoldMembersList.Where(h => h.Name.Trim() == workTask.Name.Trim()).FirstOrDefault();
            if (selectedMember != null)
            {
                workTask.PhoneNumber = selectedMember.PhoneNumber;
                workTask.Name = selectedMember.Name;
                workTask.Email = selectedMember.Email;
            }

            workTask.Completed = 0;
            workTask.DeviceName = DeviceInfo.Name.RemoveSpecialCharacters();
            string uri = "https://projecthalcyonmanagmenttransactions.azurewebsites.net/api/CreateWorkTask?code=3jM7Httk73UcdxbIVdiFM7231TbUnZdbWqyTl1tXFRwfEcpd/Dx//g==";
            await _transactionServices.AzureFunctionPostTransaction(uri, JsonConvert.SerializeObject(workTask));
            await Shell.Current.GoToAsync("..");
        }
    }
}
