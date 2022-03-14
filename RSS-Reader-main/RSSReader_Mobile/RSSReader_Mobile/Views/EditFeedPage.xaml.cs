using SharedResources.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RSSReader_Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditFeedPage : ContentPage
    {
        private readonly App app;

        private readonly EditFeedViewModel viewModel;

        public EditFeedPage()
        {
            InitializeComponent();
            app = Application.Current as App;
            viewModel = new EditFeedViewModel(app.Model);
            BindingContext = viewModel;
            viewModel.GoBack += GoBack;
        }

        private void AddKeyword_Click(object sender, EventArgs e)
        {
            viewModel.AddKeyword();
        }

        private void DelKeyword_Click(object sender, EventArgs e)
        {
            viewModel.DelKeyword();
        }

        private void SaveFeed_Click(object sender, EventArgs e)
        {
            viewModel.SaveFeed();
        }

        private void DelFeed_Click(object sender, EventArgs e)
        {
            viewModel.DelFeed();
        }

        private void GoBack()
        {
            Navigation.PopAsync();
        }
    }
}