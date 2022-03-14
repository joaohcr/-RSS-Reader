using SharedResources;
using System.Collections.ObjectModel;

namespace RSSReader_Desktop.ViewModels
{
    public class AddCategoryViewModel
    {
        public event MethodWithoutArg GoBack;
        public event MethodWithString ShowError;

        private ObservableCollection<Category> Categories { get; set; }

        public string Category { get; set; }

        public AddCategoryViewModel(ObservableCollection<Category> categories)
        {
            Categories = categories;
        }

        public void AddCategory()
        {
            if (string.IsNullOrWhiteSpace(Category) == false)
            {
                Categories.Add(new Category(Category));
                GoBack?.Invoke();
            }
            else
                ShowError?.Invoke("Categoria em falta.");
        }
    }
}
