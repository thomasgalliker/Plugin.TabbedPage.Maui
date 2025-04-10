using Font = Microsoft.Maui.Font;

namespace Plugin.TabbedPage.Maui.Controls
{
    public class Badge : Frame
    {
        public static BindableProperty BadgeTextProperty = BindableProperty.Create(
            nameof(BadgeText),
            typeof(string),
            typeof(Badge),
            propertyChanged: BadgePropertyChanged);

        public string BadgeText
        {
            get => (string)this.GetValue(BadgeTextProperty);
            set => this.SetValue(BadgeTextProperty, value);
        }

        public static BindableProperty BadgeTextColorProperty = BindableProperty.Create(
            nameof(BadgeTextColor),
            typeof(Color),
            typeof(Badge),
            propertyChanged: BadgePropertyChanged);

        public Color BadgeTextColor
        {
            get => (Color)this.GetValue(BadgeTextColorProperty);
            set => this.SetValue(BadgeTextColorProperty, value);
        }

        public static BindableProperty BadgeFontAttributesProperty = BindableProperty.Create(
            nameof(BadgeFontAttributes),
            typeof(FontAttributes),
            typeof(Badge),
            FontAttributes.Bold,
            propertyChanged: BadgePropertyChanged);

        public FontAttributes BadgeFontAttributes
        {
            get => (FontAttributes)this.GetValue(BadgeFontAttributesProperty);
            set => this.SetValue(BadgeFontAttributesProperty, value);
        }

        public static BindableProperty BadgeFontFamilyProperty = BindableProperty.Create(
            nameof(BadgeFontFamily),
            typeof(string),
            typeof(Badge),
            Font.Default.Family,
            propertyChanged: BadgePropertyChanged);

        public string BadgeFontFamily
        {
            get => (string)this.GetValue(BadgeFontFamilyProperty);
            set => this.SetValue(BadgeFontFamilyProperty, value);
        }

        public static BindableProperty BadgeFontSizeProperty = BindableProperty.Create(
            nameof(BadgeFontSizeProperty),
            typeof(double),
            typeof(Badge),
            8.0,
            propertyChanged: BadgePropertyChanged);

        public double BadgeFontSize
        {
            get => (double)this.GetValue(BadgeFontSizeProperty);
            set => this.SetValue(BadgeFontSizeProperty, value);
        }

        private new Label Content => (Label)base.Content;

        public Badge()
        {
            base.Content = new Label
            {
                VerticalOptions = LayoutOptions.Fill,
                HorizontalOptions = LayoutOptions.Fill,
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.Center
            };

            this.Padding = new Thickness(7, 3);
            this.VerticalOptions = LayoutOptions.Start;
            this.HorizontalOptions = LayoutOptions.End;
            this.BackgroundColor = Colors.Red;

            this.UpdateBadgeProperties();
        }

        protected virtual void UpdateBadgeProperties()
        {
            if (this.Content == null)
            {
                return;
            }

            if (this.Content.FontAttributes != this.BadgeFontAttributes)
            {
                this.Content.FontAttributes = this.BadgeFontAttributes;
            }

            if (!string.IsNullOrWhiteSpace(this.BadgeFontFamily))
            {
                this.Content.FontFamily = this.BadgeFontFamily;
            }

            var fontSize = this.BadgeFontSize > 0 ? this.BadgeFontSize : this.Content.FontSize;
            if (this.Content.FontSize != fontSize)
            {
                this.Content.FontSize = fontSize;
            }

            if (this.Content.TextColor != this.BadgeTextColor)
            {
                this.Content.TextColor = this.BadgeTextColor;
            }

            var isVisible = !string.IsNullOrEmpty(this.BadgeText);
            if (this.IsVisible != isVisible)
            {
                this.IsVisible = isVisible;
            }

            if (this.Content.Text != this.BadgeText)
            {
                this.Content.Text = this.BadgeText;
            }
        }

        private static void BadgePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as Badge)?.UpdateBadgeProperties();
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            this.UpdateCornerRadius(height);
        }

        private void UpdateCornerRadius(double heightHint)
        {
            var cornerRadius = this.Height > 0f ? (float)this.Height / 2f : heightHint > 0 ? (float)heightHint / 2f : (float)(this.BadgeFontSize + this.Padding.VerticalThickness) / 2f;
            if (this.CornerRadius != cornerRadius)
            {
                this.CornerRadius = cornerRadius;
            }
        }
    }
}