using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessTrack.Model
{
    public abstract class WorkOut
    {
        public DateTime Date { get; set; }
        public string Type { get; set; }
        public TimeSpan Duration { get; set; }
        public int CaloriesBurned { get; set; }
        public string Notes { get; set; }

        // Konstruktor med fem parametrar
        public WorkOut(DateTime date, string type, TimeSpan duration, int caloriesBurned, string notes)
        {
            Date = date;
            Type = type;
            Duration = duration;
            CaloriesBurned = caloriesBurned;
            Notes = notes;
        }

        // Abstrakt metod för att beräkna brända kalorier
        public abstract int CalculateCaloriesBurned();
    }
}
