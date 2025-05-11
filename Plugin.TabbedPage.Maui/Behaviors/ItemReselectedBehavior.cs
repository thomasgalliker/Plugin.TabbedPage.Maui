using System.Windows.Input;

namespace Plugin.TabbedPage.Maui.Behaviors
{
    public class ItemReselectedBehavior : BehaviorBase<Microsoft.Maui.Controls.TabbedPage>
    {
        public static readonly BindableProperty ItemReselectedCommandProperty = BindableProperty.Create(
            nameof(ItemReselectedCommand),
            typeof(ICommand),
            typeof(ItemReselectedBehavior));

        public ICommand ItemReselectedCommand
        {
            get => (ICommand)this.GetValue(ItemReselectedCommandProperty);
            set => this.SetValue(ItemReselectedCommandProperty, value);
        }

        public event EventHandler<ItemReselectedEventArgs> ItemReselected;

        internal void RaiseItemReselectedEvent(Page page)
        {
            this.ItemReselected?.Invoke(this, new ItemReselectedEventArgs(page));
        }
    }
}