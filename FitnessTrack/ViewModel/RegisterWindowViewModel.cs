using FitnessTrack.Model;
using FitnessTrack.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using FitnessTrack.View;

namespace FitnessTrack.ViewModel
{
    public class RegisterWindowViewModel : ViewModelBase
    {
        private readonly UserManager _userManager;
        private readonly Window _registerWindow; //  fönsterreferens för att kunna stänga fönstret

        public string Username { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; } // Ny variabel för bekräftelselösenord
        public ObservableCollection<string> Countries { get; set; }
        public string SelectedCountry { get; set; }
        public string SecurityAnswer { get; set; }

        public ICommand RegisterCommand { get; }

        // Konstruktorn tar emot UserManager och RegisterWindow
        public RegisterWindowViewModel(UserManager userManager, Window registerWindow)
        {
            _userManager = userManager;
            _registerWindow = registerWindow; // Referens till det aktuella RegisterWindow

            Countries = new ObservableCollection<string> { "Sverige", "Norge", "Danmark", "Finland" };
            RegisterCommand = new RelayCommand(Register);
        }

        private void Register(object parameter)
        {
            // Kontrollera att lösenordet uppfyller kraven
            if (Password.Length < 8 || !Password.Any(char.IsUpper) || !Password.Any(char.IsDigit) || !Password.Any(ch => !char.IsLetterOrDigit(ch)))
            {
                MessageBox.Show("Lösenordet måste vara minst 8 tecken långt, innehålla en stor bokstav, en siffra och ett specialtecken.", "Fel", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Kontrollera att lösenorden matchar
            if (Password != ConfirmPassword)
            {
                MessageBox.Show("Lösenorden matchar inte!", "Fel", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Kontrollera om användarnamnet redan finns
            if (_userManager.IsUsernameTaken(Username))
            {
                MessageBox.Show("Användarnamnet är redan upptaget!", "Fel", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Skapa ny användare
            User newUser = new User(Username, Password, SelectedCountry, "Vad är ditt favoritdjur?", SecurityAnswer);
            _userManager.AddUser(newUser);

            MessageBox.Show($"Användaren {Username} har registrerats framgångsrikt!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);

            // Stäng RegisterWindow och öppna MainWindow igen
            _registerWindow?.Close(); // Stäng RegisterWindow

            var mainWindow = new MainWindow(_userManager); // Öppna MainWindow
            mainWindow.Show(); // Visa MainWindow efter att ha stängt RegisterWindow
        }
    }
}