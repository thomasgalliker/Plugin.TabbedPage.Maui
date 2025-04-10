using TabbedPageDemoApp.ViewModels;

namespace TabbedPageDemoApp.Views
{
    public partial class HomePage : ContentPage
    {
        public HomePage()
        {
            this.InitializeComponent();
            this.BindingContext = IPlatformApplication.Current.Services.GetService<HomeViewModel>();
        }
    }
}