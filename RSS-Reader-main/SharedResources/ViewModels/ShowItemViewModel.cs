namespace SharedResources.ViewModels
{
    public class ShowItemViewModel
    {
        // Properties
        public RSSItem RSSItem { get; set; }

        // Constructor
        public ShowItemViewModel(RSSItem item)
        {
            RSSItem = item;
            RSSItem.IsRead = true;
        }

        // Methods
        public void OpenInBrowser()
        {
            OpenInBrowserEvent?.Invoke(RSSItem.Link);
        }

        // Events
        public event MethodWithUri OpenInBrowserEvent;
    }
}