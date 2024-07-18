using HalcyonCore.Clients;
using HalcyonCore.Interfaces;
using HalcyonCore.SharedEntities;
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
                ErrorLogModel error = Helpers.ReturnErrorMessage(ex, "WorkTaskPrioritiesViewModel", "OnAppearing");
                await _transactionServices.AzureFunctionPostTransaction("https://halcyontransactions.azurewebsites.net/api/CreateOrUpdateErrorLog?code=fXB5yroHKAH8GBb3M9VouDv2WTNjOR0AeBa_McAn6i6bAzFuJ2yxJg%3D%3D", JsonConvert.SerializeObject(error));
                App._alertSvc.ShowAlert("Exception!", $"{ex.Message}");
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
                    Assignment = workTask?.Assignment.Trim(),
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
            catch (Exception ex)
            {
                ErrorLogModel error = Helpers.ReturnErrorMessage(ex, "WorkTaskPrioritiesViewModel", "OnAppearing");
                await _transactionServices.AzureFunctionPostTransaction("https://halcyontransactions.azurewebsites.net/api/CreateOrUpdateErrorLog?code=fXB5yroHKAH8GBb3M9VouDv2WTNjOR0AeBa_McAn6i6bAzFuJ2yxJg%3D%3D", JsonConvert.SerializeObject(error));
                App._alertSvc.ShowAlert("Exception!", $"{ex.Message}");
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
