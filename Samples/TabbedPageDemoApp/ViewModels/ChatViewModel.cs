using CommunityToolkit.Mvvm.ComponentModel;
using Superdev.Maui.Services;

namespace TabbedPageDemoApp.ViewModels
{
    public class ChatViewModel : ObservableObject
    {
        private readonly Random rng = new Random();
        private int? counter;

        public ChatViewModel(IMainThread mainThread)
        {
            mainThread.BeginInvokeOnMainThread(async () =>
            {
                // await Task.Delay(2000);
                // for (var i = 1; i <= 100; i++)
                // {
                //     this.Counter = i;
                //     await Task.Delay(this.rng.Next(0, i*2));
                // }
            });
        }

        public int? Counter
        {
            get => this.counter;
            set
            {
                if (this.SetProperty(ref this.counter, value))
                {
                    this.OnPropertyChanged(nameof(this.CounterText));
                }
            }
        }

        public string CounterText
        {
            get
            {
                if (this.Counter == null)
                {
                    return null;
                }

                if (this.Counter <= 0)
                {
                    return string.Empty;
                }

                if (this.Counter > 99)
                {
                    return $"99+";
                }

                return $"{this.Counter}";
            }
        }
    }
}