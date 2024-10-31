using FitnessTrack.Model;
using FitnessTrack.MVVM;
using FitnessTrack.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace FitnessTrack.ViewModel
{

    public class WorkoutWindowViewModel : ViewModelBase
    {
        private readonly UserManager _userManager;

        public Person CurrentPerson => _userManager.CurrentPerson;
        public ObservableCollection<WorkOut> FilteredWorkouts { get; set; } = new ObservableCollection<WorkOut>();

        public ICommand OpenAddWorkoutWindowCommand { get; }
        public ICommand OpenWorkoutDetailsCommand { get; }
        public ICommand RemoveWorkoutCommand { get; }
        public ICommand OpenUserDetailsCommand { get; }
        public ICommand ShowAppInfoCommand { get; }
        public ICommand SignOutCommand { get; }
        public ICommand ApplyFilterCommand { get; }

        private WorkOut _selectedWorkout;
        public WorkOut SelectedWorkout
        {
            get => _selectedWorkout;
            set
            {
                _selectedWorkout = value;
                OnPropertyChanged(nameof(SelectedWorkout));
            }
        }

        public ObservableCollection<string> FilterOptions { get; set; } = new ObservableCollection<string> { "All", "Cardio", "Strength" };
        private string _selectedFilter;
        public string SelectedFilter
        {
            get => _selectedFilter;
            set
            {
                _selectedFilter = value;
                OnPropertyChanged(nameof(SelectedFilter));
            }
        }

        private DateTime? _selectedDateFilter;
        public DateTime? SelectedDateFilter
        {
            get => _selectedDateFilter;
            set
            {
                _selectedDateFilter = value;
                OnPropertyChanged(nameof(SelectedDateFilter));
            }
        }

        private string _filterText;
        public string FilterText
        {
            get => _filterText;
            set
            {
                _filterText = value;
                OnPropertyChanged(nameof(FilterText));
            }
        }

        public ObservableCollection<string> DurationFilterOptions { get; set; } = new ObservableCollection<string>
        {
            "All", "< 30 min", "30-60 min", "> 60 min"
        };
        private string _selectedDurationFilterOption;
        public string SelectedDurationFilterOption
        {
            get => _selectedDurationFilterOption;
            set
            {
                _selectedDurationFilterOption = value;
                OnPropertyChanged(nameof(SelectedDurationFilterOption));
            }
        }

        public WorkoutWindowViewModel(UserManager userManager)
        {
            _userManager = userManager;
            OpenAddWorkoutWindowCommand = new RelayCommand(OpenAddWorkoutWindow);
            OpenWorkoutDetailsCommand = new RelayCommand(ShowWorkoutDetails, CanExecuteWorkoutCommand);
            RemoveWorkoutCommand = new RelayCommand(RemoveWorkout, CanExecuteWorkoutCommand);
            OpenUserDetailsCommand = new RelayCommand(OpenUserDetails);
            ShowAppInfoCommand = new RelayCommand(ShowAppInfo);
            SignOutCommand = new RelayCommand(SignOut);
            ApplyFilterCommand = new RelayCommand(ApplyFilter);
        }

        private void OpenAddWorkoutWindow(object parameter)
        {
            var addWorkoutWindow = new AddWorkoutWindow(_userManager);
            addWorkoutWindow.Show();
        }

        private void ShowWorkoutDetails(object parameter)
        {
            if (SelectedWorkout == null)
            {
                // Varningsruta om inget träningspass är markerat
                MessageBox.Show("Du måste först markera ett träningspass i listan för att visa detaljer.", "Varning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var workoutDetailsWindow = new WorkoutDetailsWindow(_userManager, SelectedWorkout);
            workoutDetailsWindow.Show();
        }

        private void RemoveWorkout(object parameter)
        {
            if (SelectedWorkout == null)
            {
                // Varningsruta om inget träningspass är markerat
                MessageBox.Show("Du måste först markera ett träningspass i listan för att ta bort det.", "Varning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (_userManager.CurrentPerson is AdminUser)
            {
                var owner = _userManager.GetAllUsers().FirstOrDefault(user => user.Workouts.Contains(SelectedWorkout));
                owner?.Workouts.Remove(SelectedWorkout);
            }
            else
            {
                _userManager.CurrentPerson.Workouts.Remove(SelectedWorkout);
            }

            FilteredWorkouts.Remove(SelectedWorkout);
            SelectedWorkout = null;
        }

        private void OpenUserDetails(object parameter)
        {
            var userDetailsWindow = new UserDetailsWindow(_userManager);
            userDetailsWindow.Show();
        }

        private void ShowAppInfo(object parameter)
        {
            var infoWindow = new InfoWindow();
            infoWindow.ShowDialog();
        }

        private void SignOut(object parameter)
        {
            var mainWindow = new MainWindow(_userManager);
            mainWindow.Show();
            Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w is WorkoutWindow)?.Close();
        }

        private void ApplyFilter(object parameter = null)
        {
            var filtered = _userManager.CurrentPerson.Workouts.Where(workout =>
                (string.IsNullOrEmpty(SelectedFilter) || SelectedFilter == "All" || workout.Type == SelectedFilter) &&
                (!SelectedDateFilter.HasValue || workout.Date.Date == SelectedDateFilter.Value.Date) &&
                (string.IsNullOrEmpty(FilterText) || workout.Type.Contains(FilterText, StringComparison.OrdinalIgnoreCase)) &&
                FilterByDuration(workout)
            ).ToList();

            FilteredWorkouts.Clear();
            foreach (var workout in filtered)
            {
                FilteredWorkouts.Add(workout);
            }
        }

        private bool FilterByDuration(WorkOut workout)
        {
            return SelectedDurationFilterOption switch
            {
                "< 30 min" => workout.Duration.TotalMinutes < 30,
                "30-60 min" => workout.Duration.TotalMinutes >= 30 && workout.Duration.TotalMinutes <= 60,
                "> 60 min" => workout.Duration.TotalMinutes > 60,
                _ => true
            };
        }

        private bool CanExecuteWorkoutCommand(object parameter) => true;
    }
}