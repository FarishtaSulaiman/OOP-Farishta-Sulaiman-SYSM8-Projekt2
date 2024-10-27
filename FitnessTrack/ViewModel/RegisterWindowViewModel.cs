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
        private  Window _registerWindow;  // Lägg till en referens till själva fönstret

        public string Username { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }  
        public ObservableCollection<string> Countries { get; set; }
        public string SelectedCountry { get; set; }
        public string SecurityAnswer { get; set; }

        public ICommand RegisterCommand { get; }

        // Konstruktor som tar emot både UserManager och fönstret
        public RegisterWindowViewModel(UserManager userManager, Window registerWindow)
        {
            _userManager = userManager;
            _registerWindow = registerWindow;  // Spara referens till fönstret
            RegisterCommand = new RelayCommand(Register);

            // Initiera länder 
            Countries = new ObservableCollection<string> { "Sverige", "Norge", "Danmark", "Finland" };
        }

        public void Register(object parameter)
        {
            // Kontrollera om användarnamnet redan är taget
            if (_userManager.IsUsernameTaken(Username))
            {
                MessageBox.Show("Användarnamnet är redan upptaget!", "Fel", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Kontrollera om lösenordet matchar bekräftelselösenordet
            if (Password != ConfirmPassword)
            {
                MessageBox.Show("Lösenorden matchar inte!", "Fel", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Kontrollera om lösenordet uppfyller kraven
            if (!_userManager.IsPasswordValid(Password))
            {
                MessageBox.Show("Lösenordet måste vara minst 8 tecken långt och innehålla minst en siffra samt ett specialtecken.", "Fel", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            // Skapa ny användare
            User newUser = new User(Username, Password, SelectedCountry, "Vad är ditt favoritdjur?", SecurityAnswer);
            _userManager.AddUser(newUser);

            MessageBox.Show($"Användaren {Username} har registrerats framgångsrikt!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);


            // Öppna MainWindow igen och skicka med UserManager
            var mainWindow = new MainWindow(_userManager);
            mainWindow.Show();

            // Stäng RegisterWindow
            _registerWindow?.Close(); 

           
        }
    }
}