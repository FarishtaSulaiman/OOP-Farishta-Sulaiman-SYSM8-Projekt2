using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessTrack.Model
{
    public class UserManager
    {
        private List<User> _registeredUsers = new List<User>();

        public UserManager()
        {
            User testUser = new User("testUser", "Test123!", "Sverige", "Vad är ditt favoritdjur?", "hund");
            _registeredUsers.Add(testUser);

            AdminUser adminUser = new AdminUser("adminUser", "Admin123!", "Sverige", "Vad är ditt favoritdjur?", "katt");
            _registeredUsers.Add(adminUser);
        }

        public void AddUser(User user)
        {
            _registeredUsers.Add(user);
        }

        public bool IsUsernameTaken(string username)
        {
            return _registeredUsers.Any(u => u.UserName == username);
        }

        public User? GetUserByCredentials(string username, string password)
        {
            return _registeredUsers.FirstOrDefault(user => user.UserName == username && user.PassWord == password);
        }

        public User? GetUserByUsername(string username)
        {
            return _registeredUsers.FirstOrDefault(user => user.UserName == username);
        }

        public List<User> GetAllUsers()
        {
            return _registeredUsers;
        }

        // skapar en referens för att visa vem som är inloggad till workoutwindow genom signin metoden används den inloggade usern genom currentuser 
        private User _currentUser;
        
        public User CurrentUser
        {
            get => _currentUser;
            set => _currentUser = value;
        }

        // Metod för att logga in användaren och sätta den som aktuell
        public bool SignIn(string username, string password)
        {
            var user = _registeredUsers.FirstOrDefault(u => u.UserName == username && u.PassWord == password);
            if (user != null)
            {
                _currentUser = user;
                return true;
            }
            return false;
        }
    }
}
}
