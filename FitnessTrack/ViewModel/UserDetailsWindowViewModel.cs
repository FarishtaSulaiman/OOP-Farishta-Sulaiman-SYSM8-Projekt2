using FitnessTrack.Model;
using FitnessTrack.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessTrack.ViewModel
{
    public class UserDetailsWindowViewModel : ViewModelBase
    {
        private readonly UserManager _userManager;

        // Egenskap för användardetaljer
        public User LoggedInUser => _userManager.CurrentUser;

        // Konstruktor som tar emot en UserManager
        public UserDetailsWindowViewModel(UserManager userManager)
        {
            _userManager = userManager;
        }
        
    }
}