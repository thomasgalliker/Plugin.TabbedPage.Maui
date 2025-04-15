using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Superdev.Maui.Localization;
using Superdev.Maui.Services;
using IPreferences = Superdev.Maui.Services.IPreferences;

namespace TabbedPageDemoApp.ViewModels
{
    public class AccountViewModel : ObservableObject
    {
        private readonly IMainThread mainThread;
        private readonly IPreferences preferences;
        private readonly ILocalizer localizer;

        private IRelayCommand appearingCommand;
        private LanguageViewModel[] languages;
        private LanguageViewModel language;

        public AccountViewModel(
            IMainThread mainThread,
            IPreferences preferences,
            ILocalizer localizer)
        {
            this.mainThread = mainThread;
            this.preferences = preferences;
            this.localizer = localizer;
        }

        public IRelayCommand AppearingCommand
        {
            get => this.appearingCommand ??= new RelayCommand(this.OnAppearing);
        }

        private void OnAppearing()
        {
            this.Languages = new[]
                {
                    new CultureInfo("de"),
                    new CultureInfo("en")
                }
                .Select(ci => new LanguageViewModel(ci))
                .ToArray();

            var languageSetting = this.preferences.Get<string>("Language", null);
            if (languageSetting != null)
            {
                this.language = new LanguageViewModel(new CultureInfo(languageSetting));
                this.OnPropertyChanged(nameof(this.Language));
            }
        }

        public LanguageViewModel[] Languages
        {
            get => this.languages;
            private set => this.SetProperty(ref this.languages, value);
        }

        public LanguageViewModel Language
        {
            get => this.language;
            set
            {
                if (this.SetProperty(ref this.language, value))
                {
                    if (value != null)
                    {
                        this.mainThread.BeginInvokeOnMainThread(() => this.UpdateLanguage(value.CultureInfo));
                    }
                }
            }
        }

        private void UpdateLanguage(CultureInfo newValue)
        {
            this.localizer.SetCultureInfo(newValue);

            this.preferences.Set("Language", newValue.TwoLetterISOLanguageName);

            this.OnPropertyChanged("");
            this.Languages = this.Languages.ToArray();
        }
    }
}