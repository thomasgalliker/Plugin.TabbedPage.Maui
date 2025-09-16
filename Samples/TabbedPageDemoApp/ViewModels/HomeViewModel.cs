using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TabbedPageDemoApp.Services;

namespace TabbedPageDemoApp.ViewModels
{
    public class HomeViewModel : ObservableObject
    {
        private readonly INavigationService navigationService;
        private readonly IBrowser browser;

        private IRelayCommand appearingCommand;
        private IAsyncRelayCommand<string> navigateToPageCommand;
        private IAsyncRelayCommand<string> openUrlCommand;
        private int? counter;

        public HomeViewModel(
            INavigationService navigationService,
            IBrowser browser)
        {
            this.navigationService = navigationService;
            this.browser = browser;
        }

        public IRelayCommand AppearingCommand
        {
            get => this.appearingCommand ??= new RelayCommand(this.OnAppearing);
        }

        private void OnAppearing()
        {

        }

        public int? Counter
        {
            get => this.counter <= 0 ? null : this.counter;
            set => this.SetProperty(ref this.counter, value);
        }

        public IAsyncRelayCommand<string> NavigateToPageCommand
        {
            get => this.navigateToPageCommand ??= new AsyncRelayCommand<string>(this.NavigateToPageAsync);
        }

        private async Task NavigateToPageAsync(string page)
        {
            await this.navigationService.PushAsync(page);
        }

        public IAsyncRelayCommand<string> OpenUrlCommand
        {
            get => this.openUrlCommand ??= new AsyncRelayCommand<string>(this.OpenUrlAsync);
        }

        private async Task OpenUrlAsync(string url)
        {
            try
            {
                var options = GetDefaultBrowserLaunchOptions();
                await this.browser.OpenAsync(new Uri(url), options);
            }
            catch
            {
                // Ignore exceptions
            }
        }

        private static BrowserLaunchOptions GetDefaultBrowserLaunchOptions()
        {
            var options = new BrowserLaunchOptions
            {
                LaunchMode = BrowserLaunchMode.SystemPreferred,
                TitleMode = BrowserTitleMode.Show,
                PreferredToolbarColor = Application.Current.Resources["Primary"] as Color,
                PreferredControlColor = Colors.White,
                Flags = BrowserLaunchFlags.None
            };
            return options;
        }
    }
}
