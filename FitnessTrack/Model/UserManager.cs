using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FitnessTrack.Model
{
    public class UserManager
    {
        private List<Person> _registeredPersons = new List<Person>(); // Ändrat från User till Person

        // Egenskap för att hålla koll på den inloggade personen (kan vara både User och AdminUser)
        public Person CurrentPerson { get; set; }

        public UserManager()
        {
            // Skapat och lagt till en User och en AdminUser i programmet från start 
            User defaultUser = new User("testUser", "Test123!", "Sverige", "Vad är ditt favoritdjur?", "hund");
            AdminUser adminUser = new AdminUser("adminUser", "Admin123!", "Sverige", "Vad är ditt favoritdjur?", "katt");


            // Lägger  till dem i listan över registrerade personer
            _registeredPersons.Add(defaultUser);
            _registeredPersons.Add(adminUser);
        }

        // Metod för att validera lösenordet
        public bool IsPasswordValid(string password)
        {
            // Kontrollera om lösenordet är minst 8 tecken, innehåller minst en siffra och ett specialtecken
            return password.Length >= 8 &&
                   Regex.IsMatch(password, @"[0-9]") &&          // Minst en siffra
                   Regex.IsMatch(password, @"[\W_]");            // Minst ett specialtecken
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