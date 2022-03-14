using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceModel.Syndication;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using System.Xml.Linq;

namespace SharedResources
{
    public class RSSItem
    {
        public SyndicationItem Item { get; private set; }

        public string Title => Item.Title?.Text;
        public string Summary => Item.Summary?.Text;
        public Uri Link => Item.Links[0].Uri;
        public DateTime PubDate => Item.PublishDate.DateTime;

        public string ImageSource { get; set; }

        public bool IsRead { get; set; }

        public RSSItem(SyndicationItem item)
        {
            Item = item;  
        }

        public RSSItem(SyndicationItem item, string path)
        {
            Item = item;
            string url = null;
            if (Item.Links.Count >= 2)
            {
                url = Item.Links[1].Uri.AbsoluteUri;
            }
            else if (Summary != null)
            {
                XElement xElement = XElement.Parse($"<Root>{Summary}</Root>");
                List<XElement> elements = new(xElement.Descendants("img"));
                if (elements.Count > 0)
                {
                    url = elements[0].Attribute("src")?.Value;
                }
            }
            if (url != null)
            {
                string imgfile = $"{path}/{Model.MakeValidFileName(Link.AbsoluteUri)}.jpg";
                if (Model.DownloadImageFromUrl(url, imgfile))
                {
                    ImageSource = imgfile;
                }
            }
            CleanEntries();
            IsRead = false;
        }

        public void SaveItem()
        {
            Item.ElementExtensions.Clear();
            Item.ElementExtensions.Add(new XElement("imageSource", ImageSource).CreateReader());
            Item.ElementExtensions.Add(new XElement("isRead", IsRead).CreateReader());
        }

        public void ReadItem()
        {
            foreach (SyndicationElementExtension extension in Item.ElementExtensions)
            {
                XmlElement element = extension.GetObject<XmlElement>();
                switch (element.Name)
                {
                    case "isRead":
                        IsRead = Convert.ToBoolean(element.InnerText);
                        break;
                    case "imageSource":
                        ImageSource = element.InnerText;
                        break;
                    default:
                        break;
                }
            }
        }

        private void CleanEntries()
        {
            if (Summary != null)
            {
                string summary = Regex.Replace(Summary, "<.*?/.*?>", "");
                summary = summary.Trim();
                summary = HttpUtility.HtmlDecode(summary);
                Item.Summary = new(summary);
            }
        }

        public void DelContent()
        {
            if (ImageSource != null && File.Exists(ImageSource))
            {
                File.Delete(ImageSource);
            }
        }
    }
}