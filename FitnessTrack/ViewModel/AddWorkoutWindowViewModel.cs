using FitnessTrack.Model;
using FitnessTrack.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessTrack.ViewModel
{
    public class AddWorkoutWindowViewModel : ViewModelBase
    {
        private readonly UserManager _userManager;

        // Egenskaper 
        public string WorkoutType { get; set; }
        public DateTime WorkoutDate { get; set; } = DateTime.Now;
        public TimeSpan Duration { get; set; }

        // Konstruktor som tar UserManager som parameter
        public AddWorkoutWindowViewModel(UserManager userManager)
        {
            _userManager = userManager;
        }
    }
}

