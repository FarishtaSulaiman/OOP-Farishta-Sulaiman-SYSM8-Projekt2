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
using static System.Net.Mime.MediaTypeNames;

namespace FitnessTrack.ViewModel
{
    public class WorkoutWindowViewModel : ViewModelBase
    {
        private readonly UserManager _userManager; // Hanterar användardata
        private readonly Window _workoutWindow; // Referens till WorkoutWindow

        public Person CurrentPerson => _userManager.CurrentPerson; // Aktuell inloggad användare
        public ObservableCollection<WorkOut> FilteredWorkouts { get; set; } = new ObservableCollection<WorkOut>(); // Lista över filtrerade träningspass

        private WorkOut _selectedWorkout; // Det markerade träningspasset
        public WorkOut SelectedWorkout
        {
            get => _selectedWorkout;
            set
            {
                _selectedWorkout = value;
                OnPropertyChanged(nameof(SelectedWorkout)); // Meddelar om att egenskapen har ändrats
            }
        }

        // Kommandon för knappar i UI
        public ICommand OpenAddWorkoutWindowCommand { get; }
        public ICommand OpenWorkoutDetailsCommand { get; }
        public ICommand RemoveWorkoutCommand { get; }
        public ICommand OpenUserDetailsCommand { get; }
        public ICommand ShowAppInfoCommand { get; }
        public ICommand SignOutCommand { get; }
        public ICommand ApplyFilterCommand { get; }

        public ObservableCollection<string> FilterOptions { get; set; } = new ObservableCollection<string> { "All", "Cardio", "Strength" };

        private string _selectedFilter; // Det valda filtret
        public string SelectedFilter
        {
            get => _selectedFilter;
            set
            {
                _selectedFilter = value;
                OnPropertyChanged(nameof(SelectedFilter));
                OnPropertyChanged(nameof(IsApplyFilterEnabled)); // Uppdatera aktiv status för filterknappen
            }
        }

        private DateTime? _selectedDateFilter; // Datum för filter
        public DateTime? SelectedDateFilter
        {
            get => _selectedDateFilter;
            set
            {
                _selectedDateFilter = value;
                OnPropertyChanged(nameof(SelectedDateFilter));
                OnPropertyChanged(nameof(IsApplyFilterEnabled)); // Uppdatera aktiv status för filterknappen
            }
        }

        private string _filterText; // Textfilter
        public string FilterText
        {
            get => _filterText;
            set
            {
                _filterText = value;
                OnPropertyChanged(nameof(FilterText));
                OnPropertyChanged(nameof(IsApplyFilterEnabled)); // Uppdatera aktiv status för filterknappen
            }
        }

        public ObservableCollection<string> DurationFilterOptions { get; set; } = new ObservableCollection<string>
        {
            "All", "< 30 min", "30-60 min", "> 60 min"
        };

        private string _selectedDurationFilterOption; // Vald duration för filter
        public string SelectedDurationFilterOption
        {
            get => _selectedDurationFilterOption;
            set
            {
                _selectedDurationFilterOption = value;
                OnPropertyChanged(nameof(SelectedDurationFilterOption));
                OnPropertyChanged(nameof(IsApplyFilterEnabled)); // Uppdatera aktiv status för filterknappen
            }
        }

        // Egenskap för att avgöra om Apply Filter-knappen ska vara aktiv
        public bool IsApplyFilterEnabled => !string.IsNullOrEmpty(SelectedFilter) || SelectedDateFilter.HasValue || !string.IsNullOrWhiteSpace(FilterText) || !string.IsNullOrEmpty(SelectedDurationFilterOption);

        public WorkoutWindowViewModel(UserManager userManager, Window workoutWindow)
        {
            _userManager = userManager; // Spara referensen till användarhanteraren
            _workoutWindow = workoutWindow; // Spara referensen till WorkoutWindow

            // Initiera kommandon
            OpenAddWorkoutWindowCommand = new RelayCommand(OpenAddWorkoutWindow);
            OpenWorkoutDetailsCommand = new RelayCommand(ShowWorkoutDetails);
            RemoveWorkoutCommand = new RelayCommand(RemoveWorkout);
            OpenUserDetailsCommand = new RelayCommand(OpenUserDetails);
            ShowAppInfoCommand = new RelayCommand(ShowAppInfo);
            SignOutCommand = new RelayCommand(SignOut);
            ApplyFilterCommand = new RelayCommand(ApplyFilter);

            LoadWorkouts(); // Ladda träningspassen när ViewModel initialiseras
        }

        private void LoadWorkouts()
        {
            // Ladda träningspass för den aktuella användaren
            FilteredWorkouts.Clear(); // Rensa tidigare listan

            if (CurrentPerson is AdminUser) // Om den inloggade användaren är admin
            {
                // Hämta alla träningspass från alla användare
                foreach (var user in _userManager.GetAllUsers())
                {
                    foreach (var workout in user.Workouts)
                    {
                        FilteredWorkouts.Add(workout); // Lägg till varje träningspass i listan
                    }
                }
            }
            else if (CurrentPerson is User user) // Om den inloggade användaren är en vanlig användare
            {
                foreach (var workout in user.Workouts)
                {
                    FilteredWorkouts.Add(workout); // Lägg till endast den aktuella användarens träningspass
                }
            }
        }

        private void OpenAddWorkoutWindow(object parameter)
        {
            // Öppna fönstret för att lägga till ett nytt träningspass
            var addWorkoutWindow = new AddWorkoutWindow(_userManager, _workoutWindow); // Skicka referens till WorkoutWindow
            _workoutWindow.Hide(); // Döljer WorkoutWindow
            addWorkoutWindow.ShowDialog(); // Visa fönstret
            LoadWorkouts(); // Ladda om träningspass efter att ett nytt har lagts till
        }

        private void ShowWorkoutDetails(object parameter)
        {
            // Kontrollera om något träningspass är markerat
            if (SelectedWorkout == null)
            {
                MessageBox.Show("Vänligen markera ett träningspass för att se detaljer.", "Ingen Markering", MessageBoxButton.OK, MessageBoxImage.Warning);
                return; // Avsluta metoden om inget pass är markerat
            }

            // Skapa och visa WorkoutDetailsWindow
            var detailsWindow = new WorkoutDetailsWindow(_userManager, SelectedWorkout);
            detailsWindow.ShowDialog(); // Visa detaljer
        }

        private void RemoveWorkout(object parameter)
        {
            // Kontrollera om något träningspass är markerat
            if (SelectedWorkout == null)
            {
                MessageBox.Show("Vänligen markera ett träningspass för att ta bort det.", "Ingen Markering", MessageBoxButton.OK, MessageBoxImage.Warning);
                return; // Avsluta metoden om inget pass är markerat
            }

            // Ta bort det markerade träningspasset
            _userManager.CurrentPerson.Workouts.Remove(SelectedWorkout);
            FilteredWorkouts.Remove(SelectedWorkout); // Ta bort från den filtrerade listan
        }

        private void OpenUserDetails(object parameter)
        {
            // Öppna användarinformationen
            var userDetailsWindow = new UserDetailsWindow(_userManager);
            userDetailsWindow.ShowDialog(); // Visa användarinformation
        }

        private void ShowAppInfo(object parameter)
        {
            // Öppna app-informationsfönstret
            var infoWindow = new InfoWindow();
            infoWindow.ShowDialog(); // Visa app-info
        }

        private void SignOut(object parameter)
        {
            // Logik för utloggning (kan implementeras enligt behov)
            var mainWindow = new MainWindow(_userManager);
            mainWindow.Show(); // Öppna huvudfönstret
            //System.Windows.Application.Current.Windows[0]?.Close(); // Stäng nuvarande fönster
            _workoutWindow.Close();
        }

        private void ApplyFilter(object parameter = null)
        {
            // Kontrollera att minst ett filter är valt
            if (string.IsNullOrEmpty(SelectedFilter) && !SelectedDateFilter.HasValue && string.IsNullOrEmpty(FilterText) && string.IsNullOrEmpty(SelectedDurationFilterOption))
            {
                FilteredWorkouts.Clear(); // Töm listan om inget filter är valt
                return; // Avsluta metoden om inget filter är valt
            }

            // Filtrera träningspassen baserat på valda kriterier
            var filtered = _userManager.CurrentPerson.Workouts.Where(workout =>
                (string.IsNullOrEmpty(SelectedFilter) || workout.Type == SelectedFilter) &&
                (!SelectedDateFilter.HasValue || workout.Date.Date == SelectedDateFilter.Value.Date) &&
                (string.IsNullOrEmpty(FilterText) || workout.Type.Contains(FilterText, StringComparison.OrdinalIgnoreCase)) &&
                FilterByDuration(workout) // Filtrera baserat på duration
            ).ToList();

            // Uppdatera FilteredWorkouts
            FilteredWorkouts.Clear(); // Rensa tidigare listan
            foreach (var workout in filtered)
            {
                FilteredWorkouts.Add(workout); // Lägg till filtrerade träningspass
            }
        }

        private bool FilterByDuration(WorkOut workout)
        {
            // Filtrera träningspass baserat på valda duration
            if (string.IsNullOrEmpty(SelectedDurationFilterOption))
            {
                return true; // Inkludera alla om inget duration-filter är valt
            }

            return SelectedDurationFilterOption switch
            {
                "< 30 min" => workout.Duration.TotalMinutes < 30,
                "30-60 min" => workout.Duration.TotalMinutes >= 30 && workout.Duration.TotalMinutes <= 60,
                "> 60 min" => workout.Duration.TotalMinutes > 60,
                _ => true // Inkludera alla om inget giltigt filter är valt
            };
        }
    }
}