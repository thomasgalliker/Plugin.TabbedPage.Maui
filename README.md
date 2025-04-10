# Extended TabbedPage features for .NET MAUI

[![Version](https://img.shields.io/nuget/v/Plugin.TabbedPage.Maui.svg)](https://www.nuget.org/packages/Plugin.TabbedPage.Maui)  [![Downloads](https://img.shields.io/nuget/dt/Plugin.TabbedPage.Maui.svg)](https://www.nuget.org/packages/Plugin.TabbedPage.Maui)

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