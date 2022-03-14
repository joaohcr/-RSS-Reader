using SharedResources;
using SharedResources.ViewModels;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RSSReader_Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShowItemPage : ContentPage
    {
        private readonly ShowItemViewModel viewModel;

        public ShowItemPage(RSSItem item)
        {
            InitializeComponent();
            viewModel = new ShowItemViewModel(item);
            BindingContext = viewModel;
            viewModel.OpenInBrowserEvent += OpenInBrowserEvent;
        }

        private void OpenInBrowser_Click(object sender, EventArgs e)
        {
            viewModel.OpenInBrowser();
        }

        private async void OpenInBrowserEvent(Uri uri)
        {
            await Browser.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
        }
    }
}