using RSSReader_Mobile.ViewModels;
using SharedResources;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RSSReader_Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NotificationPage : ContentPage
    {
        private readonly NotificationViewModel ViewModel;

        public NotificationPage()
        {
            InitializeComponent();
            ViewModel = BindingContext as NotificationViewModel;
            ViewModel.ShowItemEvent += ShowItemEvent;
        }

        private void SwipeItem_Clicked(object sender, EventArgs e)
        {
            ViewModel.FileNotification((Notification)((SwipeItem)sender).BindingContext);
        }

        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ViewModel.OpenNotification();
        }

        private void ShowItemEvent(RSSItem item)
        {
            Navigation.PushAsync(new ShowItemPage(item));
        }
    }
}
