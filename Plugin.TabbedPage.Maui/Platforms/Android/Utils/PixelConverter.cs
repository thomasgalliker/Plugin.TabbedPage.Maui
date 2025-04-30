using Android.Content;
using Android.Util;

namespace Plugin.TabbedPage.Maui.Utils
{
    internal static class PixelConverter
    {
        internal static float DpToPixels(Context context, float valueInDp)
        {
            if (context is null)
            {
                return valueInDp;
            }

            var displayMetrics = context.Resources?.DisplayMetrics!;
            return TypedValue.ApplyDimension(ComplexUnitType.Dip, valueInDp, displayMetrics);
        }

        internal static int DipToPixels(Context context, float dip)
        {
            if (context is null)
            {
                return (int)dip;
            }

            var displayMetrics = context.Resources.DisplayMetrics;
            return (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, dip, displayMetrics);
        }
    }
}