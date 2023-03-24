﻿using Halcyon.Clients;
using HalcyonManager.ViewModels;

namespace HalcyonManager.Views
{
    public partial class NewItemPage : ContentPage
    {
        public NewItemPage()
        {
            InitializeComponent();
            var service = DependencyService.Get<IHalcyonManagementClient>();
            BindingContext = new NewItemViewModel(service);
        }
    }
}