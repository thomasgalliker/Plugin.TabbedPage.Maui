using System.Windows.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using TabbedPageDemoApp.Services;

namespace TabbedPageDemoApp.ViewModels
{
    public class TabbedMainViewModel : ViewModelBase, IDisposable
    {
        private readonly IDialogService dialogService;

        private double fontSize;
        private ICommand itemReselectedCommand;

        public TabbedMainViewModel(
            IDialogService dialogService)
        {
            this.dialogService = dialogService;
            this.FontSize = 16;

            WeakReferenceMessenger.Default.Register<FontSizeChangedMessage>(this, this.OnMessageReceived);
        }

        private void OnMessageReceived(object recipient, FontSizeChangedMessage message)
        {
            this.FontSize = message.Value;
        }

        public double FontSize
        {
            get => this.fontSize;
            private set => this.SetProperty(ref this.fontSize, value);
        }

        public ICommand ItemReselectedCommand
        {
            get => this.itemReselectedCommand ??= new Command<Page>(this.OnItemReselected);
        }

        private void OnItemReselected(Page page)
        {
            _ = this.dialogService.DisplayAlertAsync(
                "ItemReselectedCommand",
                page.GetType().Name,
                "OK");
        }

        public class FontSizeChangedMessage(double fontSize) : ValueChangedMessage<double>(fontSize);

        public void Dispose()
        {
            WeakReferenceMessenger.Default.UnregisterAll(this);
        }
    }
}