using RSSReader_Desktop.ViewModels;
using System;
using System.Windows;

namespace RSSReader_Desktop.Views
{
    /// <summary>
    /// Interaction logic for AddCategoryWindow.xaml
    /// </summary>
    public partial class AddCategoryWindow : Window
    {
        private readonly App app;

        private readonly AddCategoryViewModel viewModel;

        public AddCategoryWindow()
        {
            InitializeComponent();
            app = Application.Current as App;
            viewModel = new AddCategoryViewModel(app.Model.Categories);
            DataContext = viewModel;
            viewModel.ShowError += ShowError;
            viewModel.GoBack += GoBack;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
                viewModel.AddCategory();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void GoBack()
        {
            Close();
        }

        private void ShowError(string msg)
        {
            MessageBox.Show(msg, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
