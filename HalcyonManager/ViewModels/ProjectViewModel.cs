using HalcyonSoft.Clients;

using HalcyonSoft.SharedEntities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalcyonManager.ViewModels
{
    [QueryProperty(nameof(Project), nameof(Project))]
    public class ProjectViewModel : BaseViewModel
    {
        private ProjectModel _project;
        private IHalcyonManagementClient _transactionServices;

        public ProjectViewModel(IHalcyonManagementClient transactionServices, object project = null)
        {
            _transactionServices = transactionServices;
            CancelCommand = new Command(OnCancel);
            DeviceFontSize = Helpers.ReturnDeviceFontSize();

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


        public ProjectModel Project
        {
            get
            {
                return _project;
            }
            set
            {
                _project = value;
                LoadItemId(value);
            }
        }

        public void LoadItemId(ProjectModel project)
        {
            try
            {
               SelectedProject = project;

                if (String.IsNullOrEmpty(SelectedProject.PartitionKey) && String.IsNullOrEmpty(SelectedProject.RowKey))
                {
                    SelectedProject.PartitionKey = System.Guid.NewGuid().ToString();
                    SelectedProject.RowKey = System.Guid.NewGuid().ToString();
                    Name = $"Create a New Project";
                    ShowDeleteButton = false;
                }
                else
                {
                    Name = $"Edit Project: {SelectedProject.Title}";
                    ShowDeleteButton = true;
                }


                SeverityDisplayName = SelectedProject.Severity;

                StateColor = SetStateColor(SelectedProject.State);


            }
            catch (Exception ex)
            {

            }
        }

        private ProjectModel _selectedProject;
        public ProjectModel SelectedProject
        {
            get => _selectedProject;
            set => SetProperty(ref _selectedProject, value);
        }

        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private string _severityName;
        public string SeverityDisplayName
        {
            get => _severityName;
            set => SetProperty(ref _severityName, value);
        }

        private List<int> _priorityList;
        public List<int> PriorityList
        {
            get => _priorityList;
            set => SetProperty(ref _priorityList, value);
        }

        private List<string> _severityList;
        public List<string> SeverityList
        {
            get => _severityList;
            set => SetProperty(ref _severityList, value);
        }

        private List<string> _locationCategoryList;
        public List<string> LocationCategoryList
        {
            get => _locationCategoryList;
            set => SetProperty(ref _locationCategoryList, value);
        }

        private Color _stateColor;
        public Color StateColor
        {
            get => _stateColor;
            set => SetProperty(ref _stateColor, value);
        }

        private List<string> _stateList;
        public List<string> StateList
        {
            get => _stateList;
            set => SetProperty(ref _stateList, value);
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

            SeverityList = new List<string>
            {
                "1 - Critical",
                "2 - High",
                "3 - Medium",
                "4 - Low"
            };

            LocationCategoryList = new List<string>
            {
               "Attic",
               "Backyard",
               "Bedroom A",
               "Bedroom B",
               "Crawlspace",
               "Dining Room",
               "Foundation",
               "Front Yard",
               "Garage",
               "Guest Bathroom",
               "Hallway",
               "Garage",
               "Kitchen",
               "Laundry Room",
               "Living Room",
               "Master Bath",
               "Master Bedroom",
               "Roof",
               "Study",
               "Whole House",
               "Yards"
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

        private void OnDelete(object obj)
        {

            App._alertSvc.ShowConfirmation("Warning", "Are you sure you want to delete?", (async result =>
            {
                if (result)
                {
                    try
                    {
                        ProjectViewModel rawProjectViewModel = (ProjectViewModel)obj;
                        ProjectModel project = rawProjectViewModel.SelectedProject;
                        project.Completed = 0;
                        project.DeviceName = DeviceInfo.Name.RemoveSpecialCharacters();
                        string uri = "https://halcyontransactions.azurewebsites.net/api/DeleteOrCompleteProject?code=pXjfkwCmnacaaS7y0O2DAaNmX7XmXqlWvIvf1WVRP7hNAzFu7jzbUg==";
                        await _transactionServices.AzureFunctionPostTransaction(uri, JsonConvert.SerializeObject(project));
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

            App._alertSvc.ShowConfirmation("Warning", "Are you sure you want to complete this project? All child items will be marked as completed as well.", (async result =>
            {
                if (result)
                {
                    try
                    {
                        ProjectViewModel rawProjectViewModel = (ProjectViewModel)obj;
                        ProjectModel project = rawProjectViewModel.SelectedProject;
                        project.Completed = 1;
                        project.DeviceName = DeviceInfo.Name.RemoveSpecialCharacters();
                        string uri = "https://halcyontransactions.azurewebsites.net/api/DeleteOrCompleteProject?code=pXjfkwCmnacaaS7y0O2DAaNmX7XmXqlWvIvf1WVRP7hNAzFu7jzbUg==";
                        await _transactionServices.AzureFunctionPostTransaction(uri, JsonConvert.SerializeObject(project));
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
            ProjectViewModel rawProjectViewModel = (ProjectViewModel)obj;
            ProjectModel project = rawProjectViewModel.SelectedProject;
            project.Completed = 0;
            project.DeviceName = DeviceInfo.Name.RemoveSpecialCharacters();
            string uri = "https://halcyontransactions.azurewebsites.net/api/CreateOrUpdateProject?code=pXxnpM-HTEqO09hiojLhDlNTBvKbcNEO4q0k3oI3MdH9AzFuXqaI3g==";
            await _transactionServices.AzureFunctionPostTransaction(uri, JsonConvert.SerializeObject(project));
            await Shell.Current.GoToAsync("..");
        }
    }
}
