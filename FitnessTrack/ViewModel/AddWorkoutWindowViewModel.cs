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
        public DateTime? WorkoutDate { get; set; }
        public string WorkoutType { get; set; }
        public string CaloriesBurned { get; set; }
        public string WorkoutNotes { get; set; }
        public string Repetitions { get; set; }
        public string Distance { get; set; }

        // Lista över träningstyper för ComboBox
        public ObservableCollection<string> WorkoutTypes { get; } = new ObservableCollection<string> { "Cardio", "Strength" };

        // Filter för varaktighet
        public ObservableCollection<string> DurationOptions { get; } = new ObservableCollection<string> { "< 30 min", "30-60 min", "> 60 min" };
        public string SelectedDuration { get; set; }

        // Kommandon för att spara och avbryta
        public ICommand SaveWorkoutCommand { get; }
        public ICommand CancelCommand { get; }

        public AddWorkoutWindowViewModel(UserManager userManager)
        {
            _userManager = userManager;
            SaveWorkoutCommand = new RelayCommand(SaveWorkout);
            CancelCommand = new RelayCommand(Cancel);
        }

        // Metod för att tolka och konvertera varaktighet från DurationOptions
        private double GetDurationFromFilter()
        {
            return SelectedDuration switch
            {
                "< 30 min" => 20,
                "30-60 min" => 45,
                "> 60 min" => 90,
                _ => 0
            };
        }

        // Metod för att spara träningspasset
        private void SaveWorkout(object parameter)
        {
            // Kontrollera att alla obligatoriska fält är ifyllda
            if (!WorkoutDate.HasValue || string.IsNullOrEmpty(WorkoutType) || string.IsNullOrEmpty(CaloriesBurned) || string.IsNullOrEmpty(WorkoutNotes) ||
                (WorkoutType == "Cardio" && string.IsNullOrEmpty(Distance)) || (WorkoutType == "Strength" && string.IsNullOrEmpty(Repetitions)))
            {
                MessageBox.Show("Vänligen fyll i alla obligatoriska fält.", "Varning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Kontrollera att kalorier är ett giltigt heltal
            if (!int.TryParse(CaloriesBurned, out int calories))
            {
                MessageBox.Show("Kalorier måste vara ett giltigt heltal.", "Fel", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Kontrollera och konvertera specifika fält för Cardio och Strength
            WorkOut newWorkout;
            double durationMinutes = GetDurationFromFilter();

            if (WorkoutType == "Cardio")
            {
                if (!int.TryParse(Distance, out int cardioDistance))
                {
                    MessageBox.Show("Ange ett giltigt värde för Distance för Cardio-träning.", "Fel", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                newWorkout = new CardioWorkout(WorkoutDate.Value, WorkoutType, TimeSpan.FromMinutes(durationMinutes), calories, WorkoutNotes, cardioDistance);
            }
            else if (WorkoutType == "Strength")
            {
                if (!int.TryParse(Repetitions, out int strengthReps))
                {
                    MessageBox.Show("Ange ett giltigt värde för Repetitions för Strength-träning.", "Fel", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                newWorkout = new StrengthWorkout(WorkoutDate.Value, WorkoutType, TimeSpan.FromMinutes(durationMinutes), calories, WorkoutNotes, strengthReps);
            }
            else
            {
                MessageBox.Show("Ogiltig träningstyp. Välj 'Cardio' eller 'Strength'.", "Fel", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Lägg till träningspasset till den inloggade användarens lista
            _userManager.CurrentPerson.Workouts.Add(newWorkout);
            MessageBox.Show("Träningspasset har sparats.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);

            // Stäng fönstret efter sparande
            Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w is AddWorkoutWindow)?.Close();
        }

        // Metod för att avbryta och stänga fönstret
        private void Cancel(object parameter)
        {
            Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w is AddWorkoutWindow)?.Close();
        }
    }
}