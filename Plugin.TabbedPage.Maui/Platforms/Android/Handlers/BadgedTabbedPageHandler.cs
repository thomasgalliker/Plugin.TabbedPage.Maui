using Android.Views;
using Android.Widget;
using Google.Android.Material.BottomNavigation;
using Google.Android.Material.Tabs;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific;
using Microsoft.Maui.Handlers;
using Page = Microsoft.Maui.Controls.Page;
using AView = global::Android.Views.View;
using View = Android.Views.View;
using Plugin.TabbedPage.Maui.Utils;
using Plugin.TabbedPage.Maui.Controls;
using System.Diagnostics;
using Plugin.TabbedPage.Maui.Platform.Handlers;

namespace Plugin.TabbedPage.Maui.Platform
{
    using TabbedPage = Microsoft.Maui.Controls.TabbedPage;

    public class BadgedTabbedPageHandler : TabbedViewHandler
    {
        private static readonly TimeSpan DelayBeforeTabAdded = TimeSpan.FromMilliseconds(50);

        private readonly Dictionary<Element, BadgeView> badgeViews = new Dictionary<Element, BadgeView>();

        private TabLayout topTabLayout;
        private LinearLayout topTabStrip;
        private ViewGroup bottomTabStrip;
        private BottomNavigationView bottomNavigationView;

        protected TabbedPage TabbedPage => this.VirtualView as TabbedPage;
        protected ViewGroup ViewGroup => this.PlatformView as ViewGroup;

        protected override void ConnectHandler(Android.Views.View platformView)
        {
            base.ConnectHandler(platformView);

            if (this.VirtualView is TabbedPage customTabbedPage)
            {
                //this.VirtualView.AddCleanUpEvent(); // Not needed because, pages call DisconnectHandler automagically
                customTabbedPage.Loaded += this.TabbedPage_Loaded;
            }
        }

        private void TabbedPage_Loaded(object sender, EventArgs e)
        {
            this.Cleanup(this.TabbedPage);

            this.InitLayout();

            var tabCount = this.GetTabsCount();
            for (var i = 0; i < tabCount; i++)
            {
                this.AddTabBadge(i);
            }

            this.TabbedPage.ChildAdded += this.OnTabAdded;
            this.TabbedPage.ChildRemoved += this.OnTabRemoved;
        }

        private bool IsBottomTabPlacement
        {
            get => this.TabbedPage?.OnThisPlatform()?.GetToolbarPlacement() == ToolbarPlacement.Bottom;
        }

        private void InitLayout()
        {
            var tabbedPageManager = ReflectionHelper.GetFieldValue(this.TabbedPage, "_tabbedPageManager");

            if (this.IsBottomTabPlacement)
            {
                this.bottomNavigationView = ReflectionHelper.GetFieldValue<BottomNavigationView>(tabbedPageManager, "_bottomNavigationView");
                this.bottomTabStrip = this.bottomNavigationView?.GetChildAt(0) as ViewGroup;
                if (this.bottomTabStrip == null)
                {
                    Trace.WriteLine("Plugin.TabbedPage.Maui: No bottom tab layout found. Badge not added.");
                }
            }
            else
            {
                this.topTabLayout = ReflectionHelper.GetFieldValue<TabLayout>(tabbedPageManager, "_tabLayout");
                if (this.topTabLayout == null)
                {
                    Trace.WriteLine("Plugin.TabbedPage.Maui: No TabLayout found. Badge not added.");
                }

                this.topTabStrip = this.topTabLayout.FindChildOfType<LinearLayout>();
            }
        }

        private int GetTabsCount()
        {
            if (this.IsBottomTabPlacement)
            {
                return this.bottomTabStrip?.ChildCount ?? 0;
            }

            return this.topTabLayout?.TabCount ?? 0;
        }

        private void AddTabBadge(int tabIndex)
        {
            if (tabIndex == -1)
            {
                return;
            }

            var page = PageHelper.GetChildPageWithBadge(this.TabbedPage, tabIndex);

            AView targetView;
            var isBottomTabPlacement = this.IsBottomTabPlacement;
            if (isBottomTabPlacement)
            {
                targetView = this.bottomTabStrip?.GetChildAt(tabIndex);
            }
            else
            {
                targetView = this.topTabLayout?.GetTabAt(tabIndex).CustomView;
                if (targetView == null)
                {
                    targetView = this.topTabStrip?.GetChildAt(tabIndex);
                }
            }


            if (targetView is not BottomNavigationItemView targetLayout)
            {
                Trace.WriteLine("Plugin.TabbedPage.Maui: Badge target cannot be null. Badge not added.");
                return;
            }

            targetLayout.SetClipChildren(false);
            targetLayout.SetClipToPadding(false);

            // var b = this.bottomNavigationView.GetOrCreateBadge(targetLayout.Id);
            // b.BadgeTextColor
            var badgeView = targetLayout.FindChildOfType<BadgeView>();

            // var badgeView = targetLayout.Badge;

            if (badgeView == null)
            {
                var imageView = targetLayout.FindChildOfType<ImageView>();

                if (isBottomTabPlacement)
                {
                    // Create for entire tab layout
                    badgeView = BadgeView.ForTargetLayout(this.Context, imageView);
                }
                else
                {
                    // Create badge for tab image or text
                    View target = imageView?.Drawable != null ?
                        imageView :
                        targetLayout.FindChildOfType<TextView>();

                    badgeView = BadgeView.ForTarget(this.Context, target);
                }
            }

            this.badgeViews[page] = badgeView;
            badgeView.UpdateFromElement(page);

            page.PropertyChanged -= this.OnTabbedPagePropertyChanged;
            page.PropertyChanged += this.OnTabbedPagePropertyChanged;
        }


        private static IEnumerable<Page> GetChildPages(TabbedPage tabbedPage)
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

        private static int GetTabIndex(TabbedPage tabbedPage, Page page)
        {
            var tabPages = GetChildPages(tabbedPage).ToList();
            var tabIndex = tabPages.IndexOf(page);
            return tabIndex;
        }

        protected virtual void OnTabbedPagePropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (sender is not Element element)
            {
                return;
            }

            if (e.PropertyName == Page.TitleProperty.PropertyName)
            {
                var page = (Page)sender;
                var index = GetTabIndex(this.TabbedPage, page);
                if (index != -1)
                {
                    if (this.IsBottomTabPlacement)
                    {
                        var tab = this.bottomNavigationView.Menu.GetItem(index);
                        tab?.SetTitle(page.Title);
                    }
                    else
                    {
                        var tab = this.topTabLayout.GetTabAt(index);
                        tab?.SetText(page.Title);
                    }
                }
            }

            if (this.badgeViews.TryGetValue(element, out var badgeView))
            {
                badgeView.UpdateFromPropertyChangedEvent(element, e);
            }
        }

        private void OnTabRemoved(object sender, ElementEventArgs e)
        {
            e.Element.PropertyChanged -= this.OnTabbedPagePropertyChanged;
            this.badgeViews.Remove(e.Element);
        }

        private async void OnTabAdded(object sender, ElementEventArgs e)
        {
            await Task.Delay(DelayBeforeTabAdded);

            if (e.Element is not Page page)
            {
                return;
            }

            this.AddTabBadge(this.TabbedPage.Children.IndexOf(page));
        }

        protected override void DisconnectHandler(Android.Views.View platformView)
        {
            base.DisconnectHandler(platformView);

            if (this.VirtualView is TabbedPage customTabbedPage)
            {
                customTabbedPage.Loaded -= this.TabbedPage_Loaded;
            }

            this.Cleanup(this.TabbedPage);
        }

        private void Cleanup(TabbedPage tabbedPage)
        {
            if (tabbedPage == null)
            {
                return;
            }

            foreach (var page in tabbedPage.Children.Select(PageHelper.GetPageWithBadge))
            {
                page.PropertyChanged -= this.OnTabbedPagePropertyChanged;
            }

            tabbedPage.ChildRemoved -= this.OnTabRemoved;
            tabbedPage.ChildAdded -= this.OnTabAdded;

            this.badgeViews.Clear();
            this.topTabLayout = null;
            this.topTabStrip = null;
            this.bottomTabStrip = null;
            this.bottomNavigationView = null;
        }
    }
}