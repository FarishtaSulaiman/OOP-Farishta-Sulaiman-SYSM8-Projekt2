using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessTrack.Model
{
    public class AdminUser : User // har speciella rättigheter som hanterar alla träningspass
    {
        public AdminUser(string UserName, string Password, string Country, string SecurityQuestion, string SecurityAnswer)
            : base(UserName, Password, Country, SecurityQuestion, SecurityAnswer)
        {

        }
    
    

        public void ManageAllWorukouts()
        {
            // logik för att hantera alla användarnas träningspass 
        }
    }
}
