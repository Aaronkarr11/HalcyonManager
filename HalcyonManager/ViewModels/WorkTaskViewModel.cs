using CommunityToolkit.Mvvm.ComponentModel;
using HalcyonCore.Clients;
using HalcyonCore.Interfaces;
using HalcyonCore.SharedEntities;
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
            DeviceFontSize = Helpers.ReturnDeviceFontSize();

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

        private WorkTaskModel _workTask;
        public WorkTaskModel WorkTask
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

        public async void LoadItemId(WorkTaskModel rawWorkTask)
        {
            try
            {
                HouseHoldMembers = await GetHouseHold();
                SelectedWorkTask = rawWorkTask;
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
                ErrorLogModel error = Helpers.ReturnErrorMessage(ex, "WorkTaskViewModel", "LoadItemId");
                await _transactionServices.AzureFunctionPostTransaction("https://halcyontransactions.azurewebsites.net/api/CreateOrUpdateErrorLog?code=L9qTodcWmd_SyBsd5tGJucvCYhEY0gCzn4EMW0BM5rpXAzFuwcCuBQ==", JsonConvert.SerializeObject(error));
                App._alertSvc.ShowAlert("Exception!", $"{ex.Message}");
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

            try
            {
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
            catch (Exception ex)
            {
                ErrorLogModel error = Helpers.ReturnErrorMessage(ex, "WorkTaskViewModel", "GetHouseHold");
                await _transactionServices.AzureFunctionPostTransaction("https://halcyontransactions.azurewebsites.net/api/CreateOrUpdateErrorLog?code=L9qTodcWmd_SyBsd5tGJucvCYhEY0gCzn4EMW0BM5rpXAzFuwcCuBQ==", JsonConvert.SerializeObject(error));
                App._alertSvc.ShowAlert("Exception!", $"{ex.Message}");
                return newList;
            }
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
                        string uri = "https://halcyontransactions.azurewebsites.net/api/CreateOrUpdateWorkTask?code=fS1CcIz4Z6wuGSwknRMem2YrXsve5-fMbHLaevBZWuHFAzFuNIGQfQ==";
                        await _transactionServices.AzureFunctionPostTransaction(uri, JsonConvert.SerializeObject(workTask));
                        await Shell.Current.GoToAsync("..");
                    }
                    catch (Exception ex)
                    {
                        ErrorLogModel error = Helpers.ReturnErrorMessage(ex, "WorkTaskViewModel", "OnDelete");
                        await _transactionServices.AzureFunctionPostTransaction("https://halcyontransactions.azurewebsites.net/api/CreateOrUpdateErrorLog?code=L9qTodcWmd_SyBsd5tGJucvCYhEY0gCzn4EMW0BM5rpXAzFuwcCuBQ==", JsonConvert.SerializeObject(error));
                        App._alertSvc.ShowAlert("Exception!", $"{ex.Message}");
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
                        string uri = "https://halcyontransactions.azurewebsites.net/api/CreateOrUpdateWorkTask?code=fS1CcIz4Z6wuGSwknRMem2YrXsve5-fMbHLaevBZWuHFAzFuNIGQfQ==";
                        await _transactionServices.AzureFunctionPostTransaction(uri, JsonConvert.SerializeObject(workTask));
                        await Shell.Current.GoToAsync("..");
                    }
                    catch (Exception ex)
                    {
                        ErrorLogModel error = Helpers.ReturnErrorMessage(ex, "WorkTaskViewModel", "OnComplete");
                        await _transactionServices.AzureFunctionPostTransaction("https://halcyontransactions.azurewebsites.net/api/CreateOrUpdateErrorLog?code=L9qTodcWmd_SyBsd5tGJucvCYhEY0gCzn4EMW0BM5rpXAzFuwcCuBQ==", JsonConvert.SerializeObject(error));
                        App._alertSvc.ShowAlert("Exception!", $"{ex.Message}");
                    }
                }

            }));
        }


        private async void OnSave(object obj)
        {
            try
            {
                WorkTaskViewModel rawWorkTaskViewModel = (WorkTaskViewModel)obj;
                WorkTaskModel workTask = rawWorkTaskViewModel.SelectedWorkTask;
                workTask.Name = String.IsNullOrEmpty(workTask.Name) ? "NA" : workTask.Name;
                if (HouseHoldMembersList.Count != 0)
                {
                    if (workTask.Name == "0")
                    {
                        workTask.Name = "NA";
                    }
                    else
                    {
                        var blorb = HouseHoldMembersList[Convert.ToInt32(workTask.Name)];
                        workTask.PhoneNumber = blorb.PhoneNumber;
                        workTask.Name = blorb.Name;
                        workTask.Email = blorb.Email;
                    }
                }
                workTask.Completed = 0;
                workTask.DeviceName = DeviceInfo.Name.RemoveSpecialCharacters();
                string url = "https://halcyontransactions.azurewebsites.net/api/CreateOrUpdateWorkTask?code=fS1CcIz4Z6wuGSwknRMem2YrXsve5-fMbHLaevBZWuHFAzFuNIGQfQ==";
                var pagger = JsonConvert.SerializeObject(workTask);
                var pog = _transactionServices.AzureFunctionPostTransaction(url, pagger);
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                ErrorLogModel error = Helpers.ReturnErrorMessage(ex, "WorkTaskViewModel", "OnSave");
                await _transactionServices.AzureFunctionPostTransaction("https://halcyontransactions.azurewebsites.net/api/CreateOrUpdateErrorLog?code=L9qTodcWmd_SyBsd5tGJucvCYhEY0gCzn4EMW0BM5rpXAzFuwcCuBQ==", JsonConvert.SerializeObject(error));
                App._alertSvc.ShowAlert("Exception!", $"{ex.Message}");
            }
        }
    }
}
