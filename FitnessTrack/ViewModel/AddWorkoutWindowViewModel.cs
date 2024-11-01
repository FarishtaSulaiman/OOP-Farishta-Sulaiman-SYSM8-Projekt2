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
    public class AddWorkoutViewModel : ViewModelBase
    {
        private readonly UserManager _userManager;
        private readonly Window _workoutWindow; // Referens till WorkoutWindow
       

        public string WorkoutType { get; set; }  // Typ av träningspass
        public DateTime WorkoutDate { get; set; } = DateTime.Today; // Förinställt till dagens datum
        public int Distance { get; set; }
        public int Repetitions { get; set; }
        public string SelectedDuration { get; set; }
        public int CaloriesBurned { get; set; }
        public string WorkoutNotes { get; set; }

        public ObservableCollection<string> WorkoutTypes { get; set; } = new ObservableCollection<string>
        {
            "Cardio", "Strength"
        };

        public ObservableCollection<string> DurationOptions { get; set; } = new ObservableCollection<string>
        {
            "< 30 min", "30-60 min", "> 60 min"
        };

        public ICommand SaveWorkoutCommand { get; }
        public ICommand CancelCommand { get; }

        public AddWorkoutViewModel(UserManager userManager, Window workoutWindow)
        {
            _userManager = userManager;
            _workoutWindow = workoutWindow; // Spara referensen till WorkoutWindow

            SaveWorkoutCommand = new RelayCommand(SaveWorkout);
            CancelCommand = new RelayCommand(Cancel);
        }

        private void SaveWorkout(object parameter)
        {
            // Validering av inmatningarna
            if (string.IsNullOrWhiteSpace(WorkoutType) ||
                string.IsNullOrWhiteSpace(WorkoutNotes) ||
                WorkoutDate == default ||
                (WorkoutType == "Cardio" && (Distance <= 0 || CaloriesBurned <= 0 || string.IsNullOrWhiteSpace(SelectedDuration))) ||
                (WorkoutType == "Strength" && (Repetitions <= 0 || CaloriesBurned <= 0 || string.IsNullOrWhiteSpace(SelectedDuration))))
            {
                ShowRequiredFieldsMessage();
                return;
            }

            // Skapa ett nytt träningspass
            WorkOut newWorkout = WorkoutType == "Cardio"
                ? new CardioWorkout(WorkoutDate, WorkoutType, TimeSpan.FromMinutes(GetDuration()), CaloriesBurned, WorkoutNotes, Distance)
                : new StrengthWorkout(WorkoutDate, WorkoutType, TimeSpan.FromMinutes(GetDuration()), CaloriesBurned, WorkoutNotes, Repetitions);

            // Lägga till det nya träningspasset till användarens lista
            _userManager.CurrentPerson.Workouts.Add(newWorkout);
            MessageBox.Show("Träningspass sparat.");

            // Skapa och visa en ny instans av WorkoutWindow
            var workoutWindow = new WorkoutWindow(_userManager);
            workoutWindow.Show(); // Visa WorkoutWindow
                                  // Application.Current.Windows[0]?.Close(); // Stänger AddWorkoutWindow
         

        }

        private void ShowRequiredFieldsMessage(string message = "Vänligen fyll i alla obligatoriska fält.")
        {
            MessageBox.Show(message, "Obligatoriska fält", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private double GetDuration()
        {
            // Omvandla vald duration från string till minut
            return SelectedDuration switch
            {
                "< 30 min" => 15,
                "30-60 min" => 45,
                "> 60 min" => 75,
                _ => 0
            };
        }

        private void Cancel(object parameter)
        {
            Application.Current.Windows[0]?.Close(); // Stänger fönstret utan att spara
        }
    }
}