using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Plugin.TabbedPage.Maui;
using Superdev.Maui;
using Superdev.Maui.Localization;
using TabbedPageDemoApp.Resources.Text;
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
                .UseSuperdevMaui()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("IBMPlexSans-Regular.ttf", "IBMPlexSans");
                    fonts.AddFont("IBMPlexSans-Bold.ttf", "IBMPlexSansBold");
                    fonts.AddFont("IBMPlexMono-Regular.ttf", "IBMPlexMonoRegular");
                });

            builder.Services.AddLogging(b =>
            {
                b.ClearProviders();
                b.SetMinimumLevel(LogLevel.Trace);
                b.AddDebug();
                b.AddSentry(SentryConfiguration.Configure);
            });

            var translationProvider = ResxSingleTranslationProvider.Current;
            translationProvider.Init(Strings.ResourceManager);

            TranslateExtension.Init(Localizer.Current, translationProvider);

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
            builder.Services.AddTransient<CarTireAlertViewModel>();

            builder.Services.AddTransient<ChatPage>();
            builder.Services.AddTransient<ChatViewModel>();

            builder.Services.AddTransient<AccountPage>();
            builder.Services.AddTransient<AccountViewModel>();

            return builder.Build();
        }
    }
}
