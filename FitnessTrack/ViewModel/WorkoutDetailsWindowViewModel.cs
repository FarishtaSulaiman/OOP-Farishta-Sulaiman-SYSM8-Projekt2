using FitnessTrack.Model;
using FitnessTrack.MVVM;
using FitnessTrack.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace FitnessTrack.ViewModel
{
    public class WorkoutDetailsWindowViewModel : ViewModelBase
    {
        private readonly UserManager _userManager; // Hanterar användardata
        public WorkOut SelectedWorkout { get; private set; } // Det valda träningspasset

        private bool _isEditable; // Flagga för att se om fälten är redigerbara
        public bool IsEditable
        {
            get => _isEditable; // Getter för att hämta redigeringsstatus
            set
            {
                _isEditable = value; // Sätt redigeringsstatus
                OnPropertyChanged(nameof(IsEditable)); // Meddelar att egenskapen har ändrats
                OnPropertyChanged(nameof(IsReadOnly)); // Meddelar att IsReadOnly också har ändrats
            }
        }

        // Beräknad egenskap för att avgöra om fälten ska vara skrivskyddade
        public bool IsReadOnly => !IsEditable; // Fälten är skrivskyddade om de inte är redigerbara

        // Lista för tillgängliga durationer
        public ObservableCollection<string> DurationOptions { get; } = new ObservableCollection<string>
        {
            "< 30 min",
            "30-60 min",
            "> 60 min"
        };

        private string _selectedDuration; // Vald duration
        public string SelectedDuration
        {
            get => _selectedDuration; // Getter för den valda durationen
            set
            {
                _selectedDuration = value; // Sätt vald duration
                OnPropertyChanged(nameof(SelectedDuration)); // Meddelar att egenskapen har ändrats
                UpdateDurationFromFilter(); // Uppdatera workoutens duration baserat på valet
            }
        }

        // Kommandon för knappar i UI
        public ICommand EditWorkoutCommand { get; }
        public ICommand CopyWorkoutCommand { get; }
        public ICommand SaveWorkoutCommand { get; }
        public ICommand CancelCommand { get; }

        // Konstruktör för ViewModel
        public WorkoutDetailsWindowViewModel(UserManager userManager, WorkOut selectedWorkout)
        {
            _userManager = userManager; // Spara referensen till användarhanteraren
            SelectedWorkout = selectedWorkout; // Spara det valda träningspasset
            IsEditable = false; // Fälten ska vara låsta från början

            // Initiera kommandon
            EditWorkoutCommand = new RelayCommand(EditWorkout);
            CopyWorkoutCommand = new RelayCommand(CopyWorkout);
            SaveWorkoutCommand = new RelayCommand(SaveWorkout);
            CancelCommand = new RelayCommand(Cancel);

            SetDurationFilterFromWorkout(); // Ställ in duration baserat på det valda träningspasset
        }

        // Låsa upp fälten för redigering
        private void EditWorkout(object parameter)
        {
            IsEditable = true; // Gör fälten redigerbara
        }

        // Skapa en kopia av det valda träningspasset
        private void CopyWorkout(object parameter)
        {
            WorkOut newWorkout;

            // Skapa en kopia av det valda träningspasset baserat på dess typ
            string workoutType = SelectedWorkout.Type.ToString();
            if (SelectedWorkout is CardioWorkout cardioWorkout)
            {
                newWorkout = new CardioWorkout(
                    cardioWorkout.Date,
                    workoutType + " - Copied",
                    cardioWorkout.Duration,
                    cardioWorkout.CaloriesBurned,
                    cardioWorkout.Notes,
                    cardioWorkout.Distance
                );
            }
            else if (SelectedWorkout is StrengthWorkout strengthWorkout)
            {
                newWorkout = new StrengthWorkout(
                    strengthWorkout.Date,
                    workoutType + " - Copied",
                    strengthWorkout.Duration,
                    strengthWorkout.CaloriesBurned,
                    strengthWorkout.Notes,
                    strengthWorkout.Repetitions
                );
            }
            else
            {
                throw new InvalidOperationException("Unknown workout type.");
            }

            // Lägg till det nya träningspasset i listan
            _userManager.CurrentPerson.Workouts.Add(newWorkout);
            MessageBox.Show("Träningspasset har kopierats.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // Spara ändringar till det valda träningspasset
        private void SaveWorkout(object parameter)
        {
            // Kontrollera att alla obligatoriska fält är ifyllda
            if (SelectedWorkout.Date == null || string.IsNullOrEmpty(SelectedWorkout.Type) || SelectedWorkout.Duration == TimeSpan.Zero || SelectedWorkout.CaloriesBurned <= 0 || string.IsNullOrEmpty(SelectedWorkout.Notes))
            {
                MessageBox.Show("Alla obligatoriska fält måste vara ifyllda.", "Fel", MessageBoxButton.OK, MessageBoxImage.Warning);
                return; // Avsluta metoden om några fält saknas
            }

            // Lägg till "- Edited" om det inte redan finns
            if (!SelectedWorkout.Type.EndsWith(" - Edited"))
            {
                SelectedWorkout.Type = SelectedWorkout.Type.Replace(" - Copied", "") + " - Edited";
            }

            MessageBox.Show("Träningspasset har sparats.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            CloseWorkoutDetailsWindowAndOpenWorkoutsWindow(); // Stäng detta fönster och öppna WorkoutWindow
        }

        // Avbryt och stäng fönstret utan att spara
        private void Cancel(object parameter)
        {
            CloseWorkoutDetailsWindow(); // Stäng detta fönster utan att spara
        }

        // Stänger WorkoutDetailsWindow och öppnar WorkoutWindow
        private void CloseWorkoutDetailsWindowAndOpenWorkoutsWindow()
        {
            Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w is WorkoutDetailsWindow)?.Close(); // Stäng WorkoutDetailsWindow
            var workoutsWindow = new WorkoutWindow(_userManager); // Öppna WorkoutWindow
            workoutsWindow.Show();
        }

        // Stänger WorkoutDetailsWindow
        private void CloseWorkoutDetailsWindow()
        {
            Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w is WorkoutDetailsWindow)?.Close(); // Stäng WorkoutDetailsWindow
        }

        // Ställ in duration baserat på det valda träningspasset
        private void SetDurationFilterFromWorkout()
        {
            if (SelectedWorkout.Duration.TotalMinutes < 30)
                SelectedDuration = "< 30 min";
            else if (SelectedWorkout.Duration.TotalMinutes <= 60)
                SelectedDuration = "30-60 min";
            else
                SelectedDuration = "> 60 min";
        }

        // Uppdatera workoutens duration baserat på den valda durationen
        private void UpdateDurationFromFilter()
        {
            SelectedWorkout.Duration = SelectedDuration switch
            {
                "< 30 min" => TimeSpan.FromMinutes(20), // Exempelvärde
                "30-60 min" => TimeSpan.FromMinutes(45), // Exempelvärde
                "> 60 min" => TimeSpan.FromMinutes(90), // Exempelvärde
                _ => SelectedWorkout.Duration
            };
        }
    }
}