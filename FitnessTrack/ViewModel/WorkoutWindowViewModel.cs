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
    public class WorkoutWindowViewModel : ViewModelBase
    {
        private UserManager _userManager;

        // Egenskap för att exponera den inloggade användaren
        public User LoggedInUser => _userManager.CurrentUser;

        // Kommando för att öppna UserDetailsWindow
        public ICommand OpenUserDetailsCommand { get; }

        // Konstruktor
        public WorkoutWindowViewModel(UserManager userManager)
        {
            _userManager = userManager;
            OnPropertyChanged(nameof(LoggedInUser));
            // Initiera kommandot och koppla till metoden OpenUserDetails
            OpenUserDetailsCommand = new RelayCommand(OpenUserDetails);
        }

        // Metod för att öppna UserDetailsWindow
        private void OpenUserDetails(object parameter)
        {
            var userDetailsWindow = new UserDetailsWindow(_userManager); // Skapa en ny instans av UserDetailsWindow och skicka vidare UserManager
            userDetailsWindow.Show(); // Visa fönstret
        }
    }
}