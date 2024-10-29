using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FitnessTrack.Model
{
    public class User : Person
    {
        public string Country { get; set; }
        public string SecurityQuestion { get; set; }
        public string SecurityAnswer { get; set; }


        // Behåll Workouts specifikt för User
        public override List<WorkOut> Workouts { get; set; } = new List<WorkOut>();
        public User(string username, string password, string country, string securityQuestion, string securityAnswer)
            : base(username, password)
        {
            Country = country;
            SecurityQuestion = securityQuestion;
            SecurityAnswer = securityAnswer;
        }

        public override void SignIn()
        {
           
        }

        public string ResetPassword(string securityAnswer)
        {
            // Returnera lösenordet om svaret är korrekt
            return securityAnswer == SecurityAnswer ? Password : string.Empty;

        }

    }
}