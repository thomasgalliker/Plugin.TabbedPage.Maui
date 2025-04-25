using System.ComponentModel;
using Plugin.TabbedPage.Maui.Controls;
using Page = Microsoft.Maui.Controls.Page;

namespace Plugin.TabbedPage.Maui.Utils
{
    using TabbedPage = Microsoft.Maui.Controls.TabbedPage;

    internal static class PageHelper
    {
        internal static Page GetTarget(Page target)
        {
            return target switch
            {
                FlyoutPage flyout => GetTarget(flyout.Detail),
                Microsoft.Maui.Controls.TabbedPage tabbed => GetTarget(tabbed.CurrentPage),
                NavigationPage navigation => GetTarget(navigation.CurrentPage) ?? navigation,
                ContentPage page => page,
                null => null,
                _ => throw new NotSupportedException($"The page type '{target.GetType().FullName}' is not supported.")
            };
        }

        internal static string PrintNavigationPath()
        {
            var mainPage = Application.Current.MainPage;
            var navigation = mainPage.Navigation;
            var pages = GetNavigationTree(navigation, mainPage).ToArray();
            var navigationPath = PrintNavigationPath(pages);
            return navigationPath;
        }

        private static string PrintNavigationPath(IEnumerable<Page> pages)
        {
            return pages.Aggregate("", (current, page) => $"{current}/{(page?.GetType().Name ?? "")}");
        }

        internal static IEnumerable<Page> GetNavigationTree(INavigation navigation, Page page, bool modal = false)
        {
            switch (page)
            {
                case FlyoutPage flyoutPage:
                    yield return flyoutPage;
                    foreach (var p in GetNavigationTree(flyoutPage.Flyout.Navigation, flyoutPage.Flyout))
                    {
                        yield return p;
                    }

                    foreach (var p in GetNavigationTree(flyoutPage.Detail.Navigation, flyoutPage.Detail))
                    {
                        yield return p;
                    }

                    break;

                case Microsoft.Maui.Controls.TabbedPage tabbedPage:
                    yield return tabbedPage;
                    foreach (var p in GetNavigationTree(tabbedPage.CurrentPage.Navigation, tabbedPage.CurrentPage))
                    {
                        yield return p;
                    }

                    break;

                case NavigationPage navigationPage:
                    yield return navigationPage;

                    foreach (var childPage in navigationPage.InternalChildren.OfType<Page>())
                    {
                        yield return childPage;
                    }
                    break;

                case ContentPage contentPage:
                    yield return contentPage;

                    break;
            }

            if (modal == false)
            {
                foreach (var modalPage in navigation.ModalStack)
                {
                    foreach (var p in GetNavigationTree(modalPage.Navigation, modalPage, modal: true))
                    {
                        yield return p;
                    }
                }
            }
        }


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