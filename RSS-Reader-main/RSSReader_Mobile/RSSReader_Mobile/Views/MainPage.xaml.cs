using RSSReader_Mobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RSSReader_Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : FlyoutPage
    {
        private readonly App app;

        private StartViewModel StartViewModel { get; }

        public MainPage()
        {
            InitializeComponent();
            app = Application.Current as App;
            StartViewModel = new StartViewModel(app.Model);
            Flyout = new FlyoutView(StartViewModel);
            Detail = new NavigationPage(new StartPage(StartViewModel));
            StartViewModel.ShowFeedEvent += ShowFeedEvent;
        }

        private void ShowFeedEvent()
        {
            IsPresented = false;
        }
    }
}