using SharedResources.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RSSReader_Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddFeedPage : ContentPage
    {
        private readonly App app;

        private readonly AddFeedViewModel viewModel;

        public AddFeedPage()
        {
            InitializeComponent();
            app = Application.Current as App;
            viewModel = new AddFeedViewModel(app.Model);
            BindingContext = viewModel;
            viewModel.GoBack += GoBack;
            viewModel.ShowError += ShowError;
        }

        private void AddCategory_Click(object sender, EventArgs e)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                string result = await DisplayPromptAsync("Nova categoria", "Qual é a nova categoria?");
                viewModel.AddCategory(result);
            });
        }

        private void AddKeyword_Click(object sender, EventArgs e)
        {
            viewModel.AddKeyword();
        }

        private void AddFeed_Click(object sender, EventArgs e)
        {
            viewModel.AddFeed();
        }

        private async void ShowError(string msg)
        {
            await DisplayAlert(msg, "Erro", "OK");
        }

        private void GoBack()
        {
            Navigation.PopAsync();
        }
    }
}