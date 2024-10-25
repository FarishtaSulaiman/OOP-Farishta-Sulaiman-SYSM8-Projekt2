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

        public override int CalculateCaloriesBurned()
        {
            return CaloriesBurned + Repetitions * 2;
        }
    }
}