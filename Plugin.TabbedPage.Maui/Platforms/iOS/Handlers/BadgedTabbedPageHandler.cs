using System.Diagnostics;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Platform;
using Plugin.TabbedPage.Maui.Controls;
using Plugin.TabbedPage.Maui.Utils;
using UIKit;
using Font = Microsoft.Maui.Font;
using Page = Microsoft.Maui.Controls.Page;
using TabbedRenderer = Microsoft.Maui.Controls.Handlers.Compatibility.TabbedRenderer;

namespace Plugin.TabbedPage.Maui.Platform
{
    using TabbedPage = Microsoft.Maui.Controls.TabbedPage;

    [Preserve]
    public class BadgedTabbedPageHandler : TabbedRenderer
    {
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            // make sure we clean up old event registrations
            this.Cleanup(e.OldElement as TabbedPage);
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            // make sure we clean up old event registrations
            this.Cleanup(this.Tabbed);

            for (var i = 0; i < this.TabBar.Items.Length; i++)
            {
                this.AddTabBadge(i);
            }

            this.Tabbed.ChildAdded += this.OnTabAdded;
            this.Tabbed.ChildRemoved += this.OnTabRemoved;
        }

        private void AddTabBadge(int tabIndex)
        {
            if (tabIndex == -1)
            {
                return;
            }

            var element = PageHelper.GetChildPageWithBadge(this.Tabbed, tabIndex);
            element.PropertyChanged += this.OnTabbedPagePropertyChanged;

            if (this.TabBar.Items.Length > tabIndex)
            {
                var tabBarItem = this.TabBar.Items[tabIndex];
                this.UpdateTabBadgeText(tabBarItem, element);
                this.UpdateTabBadgeColor(tabBarItem, element);
                this.UpdateTabBadgeTextAttributes(tabBarItem, element);
            }
        }

        private void UpdateTitle(UITabBarItem tabBarItem, Page page)
        {
            var text = page.Title;

            tabBarItem.Title = string.IsNullOrEmpty(text) ? null : text;
        }

        private void UpdateTabBadgeText(UITabBarItem tabBarItem, Element element)
        {
            var text = TabBadge.GetBadgeText(element);

            tabBarItem.BadgeValue = string.IsNullOrEmpty(text) ? null : text;
        }

        private void UpdateTabBadgeTextAttributes(UITabBarItem tabBarItem, Element element)
        {
            var attrs = new UIStringAttributes();

            var textColor = TabBadge.GetBadgeTextColor(element);
            if (textColor.IsNotDefault())
            {
                attrs.ForegroundColor = textColor.ToPlatform();
            }

            var font = TabBadge.GetBadgeFont(element);
            if (font != Font.Default)
            {
                attrs.Font = font.ToUIFont((element.Handler ?? Application.Current.Handler).MauiContext.Services
                    .GetRequiredService<IFontManager>());
            }

            tabBarItem.SetBadgeTextAttributes(attrs, UIControlState.Normal);
        }

        private void UpdateTabBadgeColor(UITabBarItem tabBarItem, Element element)
        {
            var tabColor = TabBadge.GetBadgeColor(element);
            if (tabColor.IsNotDefault())
            {
                tabBarItem.BadgeColor = tabColor.ToPlatform();
            }
        }

        private void OnTabbedPagePropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (sender is not Page page)
            {
                return;
            }

            if (e.PropertyName == Page.TitleProperty.PropertyName)
            {
                if (this.CheckValidTabIndex(page, out var tabIndex))
                {
                    this.UpdateTitle(this.TabBar.Items[tabIndex], page);
                }
            }
            else if (e.PropertyName == Page.IconImageSourceProperty.PropertyName)
            {
                // #65 update badge properties if icon changed
                if (this.CheckValidTabIndex(page, out var tabIndex))
                {
                    this.UpdateTabBadgeText(this.TabBar.Items[tabIndex], page);
                    this.UpdateTabBadgeColor(this.TabBar.Items[tabIndex], page);
                    this.UpdateTabBadgeTextAttributes(this.TabBar.Items[tabIndex], page);
                }
            }
            else if (e.PropertyName == TabBadge.BadgeTextProperty.PropertyName)
            {
                if (this.CheckValidTabIndex(page, out var tabIndex))
                {
                    this.UpdateTabBadgeText(this.TabBar.Items[tabIndex], page);
                }
            }
            else if (e.PropertyName == TabBadge.BadgeColorProperty.PropertyName)
            {
                if (this.CheckValidTabIndex(page, out var tabIndex))
                {
                    this.UpdateTabBadgeColor(this.TabBar.Items[tabIndex], page);
                }
            }
            else if (e.PropertyName == TabBadge.BadgeTextColorProperty.PropertyName ||
                     e.PropertyName == TabBadge.BadgeFontProperty.PropertyName)
            {
                if (this.CheckValidTabIndex(page, out var tabIndex))
                {
                    this.UpdateTabBadgeTextAttributes(this.TabBar.Items[tabIndex], page);
                }
            }
        }

        protected bool CheckValidTabIndex(Page page, out int tabIndex)
        {
            tabIndex = this.Tabbed.Children.IndexOf(page);
            if (tabIndex == -1 && page.Parent != null && page.Parent is Page pageParent)
            {
                tabIndex = this.Tabbed.Children.IndexOf(pageParent);
            }

            return tabIndex >= 0 && tabIndex < this.TabBar.Items.Length;
        }

        private async void OnTabAdded(object sender, ElementEventArgs e)
        {
            // Workaround: tabbar is not updated at this point and we have no way of knowing for sure when it will be updated.
            // so we have to wait ...
            await Task.Delay(10);
            if (e.Element is not Page page)
            {
                return;
            }

            var tabIndex = this.Tabbed.Children.IndexOf(page);
            this.AddTabBadge(tabIndex);
        }

        private void OnTabRemoved(object sender, ElementEventArgs e)
        {
            e.Element.PropertyChanged -= this.OnTabbedPagePropertyChanged;
        }

        protected override void Dispose(bool disposing)
        {
            this.Cleanup(this.Tabbed);

            base.Dispose(disposing);
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

            tabbedPage.ChildAdded -= this.OnTabAdded;
            tabbedPage.ChildRemoved -= this.OnTabRemoved;
        }
    }
}