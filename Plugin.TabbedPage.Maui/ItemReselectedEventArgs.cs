namespace Plugin.TabbedPage.Maui
{
    public class ItemReselectedEventArgs : EventArgs
    {
        public ItemReselectedEventArgs(Page page)
        {
            this.Page = page;
        }

        public Page Page { get; }
    }
}