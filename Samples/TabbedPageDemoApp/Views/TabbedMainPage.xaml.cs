using TabbedPageDemoApp.ViewModels;

namespace TabbedPageDemoApp.Views
{
    public partial class TabbedMainPage : TabbedPage
    {
        public TabbedMainPage(TabbedMainViewModel tabbedMainViewModel)
        {
            this.InitializeComponent();
            this.BindingContext = tabbedMainViewModel;
        }
    }
}
