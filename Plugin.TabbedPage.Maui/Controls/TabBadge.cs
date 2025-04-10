using Font = Microsoft.Maui.Font;
using Page = Microsoft.Maui.Controls.Page;

namespace Plugin.TabbedPage.Maui.Controls
{
    using TabbedPage = Microsoft.Maui.Controls.TabbedPage;

    public static class TabBadge
    {
        public static BindableProperty BadgeTextProperty = BindableProperty.CreateAttached(
            "BadgeText",
            typeof(string),
            typeof(TabBadge),
            null);

        public static string GetBadgeText(BindableObject view)
        {
            return (string)view.GetValue(BadgeTextProperty);
        }

        public static void SetBadgeText(BindableObject view, string value)
        {
            view.SetValue(BadgeTextProperty, value);
        }

        public static BindableProperty BadgeColorProperty = BindableProperty.CreateAttached(
            "BadgeColor",
            typeof(Color),
            typeof(TabBadge),
            null);

        public static Color GetBadgeColor(BindableObject view)
        {
            return (Color)view.GetValue(BadgeColorProperty);
        }

        public static void SetBadgeColor(BindableObject view, Color value)
        {
            view.SetValue(BadgeColorProperty, value);
        }

        public static BindableProperty BadgeTextColorProperty = BindableProperty.CreateAttached(
            "BadgeTextColor",
            typeof(Color),
            typeof(TabBadge),
            null);

        public static Color GetBadgeTextColor(BindableObject view)
        {
            return (Color)view.GetValue(BadgeTextColorProperty);
        }

        public static void SetBadgeTextColor(BindableObject view, Color value)
        {
            view.SetValue(BadgeTextColorProperty, value);
        }

        public static BindableProperty BadgeFontProperty = BindableProperty.CreateAttached(
            "BadgeFont",
            typeof(Font),
            typeof(TabBadge),
            Font.Default);

        public static Font GetBadgeFont(BindableObject view)
        {
            return (Font)view.GetValue(BadgeFontProperty);
        }

        public static void SetBadgeFont(BindableObject view, Font value)
        {
            view.SetValue(BadgeFontProperty, value);
        }

        public static BindableProperty BadgePositionProperty = BindableProperty.CreateAttached(
            "BadgePosition",
            typeof(BadgePosition),
            typeof(TabBadge),
            BadgePosition.TopRight);

        public static BadgePosition GetBadgePosition(BindableObject view)
        {
            return (BadgePosition)view.GetValue(BadgePositionProperty);
        }

        public static void SetBadgePosition(BindableObject view, BadgePosition value)
        {
            view.SetValue(BadgePositionProperty, value);
        }

        public static BindableProperty BadgeMarginProperty = BindableProperty.CreateAttached(
            "BadgeMargin",
            typeof(Thickness),
            typeof(TabBadge),
            DefaultMargins);

        public static Thickness GetBadgeMargin(BindableObject view)
        {
            return (Thickness)view.GetValue(BadgeMarginProperty);
        }

        public static void SetBadgeMargin(BindableObject view, Thickness value)
        {
            view.SetValue(BadgeMarginProperty, value);
        }

        public static Thickness DefaultMargins
        {
            get
            {
                var currentPlatform = DeviceInfo.Current.Platform;
                if (currentPlatform == DevicePlatform.Android)
                {
                    return new Thickness(0, 2, 2, 0);
                }

                return new Thickness(0);
            }
        }

        /// <summary>
        /// Internal use only. Attempts to get the badged child of a tabbed page (either navigation page or content page)
        /// </summary>
        /// <param name="parentTabbedPage">Tabbed page</param>
        /// <param name="tabIndex">Index</param>
        /// <returns>Page</returns>
        internal static Page GetChildPageWithBadge(this TabbedPage parentTabbedPage, int tabIndex)
        {
            var element = parentTabbedPage.Children[tabIndex];
            return element.GetPageWithBadge();
        }

        internal static Page GetPageWithBadge(this Page element)
        {
            if (GetBadgeText(element) != (string)BadgeTextProperty.DefaultValue)
            {
                return element;
            }

            if (element is NavigationPage navigationPage)
            {
                return navigationPage.RootPage;
            }

            return element;
        }
    }
}