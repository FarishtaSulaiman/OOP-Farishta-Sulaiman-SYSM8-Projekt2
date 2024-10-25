using FitnessTrack.Model;
using System.Configuration;
using System.Data;
using System.Windows;

namespace FitnessTrack
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private UserManager _userManager;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Skapa UserManager
            _userManager = new UserManager();

            // Öppna SplashScreen och skicka UserManager till ViewModel
            var splashScreen = new View.SplashScreen(_userManager);
            splashScreen.Show();
        }
    }
}