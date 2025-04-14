using Font = Microsoft.Maui.Font;

namespace Plugin.TabbedPage.Maui.Controls
{
    public static class TabbedPage
    {
        public static readonly BindableProperty FontFamilyProperty = BindableProperty.CreateAttached(
            "FontFamily",
            typeof(string),
            typeof(TabbedPage),
            null);

        public static string GetFontFamily(BindableObject view)
        {
            return (string)view.GetValue(FontFamilyProperty);
        }

        public static void SetFontFamily(BindableObject view, string value)
        {
            view.SetValue(FontFamilyProperty, value);
        }

        public static readonly BindableProperty FontSizeProperty = BindableProperty.CreateAttached(
            "FontSize",
            typeof(double),
            typeof(TabbedPage),
            Font.Default.Size);

        public static double GetFontSize(BindableObject view)
        {
            return (double)view.GetValue(FontSizeProperty);
        }

        public static void SetFontSize(BindableObject view, double value)
        {
            view.SetValue(FontSizeProperty, value);
        }

        public static readonly BindableProperty FontAttributesProperty = BindableProperty.CreateAttached(
            "FontAttributes",
            typeof(FontAttributes),
            typeof(TabbedPage),
            FontAttributes.None);

        public static FontAttributes GetFontAttributes(BindableObject view)
        {
            return (FontAttributes)view.GetValue(FontAttributesProperty);
        }

        public static void SetFontAttributes(BindableObject view, FontAttributes value)
        {
            view.SetValue(FontAttributesProperty, value);
        }
    }
}