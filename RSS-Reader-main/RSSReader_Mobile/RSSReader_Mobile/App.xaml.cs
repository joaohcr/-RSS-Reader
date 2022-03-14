using RSSReader_Mobile.Views;
using SharedResources;
using System.Timers;
using Xamarin.Forms;

namespace RSSReader_Mobile
{
    public partial class App : Application
    {
        public Model Model { get; private set; }

        public App()
        {
            InitializeComponent();
            Model = new Model();
            MainPage = new MainPage();
            Model.Load();
            Timer t = new()
            {
                Interval = 60000,
                AutoReset = true
            };
            t.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            t.Start();
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            Model.UpdateFeeds();
        }

        protected override void OnStart()
        {

        }

        protected override void OnSleep()
        {

        }

        protected override void OnResume()
        {

        }
    }
}
