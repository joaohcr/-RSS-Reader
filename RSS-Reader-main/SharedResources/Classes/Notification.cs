using System;
using System.Xml.Linq;

namespace SharedResources
{
    public class Notification
    {
        public static event MethodWithItemReturned FindItemEvent;

        public string Message { get; private set; }

        public RSSItem RSSItem { get; set; }

        public bool IsRead { get; set; }

        public bool IsFiled { get; set; }

        public Notification()
        {
        }

        public Notification(RSSItem item)
        {
            RSSItem = item;
            Message = RSSItem.Item.Title.Text;
            IsRead = false;
            IsFiled = false;
        }

        public XElement SaveNotification()
        {
            return new XElement("notification",
                    new XElement("message", Message),
                    new XElement("link", RSSItem.Link),
                    new XElement("isRead", IsRead),
                    new XElement("isFiled", IsFiled));
        }

        public void ReadNotification(XElement element)
        {
            Message = element.Element("message").Value;
            RSSItem = FindItemEvent?.Invoke(element.Element("link").Value);
            IsRead = Convert.ToBoolean(element.Element("isRead").Value);
            IsFiled = Convert.ToBoolean(element.Element("isFiled").Value);
        }
    }
}
