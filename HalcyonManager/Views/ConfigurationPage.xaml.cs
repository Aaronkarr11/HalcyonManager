namespace HalcyonManager.Views
{
    public partial class ConfigurationPage : ContentPage
    {
        public ConfigurationPage()
        {
            InitializeComponent();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            Shell.Current.GoToAsync($"HouseHoldManagmentPage");
        }

        private void HelpButton_Clicked(object sender, EventArgs e)
        {
            Shell.Current.GoToAsync($"HelpPage");
        }
    }
}