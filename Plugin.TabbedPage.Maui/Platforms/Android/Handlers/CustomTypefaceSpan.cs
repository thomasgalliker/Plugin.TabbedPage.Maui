using Android.Content;
using Android.Graphics;
using Android.Text;
using Android.Text.Style;

namespace Plugin.TabbedPage.Maui.Platform.Handlers
{
    internal class CustomTypefaceSpan : TypefaceSpan
    {
        // https://github.com/Gamhub-io/MobileApp/blob/7a310d23b24827e263a16bb7413b306041c83e22/App/Platforms/Android/Renderers/MyBottomNavigationView.cs#L12
        private readonly float fontSize;
        private readonly Typeface typeface;

        public CustomTypefaceSpan(string fontFamily, float fontSize, Typeface typeface) : base(fontFamily)
        {
            this.fontSize = fontSize;
            this.typeface = typeface;
        }

        public override void UpdateDrawState(TextPaint ds)
        {
            ApplyCustomTypeFace(ds, this.fontSize, this.typeface);
        }

        public override void UpdateMeasureState(TextPaint paint)
        {
            ApplyCustomTypeFace(paint, this.fontSize, this.typeface);
        }

        private static void ApplyCustomTypeFace(TextPaint paint, float fontSize, Typeface typeface)
        {
            TypefaceStyle oldStyle;
            var old = paint.Typeface;
            if (old == null)
            {
                oldStyle = 0;
            }
            else
            {
                oldStyle = old.Style;
            }

            var fake = oldStyle & ~typeface.Style;
            if ((fake & TypefaceStyle.Bold) != 0)
            {
                paint.FakeBoldText = true;
            }

            if ((fake & TypefaceStyle.Italic) != 0)
            {
                paint.TextSkewX = -0.25f;
            }

            if (fontSize > 0f)
            {
                paint.TextSize = fontSize;
            }

            paint.SetTypeface(typeface);
        }
    }
}