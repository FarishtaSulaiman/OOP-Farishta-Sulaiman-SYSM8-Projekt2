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

        public User(string userName, string passWord, string country, string securityQuestion, string securityAnswer)
            : base(userName, passWord)
        {
            Country = country;
            SecurityQuestion = securityQuestion;
            SecurityAnswer = securityAnswer;
        }

        public override void SignIn()
        {
            if (!string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(PassWord))
            {
                MessageBox.Show("Inloggning lyckades!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Fel användarnamn eller lösenord.", "Fel", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void ResetPassword(string securityAnswer)
        {
            if (SecurityAnswer.ToLower() == securityAnswer.ToLower())
            {
                MessageBox.Show($"Ditt lösenord är: {PassWord}", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Fel svar på säkerhetsfrågan.", "Fel", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}