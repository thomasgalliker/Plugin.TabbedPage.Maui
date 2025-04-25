# Extended TabbedPage features for .NET MAUI

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
`tbd`

### Contribution
Contributors welcome! If you find a bug or want to propose a new feature, feel free to do so by opening a new issue on GitHub.

### Links
- https://github.com/xabre/xamarin-forms-tab-badge
- https://github.com/dotnet/maui/blob/main/src/Controls/src/Core/Platform/Android/TabbedPageManager.cs
- https://github.com/vmontoyagtz/SaamBk/blob/85d605f7a4e458aaa39b5e9d4f6871c7802c53e0/src/SaamApp.MauiApps/eShopOnContainers/Controls/CustomTabbedPage.cs#L5