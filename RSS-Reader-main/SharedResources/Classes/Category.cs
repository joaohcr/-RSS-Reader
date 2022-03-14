using System.Collections.ObjectModel;
using System.IO;

namespace SharedResources
{
    public class Category
    {
        public string Name { get; private set; }

        public ObservableCollection<RSSFeed> Feeds { get; set; }

        public Category()
        {
            Feeds = new ObservableCollection<RSSFeed>();
        }

        public Category(string name) : this()
        {
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }

        public void SaveCategory()
        {
            string path = $"{Model.AppFolder}/{Model.MakeValidFileName(Name)}";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            StreamWriter writer = new($"{path}/name.txt");
            writer.WriteLine(Name);
            writer.Close();
            foreach (RSSFeed feed in Feeds)
            {
                feed.SaveFeed();
            }
        }

        public void ReadCategory(string dir)
        {
            StreamReader reader = new($"{dir}/name.txt");
            Name = reader.ReadLine();
            reader.Close();
            foreach (string filePath in Directory.GetFiles(dir, "*.xml"))
            {
                RSSFeed feed = new();
                feed.ReadFeed(filePath);
                Feeds.Add(feed);
            }
        }
    }
}
