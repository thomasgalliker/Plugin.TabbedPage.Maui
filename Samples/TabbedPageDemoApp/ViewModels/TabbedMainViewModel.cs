using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace TabbedPageDemoApp.ViewModels
{
    public class TabbedMainViewModel : ViewModelBase, IDisposable
    {
        private double fontSize;

        public TabbedMainViewModel()
        {
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

        public class FontSizeChangedMessage(double fontSize) : ValueChangedMessage<double>(fontSize);

        public void Dispose()
        {
            WeakReferenceMessenger.Default.UnregisterAll(this);
        }
    }
}