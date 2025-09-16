using Microsoft.Extensions.Logging;
using Sentry.Extensions.Logging;

namespace TabbedPageDemoApp.Services.Logging
{
    public static class SentryConfiguration
    {
        public static void Configure(SentryLoggingOptions options)
        {
            options.InitializeSdk = true;
#if DEBUG
            options.Debug = true;
#endif
            options.Dsn = "https://87269f9f504ed34336b9a7beff165764@o4507458300280832.ingest.de.sentry.io/4510028090376272";
            options.MinimumEventLevel = LogLevel.Warning;
            options.MinimumBreadcrumbLevel = LogLevel.Debug;
        }
    }
}