using HalcyonSoft.Clients;

using HalcyonSoft.SharedEntities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HalcyonManager.ViewModels
{
    public class WorkItemManagmentViewModel : BaseViewModel
    {
        private IHalcyonManagementClient _transactionServices;
        private OperationModel _selectedOperation;
        public Command LoadWorkItemCommand { get; }
        public Command AddItemCommand { get; }

        public Command OnRefreshCommand { get; }

        public Command<RequestItemResponse> ItemTapped { get; }

        public ICommand GetSelectedOperationsCommand { get; private set; }

        public Command EditWorkTaskCommand { get; private set; }

        public Command EditProjectCommand { get; private set; }
        public Command NewProjectCommand { get; private set; }
        public Command NewWorkTaskCommand { get; private set; }
        public Command EditOperationCommand { get; private set; }

        public WorkItemManagmentViewModel(IHalcyonManagementClient transactionServices)
        {
            ShowPicker = false;
            _transactionServices = transactionServices;
            _selectedOperation = null;
            OperationList = new List<OperationModel>();
            WorkItemHierarchy = new List<OperationHierarchy>();
            DeviceFontSize = Helpers.ReturnDeviceFontSize();
            DeviceButtonWidth = Helpers.ReturnDeviceButtonWidth();

            EditWorkTaskCommand = new Command((workTask) =>
            {
                ExecuteEditWorkTaskCommand(workTask);
            });

            EditProjectCommand = new Command((project) =>
            {
                ExecuteEditProjectCommand(project);
            });

            NewProjectCommand = new Command((operation) =>
            {
                ExecuteNewProjectCommand(operation);
            });

            NewWorkTaskCommand = new Command((sender) =>
            {
                ExecuteNewWorkTaskCommand(sender);
            });

            EditOperationCommand = new Command((operation) =>
            {
                ExecuteEditOperationCommand(operation);
            });

            GetSelectedOperationsCommand = new Command((name) =>
            {
                Picker picker = (Picker)name;
                GetWorkTaskHierarchy((OperationModel)picker.SelectedItem);
            });
        }

        async void GetWorkTaskHierarchy(OperationModel item)
        {
            try
            {
                if (item != null)
                {
                    if (item.RowKey == "1")
                    {
                        ExecuteNewOperationCommand();
                    }
                    else
                    {
                        _selectedOperation = item;
                        var result = await _transactionServices.GetWorkItemHierarchy(DeviceInfo.Name.RemoveSpecialCharacters());
                        WorkItemHierarchy = result.Where(e => e.PartitionKey == item.PartitionKey && e.RowKey == item.RowKey).ToList();

                        if (String.IsNullOrEmpty(SelectedOperation))
                        {
                            SelectedOperation = "Work Item Management ";
                        }
                        else
                        {
                            SelectedOperation = "Work Item Management: " + item.Title;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        async void ExecuteEditWorkTaskCommand(object sender)
        {


            try
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
                var result = JsonConvert.SerializeObject(workTaskModel);
                var navigationParameter = new Dictionary<string, object>
                    {
                            { "WorkTask", workTaskModel }
                    };
                await Shell.Current.GoToAsync($"WorkTaskPage", navigationParameter);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        async void ExecuteNewWorkTaskCommand(object sender)
        {
            try
            {
                var parentProject = (ProjectHierarchy)sender;

                WorkTaskModel workTaskModel = new WorkTaskModel
                {
                    ParentPartitionKey = parentProject.PartitionKey,
                    ParentRowKey = parentProject.RowKey,
                    State = "New",
                    Risk = "3 - Low",
                    SendSMS = false,
                    Effort = 1,
                    Priority = 4,
                    StartDate = DateTime.Now,
                    TargetDate = DateTime.Now,
                    Completed = 0
                };
                var result = JsonConvert.SerializeObject(workTaskModel);
                var navigationParameter = new Dictionary<string, object>
                    {
                            { "WorkTask", workTaskModel }
                    };
                await Shell.Current.GoToAsync($"WorkTaskPage", navigationParameter);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        async void ExecuteNewProjectCommand(object sender)
        {
            var parentOperation = (OperationHierarchy)sender;

            ProjectModel projectModel = new ProjectModel
            {
                ParentPartitionKey = parentOperation.PartitionKey,
                ParentRowKey = parentOperation.RowKey,
                StartDate = DateTime.Now,
                TargetDate = DateTime.Now,
                Priority = 4,
                Severity = "4 - Low",
                State = "New",
                LocationCategory = "Whole House",
                Completed = 0

            };

            var navigationParameter = new Dictionary<string, object>
                    {
                            { "Project", projectModel }
                    };
            await Shell.Current.GoToAsync($"ProjectPage", navigationParameter);
        }

        async void ExecuteNewOperationCommand()
        {

            OperationModel operationModel = new OperationModel
            {
                StartDate = DateTime.Now,
                TargetDate = DateTime.Now,
                Icon = "icon.png",
                Completed = 0
            };

            var navigationParameter = new Dictionary<string, object>
                    {
                            { "Operation", operationModel }
                    };
            await Shell.Current.GoToAsync($"OperationPage", navigationParameter);
        }

        async void ExecuteEditOperationCommand(object sender)
        {
            var operation = (OperationHierarchy)sender;
            OperationModel operationModel = new OperationModel
            {
                PartitionKey = operation.PartitionKey,
                RowKey = operation.RowKey,
                StartDate = operation.StartDate,
                TargetDate = operation.TargetDate,
                Title = operation.Title,
                Description = operation.Description,
                Icon = operation.Icon,
                Completed = 0
            };

            var navigationParameter = new Dictionary<string, object>
                    {
                            { "Operation", operationModel }
                    };
            await Shell.Current.GoToAsync($"OperationPage", navigationParameter);
        }

        async void ExecuteEditProjectCommand(object sender)
        {


            var prog = (ProjectHierarchy)sender;
            ProjectModel projectModel = new ProjectModel
            {
                Title = prog.Title,
                Severity = prog.Severity ?? "4 - Low",
                RowKey = prog.RowKey,
                State = prog.State,
                PartitionKey = prog.PartitionKey,
                LocationCategory = prog.LocationCategory ?? "Whole House",
                ParentPartitionKey = prog.ParentPartitionKey,
                ParentRowKey = prog.ParentRowKey,
                Priority = prog.Priority == 0 ? 1 : prog.Priority,
                StartDate = prog.StartDate,
                TargetDate = prog.TargetDate,
                Description = prog.Description,
                Completed = 0
            };
            var navigationParameter = new Dictionary<string, object>
                    {
                            { "Project", projectModel }
                    };
            await Shell.Current.GoToAsync($"ProjectPage", navigationParameter);
        }

        public async void OnAppearing()
        {
            IsBusy = true;
            if (_selectedOperation != null)
            {
                OperationList = await _transactionServices.GetOperationList(DeviceInfo.Name.RemoveSpecialCharacters());
                GetWorkTaskHierarchy(_selectedOperation);
            }
            else
            {
                OperationList = await _transactionServices.GetOperationList(DeviceInfo.Name.RemoveSpecialCharacters());
            }

            if (String.IsNullOrEmpty(SelectedOperation))
            {
                SelectedOperation = "Work Item Management ";
            }
            else
            {
                SelectedOperation = "Work Item Management: " + _selectedOperation?.Title;
            }
            ShowPicker = true;
            IsBusy = false;
        }

        private List<OperationModel> _operationList;
        public List<OperationModel> OperationList
        {
            get => _operationList;
            set => SetProperty(ref _operationList, value);
        }


        private List<OperationHierarchy> _workItemHierarchy;
        public List<OperationHierarchy> WorkItemHierarchy
        {
            get => _workItemHierarchy;
            set => SetProperty(ref _workItemHierarchy, value);
        }

        private bool _showPicker;
        public bool ShowPicker
        {
            get => _showPicker;
            set => SetProperty(ref _showPicker, value);
        }

        private double _deviceButtonWidth;
        public double DeviceButtonWidth
        {
            get => _deviceButtonWidth;
            set => SetProperty(ref _deviceButtonWidth, value);
        }

        private string _retainedSelectedOperation;
        public string SelectedOperation
        {
            get => _retainedSelectedOperation;
            set => SetProperty(ref _retainedSelectedOperation, value);
        }


    }
}
