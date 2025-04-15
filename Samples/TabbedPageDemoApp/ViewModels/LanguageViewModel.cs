using System.Globalization;
using Superdev.Maui.Mvvm;
using TabbedPageDemoApp.Resources.Text;

namespace TabbedPageDemoApp.ViewModels
{
    public class LanguageViewModel : BaseViewModel, IEquatable<LanguageViewModel>
    {
        public LanguageViewModel(CultureInfo cultureInfo)
        {
            this.CultureInfo = cultureInfo;
        }

        public CultureInfo CultureInfo { get; }

        public string LanguageName
        {
            get
            {
                var twoLetterIsoLanguageName = GetBaseCulture(this.CultureInfo).TwoLetterISOLanguageName;
                var languageName = Strings.ResourceManager.GetString($"CultureInfo_{twoLetterIsoLanguageName}_NativeName");
                return languageName;
            }
        }

        private static CultureInfo GetBaseCulture(CultureInfo cultureInfo)
        {
            if (cultureInfo.Parent is CultureInfo cultureInfoParent &&
                !Equals(cultureInfoParent, CultureInfo.InvariantCulture))
            {
                cultureInfo = cultureInfoParent;
            }

            return cultureInfo;
        }

        public bool Equals(LanguageViewModel other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Equals(this.CultureInfo, other.CultureInfo);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return this.Equals((LanguageViewModel)obj);
        }

        public override int GetHashCode()
        {
            return this.CultureInfo != null ? this.CultureInfo.GetHashCode() : 0;
        }

        public void RaisePropertyChanged()
        {
            this.RaisePropertyChanged("");
            this.RaisePropertyChanged(nameof(this.LanguageName));
        }
    }
}