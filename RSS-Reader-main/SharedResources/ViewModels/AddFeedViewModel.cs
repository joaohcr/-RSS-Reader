using System.Collections.ObjectModel;

namespace SharedResources.ViewModels
{
    public class AddFeedViewModel : BaseViewModel
    {
        public event MethodWithoutArg GoBack;
        public event MethodWithString ShowError;

        // Properties
        public Model Model { get; set; }

        public string FeedUri { get; set; }

        public Category SelectedCategory { get; set; }

        public ObservableCollection<string> Keywords { get; set; }

        public string Keyword { get; set; }

        // Constructor
        public AddFeedViewModel(Model model)
        {
            Model = model;
            Keywords = new ObservableCollection<string>();
        }

        // Methods
        public void AddFeed()
        {
            if (!string.IsNullOrWhiteSpace(FeedUri))
            {
                if (SelectedCategory != null)
                {
                    if (Model.IsConnectedToInternet())
                    {
                        if (RSSFeed.TryParseFeed(FeedUri))
                        {
                            string path = $"{Model.AppFolder}/{Model.MakeValidFileName(SelectedCategory.Name)}";
                            RSSFeed feed = new(FeedUri, Keywords, path);
                            SelectedCategory.Feeds.Add(feed);
                            Model.SendNotification(feed);
                            GoBack?.Invoke();
                        }
                        else
                            ShowError?.Invoke("Url inválido.");
                    }
                    else
                        ShowError?.Invoke("Está offline");
                }
                else
                    ShowError?.Invoke("Categoria em falta.");
            }
            else
                ShowError?.Invoke("Url em falta.");
        }

        public void AddCategory(string name)
        {
            if (string.IsNullOrWhiteSpace(name) == false)
                Model.Categories.Add(new Category(name));
            else
                ShowError?.Invoke("Categoria em falta.");
        }

        public void AddKeyword()
        {
            if (string.IsNullOrWhiteSpace(Keyword) == false)
                Keywords.Add(Keyword);
            else
                ShowError?.Invoke("Palavra-chave em falta.");
        }
    }
}
