using RSSReader_Mobile.ViewModels;
using SharedResources;
using System;
using Xamarin.Forms;

namespace RSSReader_Mobile.Views
{
    public partial class StartPage : ContentPage
    {
        private StartViewModel ViewModel { get; }

        public StartPage(StartViewModel startViewModel)
        {
            InitializeComponent();
            ViewModel = startViewModel;
            BindingContext = ViewModel;
            ViewModel.ShowFeedEvent += ViewModel_ShowFeedEvent;
            ViewModel.ShowItemEvent += ViewModel_ShowItemEvent;
            ViewModel.UpdFeedEvent += ViewModel_ShowFeedEvent;
        }

        private void AddFeed_Click(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AddFeedPage());
        }

        private void EditFeed_Click(object sender, EventArgs e)
        {
            Navigation.PushAsync(new EditFeedPage());
        }

        private void ShowItem_Click(object sender, ItemTappedEventArgs e)
        {
            ViewModel.ShowItem();
        }

        private void ShowNotif_Click(object sender, EventArgs e)
        {
            Navigation.PushAsync(new NotificationPage());
        }

        private void Refresh_Click(object sender, EventArgs e)
        {
            ViewModel.UpdateFeed();
            ViewModel_ShowFeedEvent();
        }

        private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            ViewModel_ShowFeedEvent();
        }

        public void ViewModel_ShowFeedEvent()
        {
            switch (ViewModel.NotRead)
            {
                case false:
                    LvItems.ItemsSource = ViewModel.SelectedFeed?.RSSItems;
                    break;
                case true:
                    LvItems.ItemsSource = ViewModel.UnreadItems;
                    break;
            }
        }

        private void ViewModel_ShowItemEvent(RSSItem item)
        {
            Navigation.PushAsync(new ShowItemPage(item));
        }
    }
}
