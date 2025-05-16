# Extended Features for TabbedPage in .NET MAUI

[![Version](https://img.shields.io/nuget/v/Plugin.TabbedPage.Maui.svg)](https://www.nuget.org/packages/Plugin.TabbedPage.Maui) [![Downloads](https://img.shields.io/nuget/dt/Plugin.TabbedPage.Maui.svg)](https://www.nuget.org/packages/Plugin.TabbedPage.Maui) [![Buy Me a Coffee](https://img.shields.io/badge/support-buy%20me%20a%20coffee-FFDD00)](https://buymeacoffee.com/thomasgalliker)

This library extends the existing TabbedPage in .NET MAUI with new features such as badge text.

### Download and Install Plugin.TabbedPage.Maui
This library is available on NuGet: [Plugin.TabbedPage.Maui](https://www.nuget.org/packages/Plugin.TabbedPage.Maui)  
Use the following command to install `Plugin.TabbedPage.Maui` using the NuGet package manager console:

```powershell
PM> Install-Package Plugin.TabbedPage.Maui
```

You can use this library in any .NET MAUI project compatible with .NET 8 and higher.

#### App Setup
This plugin provides an extension method for `MauiAppBuilder` called `UseTabbedPage`, which ensures proper startup and initialization.  
Call this method within your `MauiProgram` just as demonstrated in the [TabbedPageDemoApp](https://github.com/thomasgalliker/Plugin.TabbedPage.Maui/tree/develop/Samples):

```csharp
var builder = MauiApp.CreateBuilder()
    .UseMauiApp<App>()
    .UseTabbedPage();
```

### API Usage
The following sections provide code snippets demonstrating how to use TabbedPage extensions. 
All available features and their usage can be explored in the sample app included in this repository.

#### General coloring
`tbd`
- BarBackgroundColor
- UnselectedTabColor
- SelectedTabColor

#### FontFamily and FontSize
`tbd`
- t:TabbedPage.FontFamily
- t:TabbedPage.FontSize

#### Android-specific options
`tbd`
- t:TabBadge.BadgePosition
- t:TabBadge.BadgeText
- ta:TabBadge.BadgeColor
- ta:TabBadge.BadgeTextColor

#### iOS-specific options
`tbd`
- ti:TabbedPage.NormalBadgeBackgroundColor
- ti:TabbedPage.SelectedBadgeBackgroundColor

#### Item Reselection
ItemReselected is a feature used in a TabbedPage to detect when a user taps on a tab that is already selected.
Unlike regular selection events that respond only to tab changes, ItemReselected triggers when the current tab is re-tapped. This is useful for refreshing content, scrolling to the top, or resetting the state within the active tab.

Apply ItemReselectedBehavior to the TabbedPage as demonstrated below. 
You can either use the ItemReselected event with an event handler in code-behind or bind a command to ItemReselectedCommand from your binding context.
```xml
<TabbedPage
    xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
    xmlns:t="http://plugin.tabbedpage.maui">
    <TabbedPage.Behaviors>
        <t:ItemReselectedBehavior
            ItemReselected="OnItemReselected"
            ItemReselectedCommand="{Binding ItemReselectedCommand}" />
    </TabbedPage.Behaviors>
```

### Contribution
Contributors welcome! If you find a bug or want to propose a new feature, feel free to do so by opening a new issue on GitHub.

### Links
- https://github.com/xabre/xamarin-forms-tab-badge
- https://github.com/dotnet/maui/blob/main/src/Controls/src/Core/Platform/Android/TabbedPageManager.cs
- https://github.com/vmontoyagtz/SaamBk/blob/85d605f7a4e458aaa39b5e9d4f6871c7802c53e0/src/SaamApp.MauiApps/eShopOnContainers/Controls/CustomTabbedPage.cs#L5