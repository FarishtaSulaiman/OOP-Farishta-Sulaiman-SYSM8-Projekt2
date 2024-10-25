using FitnessTrack.Model;
using FitnessTrack.MVVM;
using FitnessTrack.ViewModel;
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
        // Konstruktorn som tar emot UserManager
        public SplashScreen(UserManager userManager)
        {
            InitializeComponent();

            // Skicka vidare UserManager till ViewModel
            DataContext = new SplashScreenViewModel(userManager);
        }
    }
}
