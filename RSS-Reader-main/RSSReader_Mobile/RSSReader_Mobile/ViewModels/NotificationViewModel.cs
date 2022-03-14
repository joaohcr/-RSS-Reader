using SharedResources;
using SharedResources.ViewModels;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;

namespace RSSReader_Mobile.ViewModels
{
    public class NotificationViewModel : BaseViewModel
    {
        private readonly App app;

        // Properties
        public ObservableCollection<Notification> Notifications { get; set; }

        private Notification _SelectedNotification;
        public Notification SelectedNotification
        {
            get => _SelectedNotification;
            set
            {
                _SelectedNotification = value;
                OnPropertyChanged(nameof(SelectedNotification));
            }
        }

        // Constructor
        public NotificationViewModel()
        {
            app = Application.Current as App;
            var collection = from notification in app.Model.Notifications
                             where notification.IsFiled == false
                             select notification;
            Notifications = new ObservableCollection<Notification>(collection);
        }

        public void OpenNotification()
        {
            SelectedNotification.IsRead = true;
            SelectedNotification.IsFiled = true;
            Notifications.Remove(SelectedNotification);
            ShowItemEvent?.Invoke(SelectedNotification.RSSItem);
        }

        public void FileNotification(Notification notification)
        {
            notification.IsFiled = true;
            Notifications.Remove(notification);
        }

        // Events
        public event MethodWithItem ShowItemEvent;
    }
}
