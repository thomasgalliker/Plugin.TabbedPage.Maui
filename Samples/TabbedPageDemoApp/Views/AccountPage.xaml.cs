using TabbedPageDemoApp.ViewModels;

namespace TabbedPageDemoApp.Views
{
    public partial class AccountPage : ContentPage
    {
        public AccountPage()
        {
            this.InitializeComponent();
            this.BindingContext = IPlatformApplication.Current.Services.GetService<AccountViewModel>();
        }
    }
}