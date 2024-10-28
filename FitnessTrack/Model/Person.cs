using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessTrack.Model
{
    public abstract class Person
    {
        // Property
        public string Username { get; set; }
        public string Password { get; set; }

        // En virtuell workouts-lista som kan skrivas över i User
        public virtual List<WorkOut> Workouts { get; set; } = new List<WorkOut>();

        // Konstruktor
        public Person(string username, string password)
        {
            Username = username;
            Password = password;
        }

        // Abstrakt metod utan implementering, det är underklasserna som måste implementera den
        public abstract void SignIn();
    }
}
