using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Plugin.TabbedPage.Maui.Controls;

namespace TabbedPageDemoApp.ViewModels
{
    public class CarTireAlertViewModel : ObservableObject
    {
        private double fontSize;
        private string badgeText;
        private BadgePosition[] badgePositions;
        private BadgePosition badgePosition;
        private Color[] badgeColors;
        private Color badgeColor;
        private Color[] badgeTextColors;
        private Color badgeTextColor;

        public CarTireAlertViewModel()
        {
            this.fontSize = 16;
            this.BadgeText = "!";
            this.BadgePositions = Enum.GetValues<BadgePosition>();
            this.BadgePosition = BadgePosition.TopRight;

            this.BadgeColors = new[]
            {
                Colors.Red,
                Colors.Green,
                Colors.Blue,
                Colors.Magenta
            };
            this.BadgeColor = this.BadgeColors.First();

            this.BadgeTextColors = new[]
            {
                Colors.White,
                Colors.Red,
                Colors.Green,
                Colors.Blue,
                Colors.Magenta
            };
            this.BadgeTextColor = this.BadgeTextColors.First();
        }

        public double FontSize
        {
            get => this.fontSize;
            set
            {
                if (this.SetProperty(ref this.fontSize, value))
                {
                    WeakReferenceMessenger.Default.Send(new TabbedMainViewModel.FontSizeChangedMessage(value));
                }
            }
        }

        public string BadgeText
        {
            get => this.badgeText;
            set => this.SetProperty(ref this.badgeText, value);
        }

        public BadgePosition BadgePosition
        {
            get => this.badgePosition;
            set => this.SetProperty(ref this.badgePosition, value);
        }

        public BadgePosition[] BadgePositions
        {
            get => this.badgePositions;
            private set => this.SetProperty(ref this.badgePositions, value);
        }

        public Color[] BadgeColors
        {
            get => this.badgeColors;
            private set => this.SetProperty(ref this.badgeColors, value);
        }

        public Color BadgeColor
        {
            get => this.badgeColor;
            set => this.SetProperty(ref this.badgeColor, value);
        }

        public Color[] BadgeTextColors
        {
            get => this.badgeTextColors;
            private set => this.SetProperty(ref this.badgeTextColors, value);
        }

        public Color BadgeTextColor
        {
            get => this.badgeTextColor;
            set => this.SetProperty(ref this.badgeTextColor, value);
        }
    }
}