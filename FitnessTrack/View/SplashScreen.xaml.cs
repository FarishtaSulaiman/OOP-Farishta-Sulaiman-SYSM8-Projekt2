using FitnessTrack.Model;
using FitnessTrack.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FitnessTrack.View
{
    /// <summary>
    /// Interaction logic for SplashScreen.xaml
    /// </summary>
    public partial class SplashScreen : Window
    {
        private UserManager _userManager;  

        public SplashScreen()
        {
            InitializeComponent();
            _userManager = new UserManager();  // Skapat en instans av UserManager här
            DataContext = new SplashScreenViewModel(OpenMainWindow);  
        }

        private void OpenMainWindow()
        {
            // Skicka vidare UserManager till MainWindow
            var mainWindow = new MainWindow(_userManager);
            mainWindow.Show();

            // Stäng SplashScreen
            this.Close();
        }
    }
}