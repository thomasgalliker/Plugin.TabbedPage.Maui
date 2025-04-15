using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using CoreFoundation;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Platform;
using Plugin.TabbedPage.Maui.Controls;
using Plugin.TabbedPage.Maui.Extensions;
using Plugin.TabbedPage.Maui.Utils;
using UIKit;
using Font = Microsoft.Maui.Font;
using Page = Microsoft.Maui.Controls.Page;
using TabbedRenderer = Microsoft.Maui.Controls.Handlers.Compatibility.TabbedRenderer;

namespace Plugin.TabbedPage.Maui.Platform
{
    using TabbedPageExtensions = Controls.TabbedPage;
    using TabbedPageExtensionsiOS = Controls.PlatformConfiguration.iOSSpecific;
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

            this.UpdateTitleTextAttributes();
            this.UpdateBadgeBackgroundColor();

            this.Tabbed.ChildAdded += this.OnTabAdded;
            this.Tabbed.ChildRemoved += this.OnTabRemoved;
            this.Tabbed.PropertyChanged += this.OnTabbedPagePropertyChanged;
        }

        private void OnTabbedPagePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == TabbedPageExtensions.FontFamilyProperty.PropertyName ||
                e.PropertyName == TabbedPageExtensions.FontSizeProperty.PropertyName ||
                e.PropertyName == TabbedPageExtensions.FontAttributesProperty.PropertyName)
            {
                this.UpdateTitleTextAttributes();
            }
            else if (e.PropertyName == TabbedPageExtensionsiOS.TabbedPage.NormalBadgeBackgroundColorProperty.PropertyName ||
                     e.PropertyName == TabbedPageExtensionsiOS.TabbedPage.SelectedBadgeBackgroundColorProperty.PropertyName)
            {
                this.UpdateBadgeBackgroundColor();
            }
        }

        private void UpdateBadgeBackgroundColor()
        {
            if (!UIDevice.CurrentDevice.CheckSystemVersion(13, 0))
            {
                return;
            }

            var normalBadgeBackgroundColor = TabbedPageExtensionsiOS.TabbedPage.GetNormalBadgeBackgroundColor(this.Tabbed);
            var selectedBadgeBackgroundColor = TabbedPageExtensionsiOS.TabbedPage.GetSelectedBadgeBackgroundColor(this.Tabbed);

            if (normalBadgeBackgroundColor.IsDefault() &&
                selectedBadgeBackgroundColor.IsDefault())
            {
                return;
            }

            var normalBadgeBackground = normalBadgeBackgroundColor.ToPlatform();
            var selectedBadgeBackground = selectedBadgeBackgroundColor.ToPlatform();

            if (this.TabBar.StandardAppearance is UITabBarAppearance tabBarAppearance)
            {
                UpdateBadgeBackgroundColor(tabBarAppearance,
                    normal => normal.BadgeBackgroundColor = normalBadgeBackground,
                    selected => selected.BadgeBackgroundColor = selectedBadgeBackground);
            }

            if (UIDevice.CurrentDevice.CheckSystemVersion(15, 0))
            {
                if (this.TabBar.ScrollEdgeAppearance is UITabBarAppearance scrollEdgeAppearance)
                {
                    UpdateBadgeBackgroundColor(scrollEdgeAppearance,
                        normal => normal.BadgeBackgroundColor = normalBadgeBackground,
                        selected => selected.BadgeBackgroundColor = selectedBadgeBackground);
                }
            }

            this.TabBar.SetNeedsLayout();
        }

        private static void UpdateBadgeBackgroundColor(
            UITabBarAppearance tabBarAppearance,
            Action<UITabBarItemStateAppearance> normal,
            Action<UITabBarItemStateAppearance> selected)
        {
            if (tabBarAppearance.StackedLayoutAppearance is UITabBarItemAppearance stackedLayoutAppearance)
            {
                normal(stackedLayoutAppearance.Normal);
                selected(stackedLayoutAppearance.Selected);
            }

            if (tabBarAppearance.InlineLayoutAppearance is UITabBarItemAppearance inlineLayoutAppearance)
            {
                normal(inlineLayoutAppearance.Normal);
                selected(inlineLayoutAppearance.Selected);
            }

            if (tabBarAppearance.CompactInlineLayoutAppearance is UITabBarItemAppearance compactInlineLayoutAppearance)
            {
                normal(compactInlineLayoutAppearance.Normal);
                selected(compactInlineLayoutAppearance.Selected);
            }
        }

        private void UpdateTitleTextAttributes()
        {
            if (!UIDevice.CurrentDevice.CheckSystemVersion(13, 0))
            {
                return;
            }

            var fontFamily = TabbedPageExtensions.GetFontFamily(this.Tabbed);
            var fontSize = TabbedPageExtensions.GetFontSize(this.Tabbed);
            var fontAttributes = TabbedPageExtensions.GetFontAttributes(this.Tabbed);

            if (Equals(fontFamily, TabbedPageExtensions.FontFamilyProperty.DefaultValue) &&
                Equals(fontSize, TabbedPageExtensions.FontSizeProperty.DefaultValue) &&
                Equals(fontAttributes, TabbedPageExtensions.FontAttributesProperty.DefaultValue))
            {
                return;
            }

            var fontNormal = FontHelper.CreateFont(
                fontFamily,
                fontSize,
                fontAttributes);

            var fontManager = this.Tabbed.Handler.GetRequiredService<IFontManager>();
            var uiFontNormal = fontManager.GetFont(fontNormal);

            if (!fontAttributes.HasFlag(FontAttributes.Bold))
            {
                fontNormal = fontNormal.WithWeight(FontWeight.Medium);
            }

            var uiFontSelected = fontManager.GetFont(fontNormal);

            if (fontFamily != null)
            {
                var fontDescriptor = uiFontSelected.FontDescriptor.CreateWithTraits(UIFontDescriptorSymbolicTraits.Bold);
                if (fontDescriptor != null)
                {
                    uiFontSelected = UIFont.FromDescriptor(fontDescriptor, 0.0f); // 0.0f - keep the same size
                }
            }

            Trace.WriteLine($"UpdateTabBarAppearance: uiFontSelected={uiFontSelected}, uiFontNormal={uiFontNormal}");

            // var tabBarItemAppearance = new UITabBarItemAppearance();

            var normalAttributes = new UIStringAttributes
            {
                Font = uiFontNormal,
                //ForegroundColor = UIColor.Green,
            };

            var selectedAttributes = new UIStringAttributes
            {
                Font = uiFontSelected,
                //ForegroundColor = UIColor.Magenta,
            };

            if (this.TabBar.StandardAppearance is UITabBarAppearance tabBarAppearance)
            {
                UpdateBadgeBackgroundColor(tabBarAppearance,
                    normal => normal.TitleTextAttributes = normalAttributes,
                    selected => selected.TitleTextAttributes = selectedAttributes);
            }

            if (UIDevice.CurrentDevice.CheckSystemVersion(15, 0))
            {
                if (this.TabBar.ScrollEdgeAppearance is UITabBarAppearance scrollEdgeAppearance)
                {
                    UpdateBadgeBackgroundColor(scrollEdgeAppearance,
                        normal => normal.TitleTextAttributes = normalAttributes,
                        selected => selected.TitleTextAttributes = selectedAttributes);
                }
            }

            this.TabBar.SetNeedsLayout();
        }

        private void AddTabBadge(int tabIndex)
        {
            if (tabIndex == -1)
            {
                return;
            }

            var page = PageHelper.GetChildPageWithBadge(this.Tabbed, tabIndex);
            page.PropertyChanged += this.OnPagePropertyChanged;

            if (this.TabBar.Items.Length > tabIndex)
            {
                var tabBarItem = this.TabBar.Items[tabIndex];
                this.UpdateTabBadgeText(tabBarItem, page);
                this.UpdateTabBadgeColor(tabBarItem, page);
                this.UpdateTabBadgeTextAttributes(tabBarItem, page);
            }
        }

        private void UpdateTabTitle(UITabBarItem tabBarItem, Page page)
        {
            var text = page.Title;

            tabBarItem.Title = string.IsNullOrEmpty(text) ? null : text;
        }

        private void UpdateTabBadgeText(UITabBarItem tabBarItem, Element element)
        {
            var text = TabBadge.GetBadgeText(element);
            tabBarItem.BadgeValue = string.IsNullOrEmpty(text) ? null : text;
        }

        private void UpdateTabBadgeTextAttributes(UITabBarItem tabBarItem, Page page)
        {
            var stringAttributes = new UIStringAttributes();

            var textColor = TabBadge.GetBadgeTextColor(page);
            if (textColor.IsNotDefault())
            {
                stringAttributes.ForegroundColor = textColor.ToPlatform();
            }

            var font = TabBadge.GetBadgeFont(page);
            if (font != Font.Default)
            {
                var fontManager = this.Tabbed.Handler.GetRequiredService<IFontManager>();
                var uiFont = fontManager.GetFont(font);
                stringAttributes.Font = uiFont;
            }

            tabBarItem.SetBadgeTextAttributes(stringAttributes, UIControlState.Normal);
        }

        private void UpdateTabBadgeColor(UITabBarItem tabBarItem, Element element)
        {
            var tabColor = TabBadge.GetBadgeColor(element);
            if (tabColor.IsNotDefault())
            {
                tabBarItem.BadgeColor = tabColor.ToPlatform();
            }
        }

        private void OnPagePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Trace.WriteLine($"OnPagePropertyChanged: {e.PropertyName}");

            if (sender is not Page page)
            {
                return;
            }

            if (e.PropertyName == Page.TitleProperty.PropertyName)
            {
                if (this.CheckValidTabIndex(page, out var tabIndex))
                {
                    var tabBarItem = this.TabBar.Items[tabIndex];
                    this.UpdateTabTitle(tabBarItem, page);

                    MainThread.BeginInvokeOnMainThread(async () =>
                    {
                        // HACK: This delay is necessary because we want to update the tab font
                        // after MAUI's TabBar has finished updating the title text.
                        await Task.Delay(10);

                        var tabBarItem = this.TabBar.Items[tabIndex];
                        this.UpdateTabBadgeText(tabBarItem, page);
                    });
                }
            }
            else if (e.PropertyName == Page.IconImageSourceProperty.PropertyName)
            {
                // #65 update badge properties if icon changed
                if (this.CheckValidTabIndex(page, out var tabIndex))
                {
                    var tabBarItem = this.TabBar.Items[tabIndex];
                    this.UpdateTabBadgeText(tabBarItem, page);
                    this.UpdateTabBadgeColor(tabBarItem, page);
                    this.UpdateTabBadgeTextAttributes(tabBarItem, page);
                }
            }
            else if (e.PropertyName == TabBadge.BadgeTextProperty.PropertyName)
            {
                if (this.CheckValidTabIndex(page, out var tabIndex))
                {
                    var tabBarItem = this.TabBar.Items[tabIndex];
                    this.UpdateTabBadgeText(tabBarItem, page);
                }
            }
            else if (e.PropertyName == TabBadge.BadgeColorProperty.PropertyName)
            {
                if (this.CheckValidTabIndex(page, out var tabIndex))
                {
                    var tabBarItem = this.TabBar.Items[tabIndex];
                    this.UpdateTabBadgeColor(tabBarItem, page);
                }
            }
            else if (e.PropertyName == TabBadge.BadgeTextColorProperty.PropertyName ||
                     e.PropertyName == TabBadge.BadgeFontProperty.PropertyName)
            {
                if (this.CheckValidTabIndex(page, out var tabIndex))
                {
                    var tabBarItem = this.TabBar.Items[tabIndex];
                    this.UpdateTabBadgeTextAttributes(tabBarItem, page);
                }
            }
        }

        protected bool CheckValidTabIndex(Page page, out int tabIndex)
        {
            tabIndex = this.Tabbed.Children.IndexOf(page);
            if (tabIndex == -1 && page.Parent is Page pageParent)
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
            e.Element.PropertyChanged -= this.OnPagePropertyChanged;
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
                page.PropertyChanged -= this.OnPagePropertyChanged;
            }

            tabbedPage.ChildAdded -= this.OnTabAdded;
            tabbedPage.ChildRemoved -= this.OnTabRemoved;
            tabbedPage.PropertyChanged -= this.OnTabbedPagePropertyChanged;
        }
    }
}