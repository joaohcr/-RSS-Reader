using SharedResources.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace RSSReader_Desktop.Views
{
    /// <summary>
    /// Interaction logic for AddFeedWindow.xaml
    /// </summary>
    public partial class AddFeedWindow : Window
    {
        private readonly App app;

        private readonly AddFeedViewModel viewModel;

        public AddFeedWindow()
        {
            InitializeComponent();
            app = Application.Current as App;
            viewModel = new AddFeedViewModel(app.Model);
            DataContext = viewModel;
            viewModel.GoBack += GoBack;
            viewModel.ShowError += ShowError;
        }

        private void AddCategory_Click(object sender, RoutedEventArgs e)
        {
            AddCategoryWindow window = new();
            window.ShowDialog();
        }

        private void AddKeyword_Click(object sender, RoutedEventArgs e)
        {
            viewModel.AddKeyword();
        }

        private void BtRemover_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Keywords.Remove((sender as MenuItem).DataContext as string);
        }

        private void AddFeed_Click(object sender, RoutedEventArgs e)
        {
            viewModel.AddFeed();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ShowError(string msg)
        {
            MessageBox.Show(msg, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void GoBack()
        {
            Close();
        }
    }
}
