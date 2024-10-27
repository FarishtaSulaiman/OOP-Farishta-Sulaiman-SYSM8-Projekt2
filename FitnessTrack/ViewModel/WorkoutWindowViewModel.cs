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

        // Konstruktor
        public WorkoutWindowViewModel(UserManager userManager)
        {
            _userManager = userManager;
            OnPropertyChanged(nameof(LoggedInUser));  // Skicka meddelande om att användaren är inloggad
        }
    }
}