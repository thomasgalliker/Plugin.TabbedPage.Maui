using Plugin.TabbedPage.Maui;
using TabbedPageDemoApp.Services;
using TabbedPageDemoApp.ViewModels;

namespace TabbedPageDemoApp.Views
{
    public partial class TabbedMainPage : TabbedPage
    {
        private readonly IDialogService dialogService;

        public TabbedMainPage(
            TabbedMainViewModel tabbedMainViewModel,
            IDialogService dialogService)
        {
            this.InitializeComponent();
            this.BindingContext = tabbedMainViewModel;

            this.dialogService = dialogService;
        }

        private void OnItemReselected(object sender, ItemReselectedEventArgs e)
        {
            _ = this.dialogService.DisplayAlertAsync(
                "ItemReselected event",
                e.Page.GetType().Name,
                "OK");
        }
    }
}
