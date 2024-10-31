using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessTrack.Model
{
    public class CardioWorkout : WorkOut
    {
        public int Distance { get; set; }

        public CardioWorkout(DateTime date, string type, TimeSpan duration, int caloriesBurned, string notes, int distance)
            : base(date, type, duration, caloriesBurned, notes)
        {
            Distance = distance;
        }

        // Överlagrad metod för att beräkna kalorier för Cardio
        public override int CalculateCaloriesBurned()
        {
            CaloriesBurned = Distance * 10; // Exempel: beräkna baserat på avstånd
            return CaloriesBurned;
        }
    }
}