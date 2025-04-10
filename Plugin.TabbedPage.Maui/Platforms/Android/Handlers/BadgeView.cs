using Android.Graphics;
using Android.Util;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;
using Android.Graphics.Drawables.Shapes;
using AndroidX.Core.View;
using View = Android.Views.View;
using Animation = Android.Views.Animations.Animation;
using Color = Android.Graphics.Color;
using Context = Android.Content.Context;
using ShapeDrawable = Android.Graphics.Drawables.ShapeDrawable;
using Plugin.TabbedPage.Maui.Controls;
using System.Diagnostics;

namespace Plugin.TabbedPage.Maui.Platform.Handlers
{
    public class BadgeView : TextView
    {
        private const int DefaultLrPaddingDip = 4;
        private const int DefaultCornerRadiusDip = 7;

        private static Animation FadeInAnimation;
        private static Animation FadeOutAnimation;

        private Context context;
        private readonly Color defaultBadgeColor = Color.ParseColor("#CCFF0000");
        private ShapeDrawable backgroundShape;
        private BadgePosition position;


        private int badgeMarginL;
        private int badgeMarginR;
        private int badgeMarginT;
        private int badgeMarginB;

        private bool hasWrappedLayout;

        public static int TextSizeDip { get; set; } = 11;

        public View Target { get; private set; }

        public BadgePosition Postion
        {
            get => this.position;

            set
            {
                if (this.position == value)
                {
                    return;
                }

                this.position = value;
                this.ApplyLayoutParams();
            }
        }

        public Color BadgeColor
        {
            get => this.backgroundShape.Paint.Color;
            set
            {
                this.backgroundShape.Paint.Color = value;

                this.Background.InvalidateSelf();
            }
        }

        public Color TextColor
        {
            get => new Color(this.CurrentTextColor);
            set => this.SetTextColor(value);
        }

        public void SetMargins(float left, float top, float right, float bottom)
        {
            this.badgeMarginL = this.DipToPixels(left);
            this.badgeMarginT = this.DipToPixels(top);
            this.badgeMarginR = this.DipToPixels(right);
            this.badgeMarginB = this.DipToPixels(bottom);

            this.ApplyLayoutParams();
        }

        /// <summary>
        /// Creates a badge view for a given view by wrapping both views in a new layout.
        /// </summary>
        /// <returns>The view.</returns>
        /// <param name="context">Context.</param>
        /// <param name="target">Target.</param>
        public static BadgeView ForTarget(Context context, View target)
        {
            var badgeView = new BadgeView(context, null, Android.Resource.Attribute.TextViewStyle);
            badgeView.WrapTargetWithLayout(target);
            return badgeView;
        }

        /// <summary>
        /// Creates a bage view and adds it to the specified layout without adding any additionaly wrapping layouts.
        ///
        /// </summary>
        /// <returns>The layout.</returns>
        /// <param name="context">Context.</param>
        /// <param name="target">Target</param>
        public static BadgeView ForTargetLayout(Context context, View target)
        {
            var badgeView = new BadgeView(context, null, Android.Resource.Attribute.TextViewStyle);
            badgeView.AddToTargetLayout(target);
            return badgeView;
        }

        private BadgeView(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle)
        {
            this.Init(context);
        }

        private void Init(Context context)
        {
            this.context = context;

            this.Typeface = Typeface.DefaultBold;
            var paddingPixels = this.DipToPixels(DefaultLrPaddingDip);
            this.SetPadding(paddingPixels, 0, paddingPixels, 0);
            this.SetTextColor(Color.White);
            this.SetTextSize(ComplexUnitType.Dip, TextSizeDip);

            FadeInAnimation = new AlphaAnimation(0, 1) { Interpolator = new DecelerateInterpolator(), Duration = 200 };

            FadeOutAnimation = new AlphaAnimation(1, 0) { Interpolator = new AccelerateInterpolator(), Duration = 200 };

            this.backgroundShape = this.CreateBackgroundShape();
            ViewCompat.SetBackground(this, this.backgroundShape);
            this.BadgeColor = this.defaultBadgeColor;

            this.Visibility = ViewStates.Gone;
        }

        private ShapeDrawable CreateBackgroundShape()
        {
            var radius = this.DipToPixels(DefaultCornerRadiusDip);
            var outerR = new float[] { radius, radius, radius, radius, radius, radius, radius, radius };

            return new ShapeDrawable(new RoundRectShape(outerR, null, null));
        }

        private void AddToTargetLayout(View target)
        {
            if (target.Parent is not ViewGroup layout)
            {
                Trace.WriteLine("Plugin.TabbedPage.Maui: Badge target parent has to be a view group");
                return;
            }

            layout.SetClipChildren(false);
            layout.SetClipToPadding(false);

            layout.AddView(this);

            this.Target = target;
        }

        private void WrapTargetWithLayout(View target)
        {
            var lp = target.LayoutParameters;
            var parent = target.Parent;

            if (parent is not ViewGroup group)
            {
                Trace.WriteLine("Plugin.TabbedPage.Maui: Badge target parent has to be a view group");
                return;
            }

            group.SetClipChildren(false);
            group.SetClipToPadding(false);

            var container = new FrameLayout(this.context);
            var index = group.IndexOfChild(target);

            group.RemoveView(target);
            group.AddView(container, index, lp);

            container.AddView(target);
            group.Invalidate();

            container.AddView(this);

            this.Target = target;
            this.hasWrappedLayout = true;
        }

        public void Show()
        {
            this.Show(false, null);
        }

        public void Show(bool animate)
        {
            this.Show(animate, FadeInAnimation);
        }


        public void Hide(bool animate)
        {
            this.Hide(animate, FadeOutAnimation);
        }

        private void Show(bool animate, Animation anim)
        {
            this.ApplyLayoutParams();

            if (animate)
            {
                this.StartAnimation(anim);
            }

            this.Visibility = ViewStates.Visible;
        }

        private void Hide(bool animate, Animation anim)
        {
            this.Visibility = ViewStates.Gone;
            if (animate)
            {
                this.StartAnimation(anim);
            }
        }

        private void ApplyLayoutParams()
        {
            var layoutParameters = new FrameLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);

            if (!this.hasWrappedLayout)
            {
                var targetParams = (FrameLayout.LayoutParams)this.Target.LayoutParameters;
                var w = targetParams.Width / 2;
                var h = targetParams.Height / 2;

                layoutParameters.Gravity = GravityFlags.Center;
                switch (this.Postion)
                {
                    case BadgePosition.TopLeft:
                        layoutParameters.SetMargins(this.badgeMarginL - w, this.badgeMarginT - h, 0, 0);
                        break;
                    case BadgePosition.TopRight:
                        layoutParameters.SetMargins(0, this.badgeMarginT - h, this.badgeMarginR - w, 0);
                        break;
                    case BadgePosition.BottomLeft:
                        layoutParameters.SetMargins(this.badgeMarginL - w, 0, 0, 0 + this.badgeMarginB - h);
                        break;
                    case BadgePosition.BottomRight:
                        layoutParameters.SetMargins(0, 0, this.badgeMarginR - w, 0 + this.badgeMarginB - h);
                        break;
                    case BadgePosition.Center:
                        layoutParameters.SetMargins(this.badgeMarginL, this.badgeMarginT, this.badgeMarginR, this.badgeMarginB);
                        break;
                    case BadgePosition.TopCenter:
                        layoutParameters.SetMargins(0, 0 + this.badgeMarginT - h, 0, 0);
                        break;
                    case BadgePosition.BottomCenter:
                        layoutParameters.SetMargins(0, 0, 0, 0 + this.badgeMarginB - h);
                        break;
                    case BadgePosition.LeftCenter:
                        layoutParameters.SetMargins(this.badgeMarginL - w, 0, 0, 0);
                        break;
                    case BadgePosition.RightCenter:
                        layoutParameters.SetMargins(0, 0, this.badgeMarginR - w, 0);
                        break;
                }
            }
            else
            {
                switch (this.Postion)
                {
                    case BadgePosition.TopLeft:
                        layoutParameters.Gravity = GravityFlags.Left | GravityFlags.Top;
                        layoutParameters.SetMargins(this.badgeMarginL, this.badgeMarginT, 0, 0);
                        break;
                    case BadgePosition.TopRight:
                        layoutParameters.Gravity = GravityFlags.Right | GravityFlags.Top;
                        layoutParameters.SetMargins(0, this.badgeMarginT, this.badgeMarginR, 0);
                        break;
                    case BadgePosition.BottomLeft:
                        layoutParameters.Gravity = GravityFlags.Left | GravityFlags.Bottom;
                        layoutParameters.SetMargins(this.badgeMarginL, 0, 0, this.badgeMarginB);
                        break;
                    case BadgePosition.BottomRight:
                        layoutParameters.Gravity = GravityFlags.Right | GravityFlags.Bottom;
                        layoutParameters.SetMargins(0, 0, this.badgeMarginR, this.badgeMarginB);
                        break;
                    case BadgePosition.Center:
                        layoutParameters.Gravity = GravityFlags.Center;
                        layoutParameters.SetMargins(0, 0, 0, 0);
                        break;
                    case BadgePosition.TopCenter:
                        layoutParameters.Gravity = GravityFlags.Center | GravityFlags.Top;
                        layoutParameters.SetMargins(0, this.badgeMarginT, 0, 0);
                        break;
                    case BadgePosition.BottomCenter:
                        layoutParameters.Gravity = GravityFlags.Center | GravityFlags.Bottom;
                        layoutParameters.SetMargins(0, 0, 0, this.badgeMarginB);
                        break;
                    case BadgePosition.LeftCenter:
                        layoutParameters.Gravity = GravityFlags.Left | GravityFlags.Center;
                        layoutParameters.SetMargins(this.badgeMarginL, 0, 0, 0);
                        break;
                    case BadgePosition.RightCenter:
                        layoutParameters.Gravity = GravityFlags.Right | GravityFlags.Center;
                        layoutParameters.SetMargins(0, 0, this.badgeMarginR, 0);
                        break;
                }
            }

            this.LayoutParameters = layoutParameters;
        }

        private int DipToPixels(float dip)
        {
            return (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, dip, this.Resources.DisplayMetrics);
        }

        public new string Text
        {
            get => base.Text;
            set
            {
                base.Text = value;

                switch (this.Visibility)
                {
                    case ViewStates.Visible when string.IsNullOrEmpty(value):
                        this.Hide(true);
                        break;
                    case ViewStates.Gone when !string.IsNullOrEmpty(value):
                        this.Show(true);
                        break;
                }
            }
        }
    }
}