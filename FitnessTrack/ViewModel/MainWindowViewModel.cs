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
            SignInCommand = new RelayCommand(SignIn);
            ForgotPasswordCommand = new RelayCommand(ForgotPassword);
            RegisterCommand = new RelayCommand(Register);
        }

        // Logik för Sign In-knappen
        private void SignIn(object parameter)
        {
            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
            {
                MessageBox.Show("Fyll i både användarnamn och lösenord.", "Varning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var person = _userManager.GetPersonByCredentials(Username, Password);

            if (person != null)
            {
                _userManager.CurrentPerson = person;

                // Generera en 6-siffrig slumpmässig kod för 2FA
                string generatedCode = new Random().Next(100000, 999999).ToString();

                // Öppna VerificationWindow med den genererade koden
                var verificationWindow = new VerificationWindow
                {
                    DataContext = new VerificationWindowViewModel(generatedCode, () => CompleteLogin())
                };
                verificationWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("Fel användarnamn eller lösenord.", "Fel", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Metod för att slutföra inloggning efter att 2FA-koden verifierats
        private void CompleteLogin()
        {
            var workoutsWindow = new WorkoutWindow(_userManager);
            workoutsWindow.Show();
            CloseCurrentWindow();
        }

        // Logik för ForgotPassword-knappen med integrerad funktion för resetpassword 
        private void ForgotPassword(object parameter)
        {
            // Hämta personen (kan vara User eller AdminUser) baserat på användarnamnet
            var person = _userManager.GetPersonByUsername(Username);

            // Kontrollera om personen hittades och har en säkerhetsfråga
            if (person is User user)
            {
                // Visa säkerhetsfrågan och be om svar
                string answer = Microsoft.VisualBasic.Interaction.InputBox(user.SecurityQuestion, "Säkerhetsfråga");

                // Återställ lösenordet om svaret är korrekt
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
                MessageBox.Show("Användaren existerar inte eller har inte behörighet att återställa lösenordet.", "Fel", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Logik för Register-knappen
        private void Register(object parameter)
        {
            // Öppna RegisterWindow och skicka vidare UserManager
            var registerWindow = new RegisterWindow(_userManager);
            registerWindow.Show();

            // Stäng MainWindow efter att ha öppnat RegisterWindow
            CloseCurrentWindow();
        }

        // Hjälpmetod för att stänga MainWindow
        private void CloseCurrentWindow()
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window is MainWindow)
                {
                    window.Close();
                    break;
                }
            }
        }
    }
}