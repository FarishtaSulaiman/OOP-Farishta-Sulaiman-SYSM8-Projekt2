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
    /// Interaction logic for UserDetailsWindow.xaml
    /// </summary>

    public partial class UserDetailsWindow : Window
    {
            public UserDetailsWindow(UserManager userManager)
            {
                InitializeComponent();
                DataContext = new UserDetailsWindowViewModel(userManager);

                // Bindar PasswordBox lösenord till ViewModel
                NewPasswordBox.PasswordChanged += OnNewPasswordChanged;
                ConfirmPasswordBox.PasswordChanged += OnConfirmPasswordChanged;
            }

            private void OnNewPasswordChanged(object sender, RoutedEventArgs e)
            {
                if (DataContext is UserDetailsWindowViewModel viewModel)
                {
                    viewModel.NewPassword = NewPasswordBox.Password;
                }
            }

            private void OnConfirmPasswordChanged(object sender, RoutedEventArgs e)
            {
                if (DataContext is UserDetailsWindowViewModel viewModel)
                {
                    viewModel.ConfirmPassword = ConfirmPasswordBox.Password;
                }
            }
        }
    }