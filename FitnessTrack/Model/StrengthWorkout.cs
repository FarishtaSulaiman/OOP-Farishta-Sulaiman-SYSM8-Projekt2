using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessTrack.Model
{
    public class StrengthWorkout : WorkOut
    {
        public int Repetitions { get; set; }

        public StrengthWorkout(DateTime date, string type, TimeSpan duration, int caloriesBurned, string notes, int repetitions)
            : base(date, type, duration, caloriesBurned, notes)
        {
            Repetitions = repetitions;
        }

        // Överlagrad metod för att beräkna kalorier för Strength
        public override int CalculateCaloriesBurned()
        {
            CaloriesBurned = Repetitions * 5; // Exempel: beräkna baserat på repetitioner
            return CaloriesBurned;
        }
    }
}