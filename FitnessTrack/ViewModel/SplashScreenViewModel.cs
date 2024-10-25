using FitnessTrack.Model;
using FitnessTrack.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace FitnessTrack.ViewModel
{
    public class SplashScreenViewModel : ViewModelBase
    {
        public ICommand GetStartedCommand { get; }

        private UserManager _userManager;

        // Konstruktor som tar emot UserManager
        public SplashScreenViewModel(UserManager userManager)
        {
            _userManager = userManager;
            GetStartedCommand = new RelayCommand(OpenMainWindow);
        }

        // Öppna MainWindow och stäng SplashScreen
        private void OpenMainWindow(object parameter)
        {
            var mainWindow = new MainWindow(_userManager);
            mainWindow.Show();

            // Stäng nuvarande SplashScreen
            Application.Current.Windows[0]?.Close();
        }
    }
}