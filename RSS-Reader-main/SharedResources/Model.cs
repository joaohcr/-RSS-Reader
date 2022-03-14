using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace SharedResources
{
    public class Model
    {
        public event MethodWithFeed UpdFeedEvent;
        public event MethodWithFeed DelFeedEvent;

        // Static Properties
        public static string AppFolder
        {
            get
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                return $"{path}/RSSReader";
            }
        }

        // Properties
        public ObservableCollection<Category> Categories { get; set; }

        public ObservableCollection<Notification> Notifications { get; set; }

        // Constructor
        public Model()
        {
            Categories = new ObservableCollection<Category>();
            Notifications = new ObservableCollection<Notification>();
            Notification.FindItemEvent += Notification_FindItemEvent;
            RSSFeed.UpdNotifEvent += RSSFeed_UpdNotifEvent;
            RSSFeed.DelNotifEvent += RSSFeed_DelNotifEvent;
        }

        // Methods
        public void Load()
        {
            if (Directory.Exists(AppFolder))
            {
                foreach (string CategoryDir in Directory.GetDirectories(AppFolder))
                {
                    Category category = new();
                    category.ReadCategory(CategoryDir);
                    Categories.Add(category);
                }
                string path = $"{AppFolder}/notifications.xml";
                if (File.Exists(path))
                {
                    XDocument document = XDocument.Load(path);
                    var elements = document.Root.Elements("notification");
                    foreach (XElement element in elements)
                    {
                        Notification notification = new();
                        notification.ReadNotification(element);
                        if (notification.RSSItem != null)
                            Notifications.Add(notification);
                    }
                }
            }
        }

        public void Save()
        {
            if (!Directory.Exists(AppFolder))
            {
                Directory.CreateDirectory(AppFolder);
            }
            foreach (Category category in Categories)
            {
                category.SaveCategory();
            }
            XDocument document = new(new XElement("notifications"));
            foreach (Notification notification in Notifications)
            {
                document.Element("notifications").Add(notification.SaveNotification());
            }
            document.Save($"{AppFolder}/notifications.xml");
        }

        public void UpdateFeeds()
        {
            if (!IsConnectedToInternet()) return;
            foreach (Category category in Categories)
            {
                foreach (RSSFeed feed in category.Feeds)
                {
                    feed.Update();
                    SendNotification(feed);
                    UpdFeedEvent?.Invoke(feed);
                }
            }
            Save();
        }

        public void UpdateFeed(RSSFeed feed)
        {
            if (!IsConnectedToInternet()) return;
            feed.Update();
            SendNotification(feed);
            Save();
        }

        public void SendNotification(RSSFeed feed)
        {
            foreach (RSSItem item in feed.RSSItems)
            {
                foreach (string keyword in feed.Keywords)
                {
                    if (item.Title.Contains(keyword) || item.Summary.Contains(keyword))
                    {
                        bool Exists = false;
                        foreach (Notification notification in Notifications)
                        {
                            if (notification.RSSItem == item)
                            {
                                Exists = true;
                                break;
                            }
                        }
                        if (!Exists)
                            Notifications.Add(new Notification(item));
                    }
                }
            }
        }

        // Static Methods
        public static string MakeValidFileName(string name)
        {
            string invalidChars = Regex.Escape(new string(Path.GetInvalidFileNameChars()));
            string invalidRegStr = string.Format(@"([{0}]*\.+$)|([{0}]+)", invalidChars);
            return Regex.Replace(name, invalidRegStr, "_");
        }

        public static bool DownloadImageFromUrl(string imageUrl, string path)
        {
            try
            {
                using WebClient client = new();
                client.DownloadFile(imageUrl, path);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool IsConnectedToInternet()
        {
            try
            {
                Ping myPing = new();
                string host = "google.com";
                byte[] buffer = new byte[32];
                int timeout = 1000;
                PingOptions pingOptions = new();
                PingReply reply = myPing.Send(host, timeout, buffer, pingOptions);
                return reply.Status == IPStatus.Success;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // Event Handlers
        public void DelFeedEventHandler(RSSFeed feed)
        {
            DelFeedEvent?.Invoke(feed);
        }

        private void RSSFeed_UpdNotifEvent(RSSItem item)
        {
            foreach (Notification notification in Notifications)
            {
                if (notification.RSSItem.Link == item.Link)
                {
                    notification.RSSItem = item;
                    break;
                }
            }
        }

        public void RSSFeed_DelNotifEvent(RSSItem item)
        {
            for (int i = 0; i < Notifications.Count; i++)
            {
                if (Notifications[i].RSSItem == item)
                {
                    Notifications.RemoveAt(i);
                    break;
                }
            }
        }

        private RSSItem Notification_FindItemEvent(string link)
        {
            foreach (Category category in Categories)
            {
                foreach (RSSFeed feed in category.Feeds)
                {
                    foreach (RSSItem item in feed.RSSItems)
                    {
                        if (item.Item.Links[0].Uri.AbsoluteUri == link)
                        {
                            return item;
                        }
                    }
                }
            }
            return null;
        }

    }
}
