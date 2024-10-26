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
        public User LoggedInUser { get; private set; }
        public ObservableCollection<Workout> FilteredWorkouts { get; set; }
        public Workout SelectedWorkout { get; set; }

        // Kommandon för att binda till knappar i WorkoutWindow
        public ICommand OpenAddWorkoutWindowCommand { get; }
        public ICommand OpenWorkoutDetailsCommand { get; }
        public ICommand RemoveWorkoutCommand { get; }
        public ICommand OpenUserDetailsCommand { get; }
        public ICommand ShowAppInfoCommand { get; }
        public ICommand SignOutCommand { get; }

        public WorkoutWindowViewModel(UserManager userManager)
        {
            _userManager = userManager;
            LoggedInUser = userManager.CurrentUser; // Hämtar den nuvarande inloggade användaren
            FilteredWorkouts = new ObservableCollection<Workout>(LoggedInUser.Workouts);

            // Initiera kommandon
            OpenAddWorkoutWindowCommand = new RelayCommand(OpenAddWorkoutWindow);
            OpenWorkoutDetailsCommand = new RelayCommand(OpenWorkoutDetails, CanExecuteWorkoutAction);
            RemoveWorkoutCommand = new RelayCommand(RemoveWorkout, CanExecuteWorkoutAction);
            OpenUserDetailsCommand = new RelayCommand(OpenUserDetails);
            ShowAppInfoCommand = new RelayCommand(ShowAppInfo);
            SignOutCommand = new RelayCommand(SignOut);
        }

        // Metoder för kommandon

        // Öppna fönstret för att lägga till nytt träningspass
        private void OpenAddWorkoutWindow()
        {
            var addWorkoutWindow = new AddWorkoutWindow(_userManager);
            addWorkoutWindow.ShowDialog();

            // Uppdatera listan med träningspass efter att ett nytt pass lagts till
            RefreshFilteredWorkouts();
        }

        // Öppna detaljfönster för valt träningspass
        private void OpenWorkoutDetails()
        {
            if (SelectedWorkout != null)
            {
                var detailsWindow = new WorkoutDetailsWindow(SelectedWorkout);
                detailsWindow.ShowDialog();
            }
        }

        // Ta bort valt träningspass
        private void RemoveWorkout()
        {
            if (SelectedWorkout != null)
            {
                LoggedInUser.Workouts.Remove(SelectedWorkout);
                RefreshFilteredWorkouts();
            }
            else
            {
                MessageBox.Show("Vänligen välj ett träningspass att ta bort.", "Varning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // Öppna fönster för att visa och redigera användarens detaljer
        private void OpenUserDetails()
        {
            var userDetailsWindow = new UserDetailsWindow(LoggedInUser);
            userDetailsWindow.ShowDialog();
        }

        // Visa appinformation
        private void ShowAppInfo()
        {
            MessageBox.Show("FitTrack - Håll koll på dina träningspass och framsteg.\nVersion 1.0", "Appinfo", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // Logga ut och stäng fönstret
        private void SignOut()
        {
            Application.Current.Shutdown();
        }

        // Hjälpmetod för att uppdatera listan med filtrerade träningspass
        private void RefreshFilteredWorkouts()
        {
            FilteredWorkouts.Clear();
            foreach (var workout in LoggedInUser.Workouts)
            {
                FilteredWorkouts.Add(workout);
            }
        }

        // Metod för att kontrollera om ett träningspass är valt innan vissa kommandon utförs
        private bool CanExecuteWorkoutAction()
        {
            return SelectedWorkout != null;
        }
    }
}