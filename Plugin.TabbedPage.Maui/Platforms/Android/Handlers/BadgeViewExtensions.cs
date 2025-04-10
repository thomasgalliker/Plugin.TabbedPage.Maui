using System.ComponentModel;
using Android.Views;
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Platform;
using Plugin.TabbedPage.Maui.Controls;
using Font = Microsoft.Maui.Font;
using Page = Microsoft.Maui.Controls.Page;
using View = Android.Views.View;

namespace Plugin.TabbedPage.Maui.Platform.Handlers
{
    internal static class BadgeViewExtensions
    {
        public static void UpdateFromElement(this BadgeView badgeView, Page element)
        {
            //get text
            var badgeText = TabBadge.GetBadgeText(element);
            badgeView.Text = badgeText;

            // set color if not default
            var tabColor = TabBadge.GetBadgeColor(element);
            if (tabColor.IsNotDefault())
            {
                badgeView.BadgeColor = tabColor.ToPlatform();
            }

            // set text color if not default
            var tabTextColor = TabBadge.GetBadgeTextColor(element);
            if (tabTextColor.IsNotDefault())
            {
                badgeView.TextColor = tabTextColor.ToPlatform();
            }

            // set font if not default
            var font = TabBadge.GetBadgeFont(element);
            if (font != Font.Default)
            {
                badgeView.Typeface = font.ToTypeface((element.Handler ?? Application.Current.Handler).MauiContext.Services
                    .GetRequiredService<IFontManager>());
            }

            var margin = TabBadge.GetBadgeMargin(element);
            badgeView.SetMargins((float)margin.Left, (float)margin.Top, (float)margin.Right, (float)margin.Bottom);

            // set position
            badgeView.Postion = TabBadge.GetBadgePosition(element);
        }

        public static void UpdateFromPropertyChangedEvent(this BadgeView badgeView, Element element, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == TabBadge.BadgeTextProperty.PropertyName)
            {
                badgeView.Text = TabBadge.GetBadgeText(element);
            }
            else if (e.PropertyName == TabBadge.BadgeColorProperty.PropertyName)
            {
                badgeView.BadgeColor = TabBadge.GetBadgeColor(element).ToAndroid();
            }
            else if (e.PropertyName == TabBadge.BadgeTextColorProperty.PropertyName)
            {
                badgeView.TextColor = TabBadge.GetBadgeTextColor(element).ToAndroid();
            }
            else if (e.PropertyName == TabBadge.BadgeFontProperty.PropertyName)
            {
                var fontManager = (element.Handler ?? Application.Current.Handler).MauiContext.Services.GetRequiredService<IFontManager>();
                badgeView.Typeface = TabBadge.GetBadgeFont(element).ToTypeface(fontManager);
            }
            else if (e.PropertyName == TabBadge.BadgePositionProperty.PropertyName)
            {
                badgeView.Postion = TabBadge.GetBadgePosition(element);
            }
            else if (e.PropertyName == TabBadge.BadgeMarginProperty.PropertyName)
            {
                var margin = TabBadge.GetBadgeMargin(element);
                badgeView.SetMargins((float)margin.Left, (float)margin.Top, (float)margin.Right, (float)margin.Bottom);
            }
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