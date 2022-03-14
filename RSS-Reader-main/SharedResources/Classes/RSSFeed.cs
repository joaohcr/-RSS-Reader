using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.ServiceModel.Syndication;
using System.Threading;
using System.Xml;
using System.Xml.Linq;

namespace SharedResources
{
    public class RSSFeed
    {
        public static event MethodWithItem UpdNotifEvent;
        public static event MethodWithItem DelNotifEvent;

        public Uri Link { get; private set; }

        public SyndicationFeed Feed { get; private set; }

        public string ImageSource { get; private set; }

        public ObservableCollection<RSSItem> RSSItems { get; private set; }

        public ObservableCollection<string> Keywords { get; set; }

        public string OfflineContent { get; set; }

        public DateTime LastUpdate { get; private set; }

        public RSSFeed()
        {
            RSSItems = new ObservableCollection<RSSItem>();
            Keywords = new ObservableCollection<string>();
        }

        public RSSFeed(string link, ObservableCollection<string> keywords, string path) : this()
        {
            Link = new Uri(link);
            Feed = SyndicationFeed.Load(XmlReader.Create(link));
            if (Feed.ImageUrl != null)
            {
                string imgfile = $"{path}/{Model.MakeValidFileName(Feed.Title.Text)}.jpg";
                if (Model.DownloadImageFromUrl(Feed.ImageUrl.AbsoluteUri, imgfile))
                {
                    ImageSource = imgfile;
                }
            }
            foreach (SyndicationItem item in Feed.Items)
            {
                RSSItems.Add(new RSSItem(item, path));
            }
            Keywords = keywords;
            LastUpdate = DateTime.Now;
            OfflineContent = $"{path}/{Model.MakeValidFileName(Feed.Title.Text)}.xml";
        }

        private int isBusy = 0;
        public void Update()
        {
            if (Interlocked.CompareExchange(ref isBusy, 1, 0) == 1) return;
            if (!Model.IsConnectedToInternet()) return;
            bool flag = false;
            SyndicationFeed newFeed = SyndicationFeed.Load(XmlReader.Create(Link.AbsoluteUri));
            if (Feed.LastUpdatedTime != newFeed.LastUpdatedTime)
            {
                Feed = newFeed;
                if (Feed.ImageUrl != null)
                {
                    Model.DownloadImageFromUrl(Feed.ImageUrl.AbsoluteUri, ImageSource);
                }
                LastUpdate = DateTime.Now;
                ObservableCollection<RSSItem> newCollection = new();
                foreach (SyndicationItem item in Feed.Items)
                {
                    newCollection.Add(new RSSItem(item, Path.GetDirectoryName(OfflineContent)));
                }
                foreach (RSSItem oldItem in RSSItems)
                {
                    flag = false;
                    foreach (RSSItem newItem in newCollection)
                    {
                        if (oldItem.Link.AbsoluteUri == newItem.Link.AbsoluteUri)
                        {
                            newItem.IsRead = oldItem.IsRead;
                            flag = true;
                        }
                    }
                    if (!flag)
                    {
                        oldItem.DelContent();
                        DelNotifEvent?.Invoke(oldItem);
                    }
                }
                RSSItems = newCollection;
                foreach (RSSItem item in RSSItems)
                {
                    UpdNotifEvent?.Invoke(item);
                }
            }
            isBusy = 0;
        }

        public void DelContent()
        {
            if (ImageSource != null && File.Exists(ImageSource))
            {
                File.Delete(ImageSource);
            }
            File.Delete(OfflineContent);
            foreach (RSSItem item in RSSItems)
            {
                item.DelContent();
            }
            RSSItems.Clear();
        }

        private int isBusy2 = 0;
        public void SaveFeed()
        {
            if (Interlocked.CompareExchange(ref isBusy2, 1, 0) == 1) return;
            XmlWriter writer = XmlWriter.Create(OfflineContent, new() { Indent = true });
            Feed.ElementExtensions.Clear();
            foreach (string keyword in Keywords)
            {
                Feed.ElementExtensions.Add(new XElement("keyword", keyword).CreateReader());
            }
            Feed.ElementExtensions.Add(new XElement("imageSource", ImageSource).CreateReader());
            Feed.ElementExtensions.Add(new XElement("lastUpdate", LastUpdate).CreateReader());
            Feed.ElementExtensions.Add(new XElement("feedUrl", Link).CreateReader());
            foreach (RSSItem item in RSSItems)
            {
                item.SaveItem();
            }
            Feed.SaveAsRss20(writer);
            writer.Close();
            isBusy = 0;
        }

        public void ReadFeed(string file)
        {
            Feed = SyndicationFeed.Load(XmlReader.Create(file));
            OfflineContent = file;
            foreach (SyndicationItem item in Feed.Items)
            {
                RSSItems.Add(new RSSItem(item));
            }
            foreach (SyndicationElementExtension extension in Feed.ElementExtensions)
            {
                XmlElement element = extension.GetObject<XmlElement>();
                switch (element.Name)
                {
                    case "keyword":
                        Keywords.Add(element.InnerText);
                        break;
                    case "imageSource":
                        ImageSource = element.InnerText;
                        break;
                    case "lastUpdate":
                        LastUpdate = Convert.ToDateTime(element.InnerText);
                        break;
                    case "feedUrl":
                        Link = new Uri(element.InnerText);
                        break;
                    default:
                        break;
                }
            }
            foreach (RSSItem item in RSSItems)
            {
                item.ReadItem();
            }
        }

        public static bool TryParseFeed(string Uri)
        {
            try
            {
                SyndicationFeed feed = SyndicationFeed.Load(XmlReader.Create(Uri));
                foreach (SyndicationItem item in feed.Items)
                {
                    Debug.Print(item.Title.Text);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
