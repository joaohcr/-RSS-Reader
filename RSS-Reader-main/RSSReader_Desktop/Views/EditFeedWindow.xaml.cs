using SharedResources.ViewModels;
using System.Windows;

namespace RSSReader_Desktop.Views
{
    /// <summary>
    /// Interaction logic for EditFeedWindow.xaml
    /// </summary>
    public partial class EditFeedWindow : Window
    {
        private readonly App app;

        private readonly EditFeedViewModel viewModel;

        public EditFeedWindow()
        {
            InitializeComponent();
            app = Application.Current as App;
            viewModel = new EditFeedViewModel(app.Model);
            DataContext = viewModel;
            viewModel.GoBack += GoBack;
        }

        private void AddKeyword_Click(object sender, RoutedEventArgs e)
        {
            viewModel.AddKeyword();
        }

        private void DelKeyword_Click(object sender, RoutedEventArgs e)
        {
            viewModel.DelKeyword();
        }

        private void SaveFeed_Click(object sender, RoutedEventArgs e)
        {
            if (viewModel.SelectedFeed != null)
            {
                viewModel.SaveFeed();
            }
        }

        private void DelFeed_Click(object sender, RoutedEventArgs e)
        {
            if (viewModel.SelectedFeed != null)
            {
                if (MessageBox.Show("Tem a certeza que pretende remover o feed?", "Atenção", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                {
                    viewModel.DelFeed();
                }
            }
        }

        private void GoBack()
        {
            Close();
        }
    }
}
