using Android.Views;
using Google.Android.Material.BottomNavigation;

namespace Plugin.TabbedPage.Maui.Extensions
{
    internal static class BottomNavigationViewExtensions
    {
        internal static IEnumerable<IMenuItem> GetAllItems(this BottomNavigationView bottomNavigationView)
        {
            var menu = bottomNavigationView.Menu;
            for (var i = 0; i < bottomNavigationView.Menu.Size(); i++)
            {
                var menuItem = menu.GetItem(i);
                yield return menuItem;
            }
        }

        internal static int? GetTabIndex(this BottomNavigationView bottomNavigationView, IMenuItem menuItem)
        {
            var menu = bottomNavigationView.Menu;
            for (var i = 0; i < bottomNavigationView.Menu.Size(); i++)
            {
                if (menu.GetItem(i) == menuItem)
                {
                    return i;
                }
            }

            return null;
        }
    }
}