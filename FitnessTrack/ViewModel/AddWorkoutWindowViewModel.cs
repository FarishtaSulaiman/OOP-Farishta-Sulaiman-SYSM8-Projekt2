using FitnessTrack.Model;
using FitnessTrack.MVVM;
using FitnessTrack.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Collections.ObjectModel;

namespace FitnessTrack.ViewModel
{
    public class AddWorkoutWindowViewModel : ViewModelBase
    {
        private readonly UserManager _userManager;


        // Egenskaper för inmatningsfält
        public DateTime? WorkoutDate { get; set; } = DateTime.Now;
        public string WorkoutType { get; set; }
        public string WorkoutNotes { get; set; }
        public string Repetitions { get; set; }
        public string Distance { get; set; }

        private int _caloriesBurned;
        public int CaloriesBurned
        {
            get => _caloriesBurned;
            set
            {
                _caloriesBurned = value;
                OnPropertyChanged(nameof(CaloriesBurned));
            }
        }

        public ObservableCollection<string> WorkoutTypes { get; } = new ObservableCollection<string> { "Cardio", "Strength" };

        // Duration-filter förval
        public ObservableCollection<string> DurationOptions { get; } = new ObservableCollection<string> { "< 30 min", "30-60 min", "> 60 min" };
        public string SelectedDurationFilter { get; set; }

        public ICommand SaveWorkoutCommand { get; }
        public ICommand CancelCommand { get; }

        public AddWorkoutWindowViewModel(UserManager userManager)
        {
            _userManager = userManager;
            SaveWorkoutCommand = new RelayCommand(SaveWorkout);
            CancelCommand = new RelayCommand(Cancel);
        }

        private TimeSpan GetDurationFromFilter()
        {
            return SelectedDurationFilter switch
            {
                "< 30 min" => TimeSpan.FromMinutes(20),
                "30-60 min" => TimeSpan.FromMinutes(45),
                "> 60 min" => TimeSpan.FromMinutes(90),
                _ => TimeSpan.Zero
            };
        }

        private void SaveWorkout(object parameter)
        {
            // Validering för att säkerställa att alla obligatoriska fält är ifyllda
            if (!WorkoutDate.HasValue || string.IsNullOrEmpty(WorkoutType) || CaloriesBurned == 0 ||
                string.IsNullOrEmpty(WorkoutNotes) || string.IsNullOrEmpty(SelectedDurationFilter))
            {
                MessageBox.Show("Alla obligatoriska fält måste vara ifyllda.", "Fel", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Skapa ett nytt träningspass baserat på typen
            WorkOut newWorkout = WorkoutType == "Cardio"
                ? new CardioWorkout(WorkoutDate.Value, WorkoutType, GetDurationFromFilter(), CaloriesBurned, WorkoutNotes, int.TryParse(Distance, out var dist) ? dist : 0)
                : new StrengthWorkout(WorkoutDate.Value, WorkoutType, GetDurationFromFilter(), CaloriesBurned, WorkoutNotes, int.TryParse(Repetitions, out var reps) ? reps : 0);

            // Lägg till det nya träningspasset i användarens workout-lista
            _userManager.CurrentPerson.Workouts.Add(newWorkout);

            MessageBox.Show("Träningspasset har sparats.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w is AddWorkoutWindow)?.Close();
        }

        private void Cancel(object parameter)
        {
            Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w is AddWorkoutWindow)?.Close();
        }
    }
}



//skapa ett objekt av min workout new cardio = sen anropa calculatecalorie 