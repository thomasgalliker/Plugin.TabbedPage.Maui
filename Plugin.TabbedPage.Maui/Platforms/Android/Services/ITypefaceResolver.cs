using Android.Graphics;

namespace Plugin.TabbedPage.Maui
{
    public interface ITypefaceResolver
    {
        Typeface GetTypeface(string fontFamily, double fontSize, FontAttributes fontAttrbutes);
    }
}