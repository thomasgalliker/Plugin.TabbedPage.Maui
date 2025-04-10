using TabbedPageDemoApp.Views;

namespace TabbedPageDemoApp
{
    public partial class App : Application
    {
        public App(IServiceProvider serviceProvider)
        {
            this.InitializeComponent();

            var tabbedMainPage = serviceProvider.GetRequiredService<TabbedMainPage>();
            this.MainPage = tabbedMainPage;
        }
    }
}
