using FitnessTrack.Model;
using FitnessTrack.MVVM;
using FitnessTrack.View;
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
    public class UserDetailsWindowViewModel : ViewModelBase
    {
        private readonly UserManager _userManager;
        public string CurrentUsername { get; }
        public ObservableCollection<string> Countries { get; }

        private string _newUsername;
        public string NewUsername
        {
            get => _newUsername;
            set
            {
                _newUsername = value;
                OnPropertyChanged(nameof(NewUsername));
            }
        }

        private string _newCountry;
        public string NewCountry
        {
            get => _newCountry;
            set
            {
                _newCountry = value;
                OnPropertyChanged(nameof(NewCountry));
            }
        }

        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public UserDetailsWindowViewModel(UserManager userManager)
        {
            _userManager = userManager;
            CurrentUsername = userManager.CurrentPerson.Username;
            Countries = new ObservableCollection<string> { "Sverige", "Norge", "Danmark", "Finland" };
            NewCountry = (userManager.CurrentPerson as User)?.Country;

            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);
        }

        private void Save(object parameter)
        {
            // Validera användarnamn
            if (string.IsNullOrWhiteSpace(NewUsername) || NewUsername.Length < 3)
            {
                MessageBox.Show("Användarnamnet måste vara minst 3 tecken långt.", "Fel", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (_userManager.IsUsernameTaken(NewUsername) && NewUsername != CurrentUsername)
            {
                MessageBox.Show("Användarnamnet är redan upptaget.", "Fel", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Validera lösenord
            if (!string.IsNullOrWhiteSpace(NewPassword) && NewPassword.Length < 5)
            {
                MessageBox.Show("Lösenordet måste vara minst 5 tecken långt.", "Fel", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (NewPassword != ConfirmPassword)
            {
                MessageBox.Show("Lösenorden matchar inte.", "Fel", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Uppdatera användaruppgifter
            var currentUser = _userManager.CurrentPerson as User;
            if (currentUser != null)
            {
                currentUser.Username = NewUsername;
                currentUser.Country = NewCountry;

                if (!string.IsNullOrWhiteSpace(NewPassword))
                    currentUser.Password = NewPassword;

                MessageBox.Show("Användaruppgifterna har sparats.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            // Stäng UserDetailsWindow och återgå till WorkoutsWindow
            CloseUserDetailsWindow();
        }

        private void Cancel(object parameter)
        {
            CloseUserDetailsWindow();
        }

        private void CloseUserDetailsWindow()
        {
            Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w is UserDetailsWindow)?.Close();
        }
    }
}