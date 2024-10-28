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
        private readonly UserManager _userManager;  // För att hantera användare

        // Egenskaper för användarnamn och lösenord
        public string? Username { get; set; }
        public string? Password { get; set; }

        // Kommandon för knapparna
        public ICommand SignInCommand { get; }
        public ICommand ForgotPasswordCommand { get; }
        public ICommand RegisterCommand { get; }

        // Konstruktor som tar emot en UserManager-instans
        public MainWindowViewModel(UserManager userManager)
        {
            _userManager = userManager;

            // Initialisera kommandon och koppla till funktioner
            SignInCommand = new RelayCommand(SignIn);
            ForgotPasswordCommand = new RelayCommand(ForgotPassword);
            RegisterCommand = new RelayCommand(Register);
        }

        // Logik för Sign In-knappen
        private void SignIn(object parameter)
        {
            // Kontrollera om användarnamn och lösenord är ifyllda
            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
            {
                MessageBox.Show("Fyll i både användarnamn och lösenord.", "Varning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Använd GetPersonByCredentials för att verifiera om användarnamn och lösenord stämmer
            var person = _userManager.GetPersonByCredentials(Username, Password);

            if (person != null)
            {
                // Sätt inloggad person i UserManager
                _userManager.CurrentPerson = person;

                // Öppna WorkoutWindow och skicka med UserManager
                var workoutsWindow = new WorkoutWindow(_userManager);
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

        // Logik för ForgotPassword-knappen
        private void ForgotPassword(object parameter)
        {
            var user = _userManager.GetUserByUsername(Username);

            if (user != null)
            {
                string answer = Microsoft.VisualBasic.Interaction.InputBox(user.SecurityQuestion, "Säkerhetsfråga");
                string password = user.ResetPassword(answer);

                if (!string.IsNullOrEmpty(password))
                {
                    MessageBox.Show($"Ditt lösenord är: {password}", "Lösenord Återställning", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Felaktigt svar på säkerhetsfrågan.", "Fel", MessageBoxButton.OK, MessageBoxImage.Error);
                }
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
            var registerWindow = new RegisterWindow(_userManager);
            registerWindow.Show();

            // Stäng MainWindow efter att ha öppnat RegisterWindow
            Application.Current.Windows[0]?.Close();
        }
    }
}