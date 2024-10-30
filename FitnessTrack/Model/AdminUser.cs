using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessTrack.Model
{
    public class AdminUser : User
    {
        public AdminUser(string username, string password, string country, string securityQuestion, string securityAnswer)
            : base(username, password, country, securityQuestion, securityAnswer)
        {
        }

        // Denna metod kan hämta alla träningspass om UserManager skickas in
        public List<WorkOut> ManageAllWorkouts(UserManager userManager)
        {
            List<WorkOut> allWorkouts = new List<WorkOut>();
            foreach (var user in userManager.GetAllUsers())
            {
                allWorkouts.AddRange(user.Workouts);
            }
            return allWorkouts;
        }
    }
}