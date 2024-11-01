using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FitnessTrack.Model
{
    public class UserManager
    {
        private List<Person> _registeredPersons = new List<Person>(); // Lista för registrerade personer

        // Egenskap för att hålla koll på den inloggade personen (kan vara både User och AdminUser)
        public Person CurrentPerson { get; set; }

        public UserManager()
        {
            // Skapa och lägga till en Admin-användare och en vanlig användare i programmet från start
            AdminUser adminUser = new AdminUser("admin", "password", "Sverige", "Vad är ditt favoritdjur?", "katt");
            User defaultUser = new User("user", "password", "Sverige", "Vad är ditt favoritdjur?", "hund");


            // Lägg till träningspass för defaultUser
            defaultUser.Workouts.Add(new CardioWorkout(DateTime.Now, "Cardio", TimeSpan.FromMinutes(30), 300, "Bra träning!", 5));
            defaultUser.Workouts.Add(new StrengthWorkout(DateTime.Now, "Strength", TimeSpan.FromMinutes(45), 500, "Styrka träning!", 15));

            // Lägg till dem i listan över registrerade personer
            _registeredPersons.Add(defaultUser);
            _registeredPersons.Add(adminUser);
        }

        // Metod för att validera lösenordet
        public bool IsPasswordValid(string password)
        {
            // Om du vill tillåta "password" som giltigt lösenord kan du anpassa villkoren
            return !string.IsNullOrEmpty(password); // Tillåt alla lösenord som inte är tomma
        }

        public void AddPerson(Person person)
        {
            _registeredPersons.Add(person);
        }

        public bool IsUsernameTaken(string username)
        {
            return _registeredPersons.Any(p => p.Username == username);
        }

        public Person? GetPersonByCredentials(string username, string password)
        {
            var person = _registeredPersons.FirstOrDefault(p => p.Username == username && p.Password == password);

            // Om personen hittas, anropa dess SignIn-metod
            person?.SignIn();
            return person;
        }

        public Person? GetPersonByUsername(string username)
        {
            // Returnera användaren om den finns i listan
            return _registeredPersons.FirstOrDefault(person => person.Username == username);
        }

        // Hämta alla användare som är av typen User 
        public List<User> GetAllUsers()
        {
            return _registeredPersons.OfType<User>().ToList();
        }
    }
}