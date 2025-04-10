using CommunityToolkit.Mvvm.ComponentModel;

namespace TabbedPageDemoApp.ViewModels
{
    public class ViewModelBase : ObservableObject
    {
        private bool isBusy;

        public bool IsBusy
        {
            get => this.isBusy;
            set => this.SetProperty(ref this.isBusy, value);
        }
    }
}