using SharedResources;
using SharedResources.ViewModels;
using System;
using System.Diagnostics;
using System.Windows;

namespace RSSReader_Desktop.Views
{
    /// <summary>
    /// Interaction logic for ShowItemWindow.xaml
    /// </summary>
    public partial class ShowItemWindow : Window
    {
        private readonly ShowItemViewModel viewModel;

        public ShowItemWindow(RSSItem item)
        {
            InitializeComponent();
            viewModel = new ShowItemViewModel(item);
            DataContext = viewModel;
            viewModel.OpenInBrowserEvent += OpenInBrowserEvent;
        }

        private void OpenInBrowser_Click(object sender, RoutedEventArgs e)
        {
            viewModel.OpenInBrowser();
        }

        private void OpenInBrowserEvent(Uri uri)
        {
            ProcessStartInfo sInfo = new()
            {
                FileName = uri.AbsoluteUri,
                UseShellExecute = true,
            };
            Process.Start(sInfo);
        }
    }
}
