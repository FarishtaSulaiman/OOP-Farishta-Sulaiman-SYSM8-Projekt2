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

        // Egenskap för att visa den inloggade användaren
        public Person CurrentPerson => _userManager.CurrentPerson;

        // Lista med träningspass som visas i ListView
        public ObservableCollection<WorkOut> FilteredWorkouts { get; set; }

        // Kommando för att öppna andra fönster och hantera träningspass
        public ICommand OpenAddWorkoutWindowCommand { get; }
        public ICommand OpenWorkoutDetailsCommand { get; }
        public ICommand RemoveWorkoutCommand { get; }
        public ICommand OpenUserDetailsCommand { get; }
        public ICommand ShowAppInfoCommand { get; }
        public ICommand SignOutCommand { get; }
        public ICommand ApplyFilterCommand { get; }

        // För att hålla koll på valt träningspass
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

        // Filteralternativ för ComboBox
        public ObservableCollection<string> FilterOptions { get; set; } = new ObservableCollection<string> { "All", "Cardio", "Strength" };

        // För att hålla det valda filtret
        private string _selectedFilter;
        public string SelectedFilter
        {
            get => _selectedFilter;
            set
            {
                _selectedFilter = value;
                OnPropertyChanged(nameof(SelectedFilter));
                ApplyFilter();
            }
        }

        // För att filtrera på datum
        private DateTime? _selectedDateFilter;
        public DateTime? SelectedDateFilter
        {
            get => _selectedDateFilter;
            set
            {
                _selectedDateFilter = value;
                OnPropertyChanged(nameof(SelectedDateFilter));
                ApplyFilter();
            }
        }

        // För att filtrera på text
        private string _filterText;
        public string FilterText
        {
            get => _filterText;
            set
            {
                _filterText = value;
                OnPropertyChanged(nameof(FilterText));
                ApplyFilter();
            }
        }

        // Konstruktor
        public WorkoutWindowViewModel(UserManager userManager)
        {
            _userManager = userManager;
            FilteredWorkouts = new ObservableCollection<WorkOut>(_userManager.CurrentPerson.Workouts);
            // Kontrollera om inloggad användare är AdminUser och hämta träningspass baserat på detta
            LoadWorkouts();

            // Initiera kommandona
            OpenAddWorkoutWindowCommand = new RelayCommand(OpenAddWorkoutWindow);
            OpenWorkoutDetailsCommand = new RelayCommand(ShowWorkoutDetails);
            RemoveWorkoutCommand = new RelayCommand(RemoveWorkout);
            OpenUserDetailsCommand = new RelayCommand(OpenUserDetails);
            ShowAppInfoCommand = new RelayCommand(ShowAppInfo);
            SignOutCommand = new RelayCommand(SignOut);
            ApplyFilterCommand = new RelayCommand(ApplyFilter);
        }

        // Funktion som hämtar träningspass baserat på användarroll
        private void LoadWorkouts()
        {
            if (_userManager.CurrentPerson is AdminUser)
            {
                // Om användaren är AdminUser, visa alla användares träningspass
                FilteredWorkouts = new ObservableCollection<WorkOut>(
                    _userManager.GetAllUsers().SelectMany(user => user.Workouts));
            }
            else
            {
                // Om användaren är vanlig User, visa bara den inloggade användarens träningspass
                FilteredWorkouts = new ObservableCollection<WorkOut>(_userManager.CurrentPerson.Workouts);
            }

            OnPropertyChanged(nameof(FilteredWorkouts));
        }
    
    // Metod för att öppna AddWorkoutWindow
    private void OpenAddWorkoutWindow(object parameter)
        {
            var addWorkoutWindow = new AddWorkoutWindow(_userManager);
            addWorkoutWindow.ShowDialog();
        }

        // Metod för att öppna WorkoutDetailsWindow om ett träningspass är markerat
        private void OpenWorkoutDetails(object parameter)
        {
            if (SelectedWorkout == null)
            {
                MessageBox.Show("Vänligen välj ett träningspass först.", "Varning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var detailsWindow = new WorkoutDetailsWindow(SelectedWorkout);
            detailsWindow.Show();
        }
        // Metod för att visa detaljer för det markerade träningspasset
        private void ShowWorkoutDetails(object parameter)
        {
            if (SelectedWorkout == null)
            {
                MessageBox.Show("Vänligen markera ett träningspass för att visa detaljer.", "Varning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Skapa och visa WorkoutDetailsWindow om ett pass är markerat
            var workoutDetailsWindow = new WorkoutDetailsWindow(SelectedWorkout);
            workoutDetailsWindow.Show();
        }

        // Metod för att ta bort det markerade träningspasset
        private void RemoveWorkout(object parameter)
        {
            if (SelectedWorkout == null)
            {
                MessageBox.Show("Vänligen markera ett träningspass för att ta bort.", "Varning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Om den inloggade användaren är AdminUser
            if (_userManager.CurrentPerson is AdminUser)
            {
                // Hitta användaren som äger träningspasset och ta bort det från deras lista
                var owner = _userManager.GetAllUsers().FirstOrDefault(user => user.Workouts.Contains(SelectedWorkout));
                owner?.Workouts.Remove(SelectedWorkout);
            }
            else
            {
                // Om det är en vanlig användare, ta bort träningspasset från deras egen lista
                _userManager.CurrentPerson.Workouts.Remove(SelectedWorkout);
            }

            // Uppdatera den visade listan
            FilteredWorkouts.Remove(SelectedWorkout);
            SelectedWorkout = null;
        }
    

       
      

        // Metod för att öppna UserDetailsWindow
        private void OpenUserDetails(object parameter)
        {
            var userDetailsWindow = new UserDetailsWindow(_userManager);
            userDetailsWindow.Show();
        }

        // Metod för att visa appinfo
        private void ShowAppInfo(object parameter)
        {
            MessageBox.Show("FitTrack är en applikation för att hantera och spåra träningspass.", "Appinfo", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // Metod för att logga ut och stänga WorkoutWindow
        private void SignOut(object parameter)
        {
            // Skapa huvudfönstret och skicka vidare UserManager
            var mainWindow = new MainWindow(_userManager);
            mainWindow.Show();

            // Stäng WorkoutWindow
            Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w is WorkoutWindow)?.Close();
        }
    

        // Filtreringslogik
        private void ApplyFilter(object parameter = null)
        {
            var filtered = _userManager.CurrentPerson.Workouts.Where(workout =>
                (string.IsNullOrEmpty(SelectedFilter) || SelectedFilter == "All" || workout.Type == SelectedFilter) &&
                (!SelectedDateFilter.HasValue || workout.Date.Date == SelectedDateFilter.Value.Date) &&
                (string.IsNullOrEmpty(FilterText) || workout.Type.Contains(FilterText, StringComparison.OrdinalIgnoreCase))
            ).ToList();

            FilteredWorkouts.Clear();
            foreach (var workout in filtered)
            {
                FilteredWorkouts.Add(workout);
            }
        }

        // Kontrollera om ett träningspass är markerat
        private bool CanExecuteWorkoutCommand(object parameter) => SelectedWorkout != null;
    }
}
