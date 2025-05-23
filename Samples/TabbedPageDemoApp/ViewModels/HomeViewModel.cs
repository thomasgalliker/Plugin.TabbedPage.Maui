﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TabbedPageDemoApp.Services;

namespace TabbedPageDemoApp.ViewModels
{
    public class HomeViewModel : ObservableObject
    {
        private readonly INavigationService navigationService;
        private readonly ILauncher launcher;

        private IRelayCommand appearingCommand;
        private IAsyncRelayCommand<string> navigateToPageCommand;
        private IAsyncRelayCommand<string> openUrlCommand;
        private int? counter;

        public HomeViewModel(
            INavigationService navigationService,
            ILauncher launcher)
        {
            this.navigationService = navigationService;
            this.launcher = launcher;
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
                await this.launcher.TryOpenAsync(url);
            }
            catch
            {
                // Ignore exceptions
            }
        }
    }
}
