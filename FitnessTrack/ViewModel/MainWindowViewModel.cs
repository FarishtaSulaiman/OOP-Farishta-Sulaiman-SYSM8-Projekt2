using FitnessTrack.Model;
using FitnessTrack.MVVM;
using FitnessTrack.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;


namespace FitnessTrack.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        UserManager userManager;  // För att hantera användare

        // Egenskaper för användarnamn och lösenord
        public string Username { get; set; }
        public string Password { get; set; }

        // Kommandon för knapparna
        public ICommand SignInCommand { get; }
        public ICommand ForgotPasswordCommand { get; }
        public ICommand RegisterCommand { get; }

        // Konstruktor som tar emot en UserManager-instans
        public MainWindowViewModel(UserManager userManager)
        {
            this.userManager = userManager;

            // Initialisera kommandon och koppla till funktioner
            SignInCommand = new RelayCommand(SignIn);
            ForgotPasswordCommand = new RelayCommand(ForgotPassword);
            RegisterCommand = new RelayCommand(Register);
        }

        // Logik för Sign In-knappen
        private void SignIn(object parameter)
        {
            // Kontrollera om användarnamn och lösenord stämmer
            var user = userManager.GetUserByCredentials(Username, Password);

            if (user != null && user.PassWord == Password)
            {
                // Om inloggningen lyckas, öppna WorkoutWindow
                var workoutsWindow = new WorkoutWindow(user);
                workoutsWindow.Show();

                // Stäng MainWindow
                Application.Current.Windows[0]?.Close();
            }
            else
            {
                // Visa felmeddelande om inloggningen misslyckas
                MessageBox.Show("Fel användarnamn eller lösenord.", "Fel", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Logik för Forgot Password-knappen
        private void ForgotPassword(object parameter)
        {
            var user = userManager.GetUserByUsername(Username);

            if (user != null)
            {
                // Säkerhetsfråga för att återställa lösenord
                string answer = Microsoft.VisualBasic.Interaction.InputBox(user.SecurityQuestion, "Säkerhetsfråga");
                user.ResetPassword(answer);
            }
            else
            {
                MessageBox.Show("Användarnamnet existerar inte.", "Fel", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Logik för Register-knappen
        private void Register(object parameter)
        {
            // Öppna RegisterWindow och skicka vidare UserManager
            var registerWindow = new RegisterWindow(userManager);
            registerWindow.Show();

            // Stäng MainWindow efter att ha öppnat RegisterWindow
            Application.Current.Windows[0]?.Close();
        }
    }
}