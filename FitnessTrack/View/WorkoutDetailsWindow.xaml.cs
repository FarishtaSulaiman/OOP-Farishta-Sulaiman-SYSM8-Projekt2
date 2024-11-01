using FitnessTrack.Model;
using FitnessTrack.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FitnessTrack.View
{
    /// <summary>
    /// Interaction logic for WorkoutDetailsWindow.xaml
    /// </summary>
    public partial class WorkoutDetailsWindow : Window
    {
        private WorkOut selectedWorkout; // Det valda träningspasset

        // Konstruktor för att visa detaljer om ett träningspass
        public WorkoutDetailsWindow(UserManager userManager, WorkOut selectedWorkout)
        {
            InitializeComponent();
            this.selectedWorkout = selectedWorkout;

            // Skapa och sätta DataContext till ViewModel
            DataContext = new WorkoutDetailsWindowViewModel(userManager, selectedWorkout);
        }
    }
}