using Halcyon.Clients;
using HalcyonManagement.Entities;
using HalcyonSoft.SharedEntities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalcyonManager.ViewModels
{
    public class HouseHoldManagmentViewModel : BaseViewModel
    {
        private IHalcyonManagementClient _transactionServices;
        public Command LoadItemsCommand { get; }
        public Command AddMemberCommand { get; }

        public Command OnRefreshCommand { get; }

        public Command<RequestItemResponse> ItemTapped { get; }

        public Command DeleteCommand { get; }
        public Command EditCommand { get; }
        public Command ExecuteNewMemberCommand { get; }

        public HouseHoldManagmentViewModel(IHalcyonManagementClient transactionServices)
        {
            _transactionServices = transactionServices;

            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            OnRefreshCommand = new Command(async () => await ExecuteLoadItemsCommand());


            ExecuteNewMemberCommand = new Command(() =>
            {
                ExecuteNewMember();
            });

            EditCommand = new Command((sender) =>
            {
                ExecuteEditOperationCommand(sender);
            });

            DeleteCommand = new Command((obj) =>
            {
                OnDelete(obj);
            });


        }

        async void ExecuteNewMember()
        {
            HouseHoldMember member = new HouseHoldMember();
            await Shell.Current.GoToAsync($"HouseHoldMemberPage?Member={JsonConvert.SerializeObject(member)}");
        }

        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;
            ShowMessage = false;

            try
            {

                HouseHoldList = await _transactionServices.GetHouseHoldMembers(DeviceInfo.Name.RemoveSpecialCharacters());
                if (HouseHoldList.Count() == 0)
                {
                    ShowMessage = true;
                }
            }
            catch (Exception ex)
            {
                App._alertSvc.ShowConfirmation("Error", $"{ex.Message}", (result =>
                {
                    App._alertSvc.ShowAlert("Result", $"{result}");
                }));
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async void OnAppearing()
        {
            IsBusy = true;
            ShowMessage = false;

            try
            {
                HouseHoldList = await _transactionServices.GetHouseHoldMembers(DeviceInfo.Name.RemoveSpecialCharacters());
                if (HouseHoldList.Count() == 0)
                {
                    ShowMessage = true;
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

        async void ExecuteEditOperationCommand(object sender)
        {
            try
            {
                var houseHold = (HouseHoldMember)sender;
                HouseHoldMember sentHouseHold = new HouseHoldMember
                {
                    PartitionKey = houseHold.PartitionKey,
                    RowKey = houseHold.RowKey,
                    Name = houseHold.Name,
                    Email = houseHold.Email,
                    PhoneNumber = houseHold.PhoneNumber.RemoveSpecialCharacters()
                };
                await Shell.Current.GoToAsync($"HouseHoldMemberPage?Member={JsonConvert.SerializeObject(sentHouseHold)}");
            }
            catch (Exception ex)
            {
                App._alertSvc.ShowConfirmation("Error", $"{ex.Message}", (result => { App._alertSvc.ShowAlert("Result", $"{result}"); }));
            }
        }

        private void OnDelete(object obj)
        {

            App._alertSvc.ShowConfirmation("Error", "Are you sure you want to delete?", (async result =>
            {
                if (result)
                {
                    try
                    {
                        HouseHoldMember member = (HouseHoldMember)obj;
                        member.DeviceName = DeviceInfo.Name.RemoveSpecialCharacters();
                        string uri = "https://projecthalcyonmanagmenttransactions.azurewebsites.net/api/DeleteHouseHold?code=sxW9Y3fQvvs/9Q5aLPJSbq5euGFMcOboYlcymUY/v317KCRgZWTu9w==";
                        await _transactionServices.AzureFunctionPostTransaction(uri, JsonConvert.SerializeObject(member));
                        HouseHoldList = await _transactionServices.GetHouseHoldMembers(DeviceInfo.Name.RemoveSpecialCharacters());
                    }
                    catch (Exception ex)
                    {
                        App._alertSvc.ShowConfirmation("Error", $"{ex.Message}", (result => { App._alertSvc.ShowAlert("Result", $"{result}"); }));
                    }
                }

            }));
        }


        private List<HouseHoldMember> _houseHoldList;
        public List<HouseHoldMember> HouseHoldList
        {
            get => _houseHoldList;
            set => SetProperty(ref _houseHoldList, value);
        }

        private HouseHoldMember _selectedHouseHoldMember;
        public HouseHoldMember SelectedHouseHoldMember
        {
            get => _selectedHouseHoldMember;
            set => SetProperty(ref _selectedHouseHoldMember, value);
        }



        private bool _showMessage;
        public bool ShowMessage
        {
            get => _showMessage;
            set => SetProperty(ref _showMessage, value);
        }


    }
}
