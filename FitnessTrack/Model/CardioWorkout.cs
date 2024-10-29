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

        public override int CalculateCaloriesBurned()
        {
            return CaloriesBurned + Distance * 5; // ett exempel på beräkning 
        }
    }
}