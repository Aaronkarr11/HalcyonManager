using HalcyonSoft.Clients;
using HalcyonSoft.Interfaces;
using HalcyonSoft.SharedEntities;
using Newtonsoft.Json;

namespace HalcyonManager.ViewModels
{
    [QueryProperty(nameof(WorkTask), nameof(WorkTask))]
    public class WorkTaskPrioritiesViewModel : BaseViewModel
    {
        private IHalcyonManagementClient _transactionServices;
        private WorkTaskModel _selectedWorkTask;

        public Command EditWorkTaskCommand { get; private set; }

        public WorkTaskPrioritiesViewModel(IHalcyonManagementClient transactionServices)
        {
            _transactionServices = transactionServices;
            DeviceFontSize = Helpers.ReturnDeviceFontSize();

            EditWorkTaskCommand = new Command((workTask) =>
            {
                ExecuteEditWorkTaskCommand(workTask);
            });
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
                ExecuteEditWorkTaskCommand(value);
            }
        }


        public async void OnAppearing()
        {
          IsBusy = true;
            try
            {
                WorkTaskList = await _transactionServices.GetWorkTaskPrioritiesList(DeviceInfo.Name.RemoveSpecialCharacters());
                IsBusy = false;
            }
            catch (Exception ex)
            {
                App._alertSvc.ShowConfirmation("Error", $"{ex.Message}", (result =>
                {
                    App._alertSvc.ShowAlert("Result", $"{result}");
                }));
            }
       
        }

        async void ExecuteEditWorkTaskCommand(object sender)
        {
            var workTask = (WorkTaskModel)sender;
            try
            {
                WorkTaskModel workTaskModel = new WorkTaskModel
                {
                    Title = workTask.Title,
                    Name = workTask?.Name.Trim(),
                    Risk = workTask?.Risk ?? "3 - Low",
                    SendSMS = workTask.SendSMS,
                    RowKey = workTask?.RowKey,
                    State = workTask?.State,
                    PartitionKey = workTask?.PartitionKey,
                    Effort = workTask?.Effort == 0 ? 1 : workTask.Effort,
                    ParentPartitionKey = workTask?.ParentPartitionKey,
                    ParentRowKey = workTask?.ParentRowKey,
                    Priority = workTask?.Priority == 0 ? 1 : workTask.Priority,
                    StartDate = workTask.StartDate,
                    TargetDate = workTask.TargetDate,
                    Description = workTask?.Description,
                    Completed = 0
                };
                var navigationParameter = new Dictionary<string, object>
                    {
                            { "WorkTask", workTaskModel }
                    };
                await Shell.Current.GoToAsync($"WorkTaskPage", navigationParameter);
            }
            catch (Exception ee)
            {

                throw;
            }

        }

        private List<WorkTaskModel> _workTaskList;
        public List<WorkTaskModel> WorkTaskList
        {
            get => _workTaskList;
            set => SetProperty(ref _workTaskList, value);
        }
    }
}
