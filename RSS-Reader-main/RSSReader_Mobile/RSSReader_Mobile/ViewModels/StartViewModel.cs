using SharedResources;
using SharedResources.ViewModels;
using System.Collections.ObjectModel;
using System.Linq;

namespace RSSReader_Mobile.ViewModels
{
    public class StartViewModel : MainViewModel
    {
        public event MethodWithoutArg UpdFeedEvent;

        // Properties
        private ObservableCollection<RSSItem> _UnreadItems;
        public ObservableCollection<RSSItem> UnreadItems
        {
            get => _UnreadItems;
            set
            {
                _UnreadItems = value;
                OnPropertyChanged(nameof(UnreadItems));
            }
        }

        // Constructor
        public StartViewModel(Model model) : base(model)
        {
            UnreadItems = new ObservableCollection<RSSItem>();
            Model.DelFeedEvent -= base.Model_DelFeedEvent;
            Model.UpdFeedEvent -= base.Model_UpdFeedEvent;
            Model.DelFeedEvent += Model_DelFeedEvent;
            Model.UpdFeedEvent += Model_UpdFeedEvent;
        }


        // Methods
        private new void Model_DelFeedEvent(RSSFeed feed)
        {
            if(SelectedFeed == feed)
            {
                UnreadItems.Clear();
                SelectedFeed = null;
            }
        }

        private new void Model_UpdFeedEvent(RSSFeed feed)
        {
            if(SelectedFeed == feed)
            {
                UnreadItems.Clear();
                var collection = from item in SelectedFeed.RSSItems
                                 where item.IsRead == false
                                 select item;
                UnreadItems = new ObservableCollection<RSSItem>(collection);
                UpdFeedEvent?.Invoke();
            }
        }

        public new void ShowFeed(RSSFeed feed)
        {
            var collection = from item in feed.RSSItems
                             where item.IsRead == false
                             select item;
            UnreadItems = new ObservableCollection<RSSItem>(collection);
            base.ShowFeed(feed);
        }

        public new void ShowItem()
        {
            UnreadItems.Remove(SelectedItem);
            base.ShowItem();
        }

        public new void UpdateFeed()
        {
            base.UpdateFeed();
            UnreadItems.Clear();
            var collection = from item in SelectedFeed.RSSItems
                             where item.IsRead == false
                             select item;
            UnreadItems = new ObservableCollection<RSSItem>(collection);
        }
    }
}
