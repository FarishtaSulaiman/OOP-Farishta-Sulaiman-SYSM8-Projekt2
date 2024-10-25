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

namespace FitnessTrack.ViewModel
{
    public class RegisterWindowViewModel : ViewModelBase
    {
        private readonly UserManager _userManager;

        public string Username { get; set; }
        public string Password { get; set; }
        public ObservableCollection<string> Countries { get; set; }
        public string SelectedCountry { get; set; }
        public string SecurityAnswer { get; set; }

        public ICommand RegisterCommand { get; }

        public RegisterWindowViewModel(UserManager userManager)
        {
            _userManager = userManager;
            RegisterCommand = new RelayCommand(Register);
        }

        public void Register(object parameter)
        {
            if (_userManager.IsUsernameTaken(Username))
            {
                MessageBox.Show("Användarnamnet är redan upptaget!", "Fel", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            User newUser = new User(Username, Password, SelectedCountry, "Vad är ditt favoritdjur?", SecurityAnswer);
            _userManager.AddUser(newUser);

            MessageBox.Show($"Användaren {Username} har registrerats framgångsrikt!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}