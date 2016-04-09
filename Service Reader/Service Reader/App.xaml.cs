using System.Windows;

namespace Service_Reader
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            ApplicationWindow app = new ApplicationWindow();
            ApplicationViewModel context = new ApplicationViewModel();
            app.DataContext = context;
            app.Show();
        }
    }
}
