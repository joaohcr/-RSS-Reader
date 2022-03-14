using SharedResources;
using System.Timers;
using System.Windows;

namespace RSSReader_Desktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public Model Model { get; init; }

        public App()
        {
            Model = new Model();
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
            Current.Dispatcher.Invoke(delegate
            {
                Model.UpdateFeeds();
            });
        }
    }
}
