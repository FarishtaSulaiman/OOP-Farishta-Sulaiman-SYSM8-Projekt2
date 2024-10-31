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
        private readonly UserManager _userManager;
        private bool _isEditable;

        public bool IsEditable
        {
            get => _isEditable;
            set
            {
                _isEditable = value;
                OnPropertyChanged(nameof(IsEditable));
                OnPropertyChanged(nameof(IsReadOnly));
            }
        }

        public bool IsReadOnly => !IsEditable;

        public WorkOut SelectedWorkout { get; }

        public ObservableCollection<string> DurationOptions { get; } = new ObservableCollection<string>
        {
            "< 30 min",
            "30-60 min",
            "> 60 min"
        };

        private string _selectedDuration;
        public string SelectedDuration
        {
            get => _selectedDuration;
            set
            {
                _selectedDuration = value;
                OnPropertyChanged(nameof(SelectedDuration));
                UpdateDurationFromFilter();
            }
        }

        public ICommand EditWorkoutCommand { get; }
        public ICommand CopyWorkoutCommand { get; }
        public ICommand SaveWorkoutCommand { get; }
        public ICommand CancelCommand { get; }

        public WorkoutDetailsWindowViewModel(UserManager userManager, WorkOut selectedWorkout)
        {
            _userManager = userManager;
            SelectedWorkout = selectedWorkout;
            IsEditable = false;

            EditWorkoutCommand = new RelayCommand(EditWorkout);
            CopyWorkoutCommand = new RelayCommand(CopyWorkout);
            SaveWorkoutCommand = new RelayCommand(SaveWorkout);
            CancelCommand = new RelayCommand(Cancel);

            SetDurationFilterFromWorkout();
        }

        private void SetDurationFilterFromWorkout()
        {
            if (SelectedWorkout.Duration.TotalMinutes < 30)
                SelectedDuration = "< 30 min";
            else if (SelectedWorkout.Duration.TotalMinutes <= 60)
                SelectedDuration = "30-60 min";
            else
                SelectedDuration = "> 60 min";
        }

        private void UpdateDurationFromFilter()
        {
            SelectedWorkout.Duration = SelectedDuration switch
            {
                "< 30 min" => TimeSpan.FromMinutes(20),
                "30-60 min" => TimeSpan.FromMinutes(45),
                "> 60 min" => TimeSpan.FromMinutes(90),
                _ => SelectedWorkout.Duration
            };
        }

        private void EditWorkout(object parameter)
        {
            IsEditable = true;
        }

        private void CopyWorkout(object parameter)
        {
            WorkOut newWorkout;
            string workoutType = SelectedWorkout.Type.ToString().Replace(" - Edited", "").Replace(" - Copied", "");

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
                MessageBox.Show("Okänd typ av träningspass.", "Fel", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            _userManager.CurrentPerson.Workouts.Add(newWorkout);
            MessageBox.Show("Träningspasset har kopierats som en mall.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void SaveWorkout(object parameter)
        {
            if (SelectedWorkout.Date == null || string.IsNullOrEmpty(SelectedWorkout.Type) || SelectedWorkout.Duration == TimeSpan.Zero || SelectedWorkout.CaloriesBurned <= 0 || string.IsNullOrEmpty(SelectedWorkout.Notes))
            {
                MessageBox.Show("Alla obligatoriska fält måste vara ifyllda.", "Fel", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Lägg till "- Edited" om det inte redan finns
            if (!SelectedWorkout.Type.EndsWith(" - Edited"))
            {
                SelectedWorkout.Type = SelectedWorkout.Type.Replace(" - Copied", "") + " - Edited";
            }

            MessageBox.Show("Träningspasset har sparats.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            CloseWorkoutDetailsWindowAndOpenWorkoutsWindow();
        }

        private void Cancel(object parameter)
        {
            CloseWorkoutDetailsWindow();
        }

        private void CloseWorkoutDetailsWindowAndOpenWorkoutsWindow()
        {
            Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w is WorkoutDetailsWindow)?.Close();

            var workoutsWindow = new WorkoutWindow(_userManager);
            workoutsWindow.Show();
        }

        private void CloseWorkoutDetailsWindow()
        {
            Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w is WorkoutDetailsWindow)?.Close();
        }
    }
}