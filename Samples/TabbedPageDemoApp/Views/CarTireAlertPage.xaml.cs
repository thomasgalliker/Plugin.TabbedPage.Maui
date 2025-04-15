using TabbedPageDemoApp.ViewModels;

namespace TabbedPageDemoApp.Views
{
    public partial class CarTireAlertPage : ContentPage
    {
        public CarTireAlertPage()
        {
            this.InitializeComponent();
            this.BindingContext = IPlatformApplication.Current.Services.GetService<CarTireAlertViewModel>();
        }
    }
}