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
        private WorkOut _selectedWorkout;

        // Egenskap för att visa den inloggade användaren
        public Person CurrentPerson => _userManager.CurrentPerson;

        // Lista med träningspass som visas i ListView
        public ObservableCollection<WorkOut> FilteredWorkouts { get; set; }

        // Kommando-egenskaper för knappar och interaktioner
        public ICommand OpenAddWorkoutWindowCommand { get; }
        public ICommand OpenWorkoutDetailsCommand { get; }
        public ICommand RemoveWorkoutCommand { get; }
        public ICommand OpenUserDetailsCommand { get; }
        public ICommand ShowAppInfoCommand { get; }
        public ICommand SignOutCommand { get; }
        public ICommand ApplyFilterCommand { get; }

        // För att hålla koll på valt träningspass
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
            OnPropertyChanged(nameof(CurrentPerson));

            // Hämta träningspass för inloggad användare
            FilteredWorkouts = new ObservableCollection<WorkOut>(_userManager.CurrentPerson.Workouts);

            // Initiera kommandona
            OpenAddWorkoutWindowCommand = new RelayCommand(OpenAddWorkoutWindow);
            OpenWorkoutDetailsCommand = new RelayCommand(OpenWorkoutDetails, CanExecuteWorkoutCommand);
            RemoveWorkoutCommand = new RelayCommand(RemoveWorkout, CanExecuteWorkoutCommand);
            OpenUserDetailsCommand = new RelayCommand(OpenUserDetails);
            ShowAppInfoCommand = new RelayCommand(ShowAppInfo);
            SignOutCommand = new RelayCommand(SignOut);
            ApplyFilterCommand = new RelayCommand(ApplyFilter);
        }
    }
}
