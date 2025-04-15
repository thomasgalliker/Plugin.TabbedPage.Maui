using System.ComponentModel;
using Android.Views;
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Platform;
using Plugin.TabbedPage.Maui.Controls;
using Plugin.TabbedPage.Maui.Extensions;
using Font = Microsoft.Maui.Font;
using Page = Microsoft.Maui.Controls.Page;
using View = Android.Views.View;

namespace Plugin.TabbedPage.Maui.Platform.Handlers
{
    using TabBadgeExtensionsAndroid = Controls.PlatformConfiguration.AndroidSpecific.TabBadge;

    internal static class BadgeViewExtensions
    {
        public static void UpdateFromElement(this BadgeView badgeView, Page page)
        {
            //get text
            var badgeText = TabBadge.GetBadgeText(page);
            badgeView.Text = badgeText;

            // set color if not default
            var tabColor = TabBadgeExtensionsAndroid.GetBadgeColor(page);
            if (tabColor.IsNotDefault())
            {
                badgeView.BadgeColor = tabColor.ToPlatform();
            }

            // set text color if not default
            var tabTextColor = TabBadgeExtensionsAndroid.GetBadgeTextColor(page);
            if (tabTextColor.IsNotDefault())
            {
                badgeView.TextColor = tabTextColor.ToPlatform();
            }

            // set font if not default
            var font = TabBadge.GetBadgeFont(page);
            if (font != Font.Default)
            {
                var fontManager = page.Handler.GetRequiredService<IFontManager>();
                badgeView.Typeface = fontManager.GetTypeface(font);
            }

            var margin = TabBadge.GetBadgeMargin(page);
            badgeView.SetMargins((float)margin.Left, (float)margin.Top, (float)margin.Right, (float)margin.Bottom);

            // set position
            badgeView.Postion = TabBadge.GetBadgePosition(page);
        }

        public static T FindChildOfType<T>(this ViewGroup parent) where T : View
        {
            if (parent == null)
            {
                return null;
            }

            if (parent.ChildCount == 0)
            {
                return null;
            }

            for (var i = 0; i < parent.ChildCount; i++)
            {
                var child = parent.GetChildAt(i);


                if (child is T typedChild)
                {
                    return typedChild;
                }

                if (!(child is ViewGroup))
                {
                    continue;
                }


                var result = FindChildOfType<T>(child as ViewGroup);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }
    }
}