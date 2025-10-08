using System.ComponentModel;
using Plugin.TabbedPage.Maui.Controls;
using Page = Microsoft.Maui.Controls.Page;

namespace Plugin.TabbedPage.Maui.Utils
{
    using TabbedPage = Microsoft.Maui.Controls.TabbedPage;

    internal static class PageHelper
    {
        /// <summary>
        /// Internal use only. Attempts to get the badged child of a tabbed page (either navigation page or content page)
        /// </summary>
        /// <param name="parentTabbedPage">Tabbed page</param>
        /// <param name="tabIndex">Index</param>
        /// <returns>Page</returns>
        internal static Page GetChildPageWithBadge(TabbedPage parentTabbedPage, int tabIndex)
        {
            var page = parentTabbedPage.Children[tabIndex];
            return GetPageWithBadge(page);
        }

        internal static Page GetPageWithBadge(Page element)
        {
            if (TabBadge.GetBadgeText(element) != (string)TabBadge.BadgeTextProperty.DefaultValue)
            {
                return element;
            }

            if (element is NavigationPage navigationPage)
            {
                return navigationPage.RootPage;
            }

            return element;
        }

        internal static IEnumerable<Page> GetChildPages(TabbedPage tabbedPage)
        {
            foreach (var child in tabbedPage.Children)
            {
                if (child is NavigationPage navigationPage)
                {
                    yield return navigationPage.RootPage;
                }
                else
                {
                    yield return child;
                }
            }
        }
    }
}