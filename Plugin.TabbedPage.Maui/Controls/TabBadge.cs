using Font = Microsoft.Maui.Font;

namespace Plugin.TabbedPage.Maui.Controls
{
    public static class TabBadge
    {
        public static readonly BindableProperty BadgeTextProperty = BindableProperty.CreateAttached(
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

        public static readonly BindableProperty BadgeFontProperty = BindableProperty.CreateAttached(
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

        public static readonly BindableProperty BadgePositionProperty = BindableProperty.CreateAttached(
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

        public static readonly BindableProperty BadgeMarginProperty = BindableProperty.CreateAttached(
            "BadgeMargin",
            typeof(Thickness),
            typeof(TabBadge),
            GetDefaultBadgeMargin());

        public static Thickness GetBadgeMargin(BindableObject view)
        {
            return (Thickness)view.GetValue(BadgeMarginProperty);
        }

        public static void SetBadgeMargin(BindableObject view, Thickness value)
        {
            view.SetValue(BadgeMarginProperty, value);
        }

        public static Thickness GetDefaultBadgeMargin()
        {
            var currentPlatform = DeviceInfo.Current.Platform;
            if (currentPlatform == DevicePlatform.Android)
            {
                return new Thickness(0, 2, 2, 0);
            }

            return new Thickness(0);
        }
    }
}