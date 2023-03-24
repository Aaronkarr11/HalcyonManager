using Halcyon.Clients;
using HalcyonManagement.Entities;
using HalcyonSoft.SharedEntities;
using Newtonsoft.Json;

namespace HalcyonManager.ViewModels
{
    public class WorkTaskPrioritiesViewModel : BaseViewModel
    {
        private IHalcyonManagementClient _transactionServices;
        private WorkTaskModel _selectedWorkTask;

        public Command EditWorkTaskCommand { get; private set; }

        public WorkTaskPrioritiesViewModel(IHalcyonManagementClient transactionServices)
        {
            _transactionServices = transactionServices;

            EditWorkTaskCommand = new Command((workTask) =>
            {
                ExecuteEditWorkTaskCommand(workTask);
            });
        }


        public async void OnAppearing()
        {
            IsBusy = true;
            try
            {
                WorkTaskList = await _transactionServices.GetWorkTaskPrioritiesList(DeviceInfo.Name.RemoveSpecialCharacters());
            }
            catch (Exception ex)
            {
                App._alertSvc.ShowConfirmation("Error", $"{ex.Message}", (result =>
                {
                    App._alertSvc.ShowAlert("Result", $"{result}");
                }));
            }
            IsBusy = false;
        }

        async void ExecuteEditWorkTaskCommand(object sender)
        {
            var workTask = (WorkTaskModel)sender;
            WorkTaskModel workTaskModel = new WorkTaskModel
            {
                Title = workTask.Title,
                Name = workTask.Name.Trim(),
                Risk = workTask.Risk ?? "3 - Low",
                SendSMS = workTask.SendSMS,
                RowKey = workTask.RowKey,
                State = workTask.State,
                PartitionKey = workTask.PartitionKey,
                Effort = workTask.Effort == 0 ? 1 : workTask.Effort,
                ParentPartitionKey = workTask.ParentPartitionKey,
                ParentRowKey = workTask.ParentRowKey,
                Priority = workTask.Priority == 0 ? 1 : workTask.Priority,
                StartDate = workTask.StartDate,
                TargetDate = workTask.TargetDate,
                Description = workTask.Description,
                Completed = 0
            };
            await Shell.Current.GoToAsync($"WorkTaskPage?WorkTask={JsonConvert.SerializeObject(workTaskModel)}");
        }

        private List<WorkTaskModel> _workTaskList;
        public List<WorkTaskModel> WorkTaskList
        {
            get => _workTaskList;
            set => SetProperty(ref _workTaskList, value);
        }
    }
}
