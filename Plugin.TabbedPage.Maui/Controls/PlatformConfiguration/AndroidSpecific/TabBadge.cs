namespace Plugin.TabbedPage.Maui.Controls.PlatformConfiguration.AndroidSpecific
{
    /// <summary>
    /// Android-specific tab badge configuration
    /// </summary>
    public static class TabBadge
    {
        public static readonly BindableProperty BadgeColorProperty = BindableProperty.CreateAttached(
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

        public static readonly BindableProperty BadgeTextColorProperty = BindableProperty.CreateAttached(
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
    }
}