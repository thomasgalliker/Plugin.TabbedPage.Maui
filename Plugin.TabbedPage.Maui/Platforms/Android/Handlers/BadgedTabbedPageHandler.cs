using System.ComponentModel;
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
using System.Diagnostics;
using Android.Text;
using Android.Text.Style;
using Microsoft.Maui.Platform;
using Plugin.TabbedPage.Maui.Extensions;
using Plugin.TabbedPage.Maui.Controls;
using Plugin.TabbedPage.Maui.Platform.Handlers;
using Application = Microsoft.Maui.Controls.Application;

namespace Plugin.TabbedPage.Maui.Platform
{
    using TabbedPageExtensions = Controls.TabbedPage;
    using TabbedPage = Microsoft.Maui.Controls.TabbedPage;
    using TabBadgeExtensionsAndroid = Controls.PlatformConfiguration.AndroidSpecific.TabBadge;


    public class BadgedTabbedPageHandler : TabbedViewHandler
    {
        private static readonly TimeSpan DelayBeforeTabAdded = TimeSpan.FromMilliseconds(50);

        private readonly Dictionary<Page, BadgeView> badgeViews = new Dictionary<Page, BadgeView>();

        private TabLayout topTabLayout;
        private LinearLayout topTabStrip;
        private ViewGroup bottomTabStrip;
        private BottomNavigationView bottomNavigationView;

        protected TabbedPage TabbedPage => this.VirtualView as TabbedPage;
        protected ViewGroup ViewGroup => this.PlatformView as ViewGroup;

        protected override void ConnectHandler(View platformView)
        {
            base.ConnectHandler(platformView);

            if (this.VirtualView is TabbedPage tabbedPage)
            {
                //this.VirtualView.AddCleanUpEvent(); // Not needed because, pages call DisconnectHandler automagically
                tabbedPage.Loaded += this.OnTabbedPageLoaded;
                tabbedPage.PropertyChanged += this.OnTabbedPagePropertyChanged;
            }
        }

        private void OnTabbedPagePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == TabbedPageExtensions.FontFamilyProperty.PropertyName ||
                e.PropertyName == TabbedPageExtensions.FontSizeProperty.PropertyName ||
                e.PropertyName == TabbedPageExtensions.FontAttributesProperty.PropertyName)
            {
                this.UpdateTabbedPageFont();
            }
        }

        private void OnTabbedPageLoaded(object sender, EventArgs e)
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
                this.UpdateTabbedPageFont();
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

        private void UpdateTabbedPageFont()
        {
            var fontFamily = TabbedPageExtensions.GetFontFamily(this.TabbedPage);
            var fontSize = TabbedPageExtensions.GetFontSize(this.TabbedPage);
            var fontAttributes = TabbedPageExtensions.GetFontAttributes(this.TabbedPage);

            if (Equals(fontFamily, TabbedPageExtensions.FontFamilyProperty.DefaultValue) &&
                Equals(fontSize, TabbedPageExtensions.FontSizeProperty.DefaultValue) &&
                Equals(fontAttributes, TabbedPageExtensions.FontAttributesProperty.DefaultValue))
            {
                return;
            }

            var typefaceResolver = this.GetRequiredService<ITypefaceResolver>();
            var typeface = typefaceResolver.GetTypeface(
                fontFamily,
                fontSize,
                fontAttributes);

            var fontSizePx = PixelConverter.DipToPixels(this.Context, (float)fontSize);

            if (this.IsBottomTabPlacement)
            {
                var menu = this.bottomNavigationView.Menu;
                for (var i = 0; i < this.bottomNavigationView.Menu.Size(); i++)
                {
                    var menuItem = menu.GetItem(i);
                    var title = menuItem.TitleFormatted;
                    var sb = new SpannableStringBuilder(title);
                    var typefaceSpan = new CustomTypefaceSpan(fontFamily, fontSizePx, typeface);
                    sb.SetSpan(typefaceSpan, 0, sb.Length(), SpanTypes.InclusiveInclusive);
                    menuItem.SetTitle(sb);
                }
            }
            else
            {
                // TODO: Implement for top navigation view
                //throw new NotImplementedException();
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

            var badgeView = targetLayout.FindChildOfType<BadgeView>();
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

            page.PropertyChanged -= this.OnPagePropertyChanged;
            page.PropertyChanged += this.OnPagePropertyChanged;
        }

        private static int GetTabIndex(TabbedPage tabbedPage, Page page)
        {
            var tabPages = PageHelper.GetChildPages(tabbedPage).ToList();
            var tabIndex = tabPages.IndexOf(page);
            return tabIndex;
        }

        protected virtual void OnPagePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender is not Page page)
            {
                return;
            }

            if (e.PropertyName == Page.TitleProperty.PropertyName)
            {
                var index = GetTabIndex(this.TabbedPage, page);
                if (index != -1)
                {
                    if (this.IsBottomTabPlacement)
                    {
                        var menuItem = this.bottomNavigationView.Menu.GetItem(index);
                        if (menuItem != null)
                        {
                            if (menuItem.TitleFormatted?.ToString() != page.Title)
                            {
                                menuItem.SetTitle(page.Title);

                                MainThread.BeginInvokeOnMainThread(async () =>
                                {
                                    // HACK: This delay is necessary because we want to update the tab font
                                    // after MAUI's TabbedPageManager has finished updating the title text.
                                    // https://github.com/dotnet/maui/blob/ef1ec07908eda7b7bf9979fbbf9c374fbe7f2acf/src/Controls/src/Core/Platform/Android/TabbedPageManager.cs#L420
                                    await Task.Delay(10);
                                    this.UpdateTabbedPageFont();
                                });
                            }
                        }
                    }
                    else
                    {
                        var tab = this.topTabLayout.GetTabAt(index);
                        if (tab != null)
                        {
                            tab.SetText(page.Title);
                        }
                    }
                }
            }
            else  if (e.PropertyName == TabbedPageExtensions.FontFamilyProperty.PropertyName ||
                      e.PropertyName == TabbedPageExtensions.FontSizeProperty.PropertyName||
                      e.PropertyName == TabbedPageExtensions.FontAttributesProperty.PropertyName)
            {
                this.UpdateTabbedPageFont();
            }

            if (this.badgeViews.TryGetValue(page, out var badgeView))
            {
                if (e.PropertyName == TabBadge.BadgeTextProperty.PropertyName)
                {
                    badgeView.Text = TabBadge.GetBadgeText(page);
                }
                else if (e.PropertyName == TabBadgeExtensionsAndroid.BadgeColorProperty.PropertyName)
                {
                    var badgeColor = TabBadgeExtensionsAndroid.GetBadgeColor(page);
                    badgeView.BadgeColor = badgeColor.ToPlatform();
                }
                else if (e.PropertyName == TabBadgeExtensionsAndroid.BadgeTextColorProperty.PropertyName)
                {
                    var badgeTextColor = TabBadgeExtensionsAndroid.GetBadgeTextColor(page);
                    badgeView.TextColor = badgeTextColor.ToPlatform();
                }
                else if (e.PropertyName == TabBadge.BadgeFontProperty.PropertyName)
                {
                    var fontManager = page.Handler.GetRequiredService<IFontManager>();
                    var badgeFont = TabBadge.GetBadgeFont(page);
                    badgeView.Typeface = badgeFont.ToTypeface(fontManager);
                }
                else if (e.PropertyName == TabBadge.BadgePositionProperty.PropertyName)
                {
                    badgeView.Postion = TabBadge.GetBadgePosition(page);
                }
                else if (e.PropertyName == TabBadge.BadgeMarginProperty.PropertyName)
                {
                    var badgeMargin = TabBadge.GetBadgeMargin(page);
                    badgeView.SetMargins((float)badgeMargin.Left, (float)badgeMargin.Top, (float)badgeMargin.Right, (float)badgeMargin.Bottom);
                }
            }
        }

        private void OnTabRemoved(object sender, ElementEventArgs e)
        {
            if (e.Element is not Page page)
            {
                return;
            }

            page.PropertyChanged -= this.OnPagePropertyChanged;
            this.badgeViews.Remove(page);
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

            if (this.VirtualView is TabbedPage tabbedPage)
            {
                tabbedPage.Loaded -= this.OnTabbedPageLoaded;
                tabbedPage.PropertyChanged -= this.OnTabbedPagePropertyChanged;
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
                page.PropertyChanged -= this.OnPagePropertyChanged;
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