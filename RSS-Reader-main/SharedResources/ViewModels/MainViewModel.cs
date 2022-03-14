using System.Linq;

namespace SharedResources.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        // Properties
        public Model Model { get; set; }

        private bool _NotRead;
        public bool NotRead
        {
            get => _NotRead;
            set
            {
                _NotRead = value;
                OnPropertyChanged(nameof(NotRead));
            }
        }

        private RSSFeed _SelectedFeed;
        public RSSFeed SelectedFeed
        {
            get => _SelectedFeed;
            set
            {
                _SelectedFeed = value;
                OnPropertyChanged(nameof(SelectedFeed));
            }
        }

        public RSSItem SelectedItem { get; set; }

        // Constructor
        public MainViewModel(Model model)
        {
            Model = model;
            var collection = from category in Model.Categories
                             where category.Feeds.Count != 0
                             select category;
            if (collection.Count() != 0)
                SelectedFeed = collection.First().Feeds[0];
            Model.DelFeedEvent += Model_DelFeedEvent;
            Model.UpdFeedEvent += Model_UpdFeedEvent;
        }

        // Methods
        public void AddCategory(string name)
        {
            if (string.IsNullOrWhiteSpace(name) == false)
            {
                Model.Categories.Add(new Category(name));
            }
            else
            {
                ShowError?.Invoke("Categoria em falta.");
            }
        }

        public void OpenNotification(Notification notification)
        {
            notification.IsRead = true;
            notification.IsFiled = true;
            ShowItemEvent?.Invoke(notification.RSSItem);
        }

        public void ShowItem()
        {
            ShowItemEvent?.Invoke(SelectedItem);
        }

        public void ShowFeed(RSSFeed feed)
        {
            SelectedFeed = feed;
            ShowFeedEvent?.Invoke();
        }

        public void UpdateFeed()
        {
            Model.UpdateFeed(SelectedFeed);
            OnPropertyChanged(nameof(SelectedFeed));
        }

        // Event Handlers
        public void Model_UpdFeedEvent(RSSFeed feed)
        {
            if (SelectedFeed == feed)
            {
                OnPropertyChanged(nameof(SelectedFeed));
            }
        }

        public void Model_DelFeedEvent(RSSFeed feed)
        {
            if (SelectedFeed == feed)
            {
                SelectedFeed = null;
            }
        }

        // Events
        public event MethodWithoutArg ShowFeedEvent;
        public event MethodWithString ShowError;
        public event MethodWithItem ShowItemEvent;
    }
}
