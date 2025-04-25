namespace Plugin.TabbedPage.Maui.Controls.PlatformConfiguration.iOSSpecific
{
    /// <summary>
    /// iOS-specific tabbed page configuration
    /// </summary>
    public static class TabbedPage
    {
        public static readonly BindableProperty NormalBadgeBackgroundColorProperty = BindableProperty.CreateAttached(
            "NormalBadgeBackgroundColor",
            typeof(Color),
            typeof(TabbedPage),
            null);

        public static Color GetNormalBadgeBackgroundColor(BindableObject view)
        {
            return (Color)view.GetValue(NormalBadgeBackgroundColorProperty);
        }

        public static void SetNormalBadgeBackgroundColor(BindableObject view, Color value)
        {
            view.SetValue(NormalBadgeBackgroundColorProperty, value);
        }

        public static readonly BindableProperty SelectedBadgeBackgroundColorProperty = BindableProperty.CreateAttached(
            "SelectedBadgeBackgroundColor",
            typeof(Color),
            typeof(TabbedPage),
            null);

        public static Color GetSelectedBadgeBackgroundColor(BindableObject view)
        {
            return (Color)view.GetValue(SelectedBadgeBackgroundColorProperty);
        }

        public static void SetSelectedBadgeBackgroundColor(BindableObject view, Color value)
        {
            view.SetValue(SelectedBadgeBackgroundColorProperty, value);
        }
    }
}