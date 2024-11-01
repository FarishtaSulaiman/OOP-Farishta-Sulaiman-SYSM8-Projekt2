using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessTrack.Model
{
    public class CardioWorkout : WorkOut
    {
        public int Distance { get; set; } // Avstånd i kilometer

        public CardioWorkout(DateTime date, string type, TimeSpan duration, int caloriesBurned, string notes, int distance)
            : base(date, type, duration, caloriesBurned, notes)
        {
            Distance = distance;
        }

        public override int CalculateCaloriesBurned()
        {
            // Exempel på kaloriberäkning för konditionsträning baserat på duration och distance
            CaloriesBurned = (int)(Distance * 1.2 * Duration.TotalMinutes);
            return CaloriesBurned;
        }
    }
}