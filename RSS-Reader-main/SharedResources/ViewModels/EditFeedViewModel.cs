using System.Collections.ObjectModel;

namespace SharedResources.ViewModels
{
    public class EditFeedViewModel : BaseViewModel
    {
        // Events
        public event MethodWithoutArg GoBack;

        // Properties
        public Model Model { get; private set; }

        private Category _SelectedCategory;
        public Category SelectedCategory
        {
            get => _SelectedCategory;
            set
            {
                _SelectedCategory = value;
                OnPropertyChanged(nameof(SelectedCategory));
            }
        }

        private RSSFeed _SelectedFeed;
        public RSSFeed SelectedFeed
        {
            get => _SelectedFeed;
            set
            {
                _SelectedFeed = value;
                Keywords.Clear();
                if (SelectedFeed != null)
                {
                    foreach (string keyword in SelectedFeed.Keywords)
                    {
                        Keywords.Add(keyword);
                    }
                }
                OnPropertyChanged(nameof(SelectedFeed));
            }
        }

        public string KeywordTextBox { get; set; }

        public ObservableCollection<string> Keywords { get; set; }

        public string SelectedKeyword { get; set; }

        // Constructor
        public EditFeedViewModel(Model model)
        {
            Model = model;
            Keywords = new ObservableCollection<string>();
        }

        // Methods
        public void AddKeyword()
        {
            if (string.IsNullOrWhiteSpace(KeywordTextBox) == false)
            {
                Keywords.Add(KeywordTextBox);
            }
        }

        public void DelKeyword()
        {
            if (SelectedKeyword != null)
            {
                Keywords.Remove(SelectedKeyword);
            }
        }

        public void SaveFeed()
        {
            if (SelectedFeed != null)
            {
                SelectedFeed.Keywords = Keywords;
                Model.SendNotification(SelectedFeed);
                GoBack?.Invoke();
            }
        }

        public void DelFeed()
        { 
            if (SelectedFeed != null)
            {
                RSSFeed feed = SelectedFeed;
                foreach (RSSItem item in feed.RSSItems)
                {
                    Model.RSSFeed_DelNotifEvent(item);
                }
                SelectedCategory.Feeds.Remove(feed);
                Model.DelFeedEventHandler(feed);
                feed.DelContent();
                GoBack?.Invoke();
            }
        }
    }
}
