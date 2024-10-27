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
        
        // Egenskap för att hålla koll på den inloggade användaren
        public User CurrentUser { get; set; }

        // Metod för att hämta en användare baserat på användarnamn och lösenord
      

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
            return _registeredUsers.Any(u => u.Username == username);
        }

        public User? GetUserByCredentials(string username, string password)
        {
            return _registeredUsers.FirstOrDefault(user => user.Username == username && user.Password == password);
        }

        public User? GetUserByUsername(string username)
        {
            return _registeredUsers.FirstOrDefault(user => user.Username == username);
        }

        public List<User> GetAllUsers()
        {
            return _registeredUsers;
        }

    }
}
