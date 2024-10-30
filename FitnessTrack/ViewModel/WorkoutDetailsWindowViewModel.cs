using FitnessTrack.Model;
using FitnessTrack.MVVM;
using FitnessTrack.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FitnessTrack.ViewModel
{
    public class WorkoutDetailsWindowViewModel : ViewModelBase
    {
        private UserManager userManager;
        private WorkOut workout;

        public WorkoutDetailsWindowViewModel(UserManager userManager, WorkOut workout)
        {
            this.userManager = userManager;
            this.workout = workout;
        }
    }
}