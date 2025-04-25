
using TabbedPageDemoApp.ViewModels;

namespace TabbedPageDemoApp.Views
{
    public partial class ChatPage : ContentPage
    {
        public ChatPage()
        {
            this.InitializeComponent();
            this.BindingContext = IPlatformApplication.Current.Services.GetService<ChatViewModel>();
        }
    }
}