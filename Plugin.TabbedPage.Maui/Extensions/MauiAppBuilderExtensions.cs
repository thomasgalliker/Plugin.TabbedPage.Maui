#if (ANDROID || IOS)
using Plugin.TabbedPage.Maui.Platform;
#endif

namespace Plugin.TabbedPage.Maui
{
    public static class MauiAppBuilderExtensions
    {
        public static MauiAppBuilder UseTabbedPage(this MauiAppBuilder builder)
        {
#if (ANDROID || IOS)
            builder.ConfigureMauiHandlers(handlers =>
            {
                handlers.AddHandler(typeof(Microsoft.Maui.Controls.TabbedPage), typeof(BadgedTabbedPageHandler));
            });
#endif

#if ANDROID
            builder.Services.AddSingleton<ITypefaceResolver, TypefaceResolver>();
#endif

            return builder;
        }
    }
}