using HalcyonManager.ViewModels;
using HalcyonCore.Interfaces;

namespace HalcyonManager.Views
{
    public partial class HouseHoldMemberPage : ContentPage
    {
        public HouseHoldMemberPage()
        {
            InitializeComponent();
            var service = DependencyService.Get<IHalcyonManagementClient>();
            BindingContext = new HouseHoldMemberViewModel(service);
        }
    }
}