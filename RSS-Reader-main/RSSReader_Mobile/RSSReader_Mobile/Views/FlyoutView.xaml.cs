using RSSReader_Mobile.ViewModels;
using SharedResources;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RSSReader_Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FlyoutView : ContentPage
    {
        private StartViewModel ViewModel { get; }

        public FlyoutView(StartViewModel startViewModel)
        {
            InitializeComponent();
            ViewModel = startViewModel;
            BindingContext = ViewModel;
            ViewModel.ShowError += ShowError;
        }

        private void ShowFeed_Click(object sender, EventArgs e)
        {
            RSSFeed feed = (sender as Button).BindingContext as RSSFeed;
            ViewModel.ShowFeed(feed);
        }

        private void AddCategory_Click(object sender, EventArgs e)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                string result = await DisplayPromptAsync("Nova categoria", "Qual é a nova categoria?");
                ViewModel.AddCategory(result);
            });
        }

        private async void ShowError(string msg)
        {
            await DisplayAlert("Erro", msg, "OK");
        }
    }
}