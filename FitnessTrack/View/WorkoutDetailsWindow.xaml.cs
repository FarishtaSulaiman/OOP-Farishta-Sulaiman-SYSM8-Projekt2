﻿using FitnessTrack.Model;
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
        public WorkoutDetailsWindow(WorkOut workout)
        {
            InitializeComponent();

            // Sätter DataContext till en instans av WorkoutDetailsWindowViewModel
            DataContext = new WorkoutDetailsWindowViewModel(workout);
        }
    }
}