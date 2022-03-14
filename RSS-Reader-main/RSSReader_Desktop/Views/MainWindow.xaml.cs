using SharedResources;
using SharedResources.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace RSSReader_Desktop.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly App app;

        private readonly MainViewModel viewModel;

        public MainWindow()
        {
            InitializeComponent();
            app = Application.Current as App;
            viewModel = new MainViewModel(app.Model);
            DataContext = viewModel;
            viewModel.ShowItemEvent += ShowItemEvent;
        }

        private void AddFeed_Click(object sender, RoutedEventArgs e)
        {
            AddFeedWindow window = new();
            window.ShowDialog();
        }

        private void EditFeed_Click(object sender, RoutedEventArgs e)
        {
            EditFeedWindow window = new();
            window.ShowDialog();
        }

        private void BtSair_Click(object sender, RoutedEventArgs e)
        {
            app.Model.Save();
            Close();
        }

        private void AddCategory_Click(object sender, RoutedEventArgs e)
        {
            AddCategoryWindow window = new();
            window.ShowDialog();
        }

        private void ShowFeed_Click(object sender, RoutedEventArgs e)
        {
            if (((TreeView)sender).SelectedItem is RSSFeed feed)
            {
                viewModel.ShowFeed(feed);
            }
        }

        private void ShowItem_Click(object sender, MouseButtonEventArgs e)
        {
            viewModel.ShowItem();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            app.Model.Save();
        }

        private void NotificationClicked(object sender, RoutedEventArgs e)
        {
            Notification notification = (Notification)((MenuItem)sender).Header;
            viewModel.OpenNotification(notification);
            NotificationMenu.Items.Refresh();
        }

        private void UpdateFeed_Click(object sender, RoutedEventArgs e)
        {
            viewModel.UpdateFeed();
            LbItems.Items.Refresh();
        }

        private void NotifFiling_Click(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).DataContext is Notification notification)
            {
                notification.IsFiled = true;
                NotificationMenu.Items.Refresh();
            }
        }

        private void ShowItemEvent(RSSItem item)
        {
            ShowItemWindow window = new(item);
            window.ShowDialog();
            LbItems.Items.Refresh();
        }
    }
}
