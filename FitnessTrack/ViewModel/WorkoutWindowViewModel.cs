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
        private readonly UserManager _userManager;

        // Egenskap för att visa den inloggade användaren som Person
        public Person CurrentPerson => _userManager.CurrentPerson;

        // Kommandon för att öppna andra fönster
        public ICommand OpenUserDetailsCommand { get; }
        public ICommand OpenAddWorkoutWindowCommand { get; }

        // Konstruktor
        public WorkoutWindowViewModel(UserManager userManager)
        {
            _userManager = userManager;
            OnPropertyChanged(nameof(CurrentPerson));

            // Initiera kommandona
            OpenUserDetailsCommand = new RelayCommand(OpenUserDetails);
            OpenAddWorkoutWindowCommand = new RelayCommand(OpenAddWorkoutWindow);
        }

        // Metod för att öppna UserDetailsWindow
        private void OpenUserDetails(object parameter)
        {
            var userDetailsWindow = new UserDetailsWindow(_userManager); // Skapa och visa UserDetailsWindow
            userDetailsWindow.Show();
        }

        // Metod för att öppna AddWorkoutWindow
        private void OpenAddWorkoutWindow(object parameter)
        {
            var addWorkoutWindow = new AddWorkoutWindow(_userManager); // Skapa och visa AddWorkoutWindow
            addWorkoutWindow.ShowDialog();
        }
    }
}