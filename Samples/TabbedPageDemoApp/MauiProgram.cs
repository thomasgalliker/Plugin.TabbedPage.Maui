using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Plugin.TabbedPage.Maui;
using TabbedPageDemoApp.Services;
using TabbedPageDemoApp.Services.Logging;
using TabbedPageDemoApp.ViewModels;
using TabbedPageDemoApp.Views;

namespace TabbedPageDemoApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseTabbedPage()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });


            builder.Services.AddLogging(b =>
            {
                b.ClearProviders();
                b.SetMinimumLevel(LogLevel.Trace);
                b.AddDebug();
                b.AddSentry(SentryConfiguration.Configure);
            });

            // Register services
            builder.Services.AddSingleton<INavigationService, MauiNavigationService>();
            builder.Services.AddSingleton<IDialogService, DialogService>();
            builder.Services.AddSingleton<ILauncher>(_ => Launcher.Default);

            // Register pages and view models
            builder.Services.AddTransient<TabbedMainPage>();
            builder.Services.AddTransient<TabbedMainViewModel>();

            builder.Services.AddTransient<HomePage>();
            builder.Services.AddTransient<HomeViewModel>();

            builder.Services.AddTransient<CarTireAlertPage>();
            builder.Services.AddTransient<AccountPage>();
            builder.Services.AddTransient<ChatPage>();

            return builder.Build();
        }
    }
}
