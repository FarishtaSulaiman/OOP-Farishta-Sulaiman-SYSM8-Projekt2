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
        public string UserName { get; set; }
        public string PassWord { get; set; }

        // Konstruktor
        public Person(string userName, string passWord)
        {
            UserName = userName;
            PassWord = passWord;
        }

        // Abstrakt metod utan implementering, det är underklasserna som måste implementera den
        public abstract void SignIn();
    }
}
