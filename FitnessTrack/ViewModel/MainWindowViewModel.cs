using FitnessTrack.Model;
using FitnessTrack.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;


namespace FitnessTrack.ViewModel
{
   public class MainWindowViewModel : ViewModelBase
    {
        private readonly UserManager _userManager;

        public string Username { get; set; }
        public string Password { get; set; }

        public ICommand SignInCommand { get; }
        public ICommand ForgotPasswordCommand { get; }
        public ICommand RegisterCommand { get; }

        public MainWindowViewModel(UserManager userManager)
        {
            _userManager = userManager;
            SignInCommand = new RelayCommand(SignIn);
            ForgotPasswordCommand = new RelayCommand(ForgotPassword);
            RegisterCommand = new RelayCommand(Register);
        }

        private void SignIn(object parameter)
        {
            var user = _userManager.GetUserByCredentials(Username, Password);

            if (user != null && user.PassWord == Password)
            {
                var workoutsWindow = new WorkoutWindow(user);
                workoutsWindow.Show();
                Application.Current.Windows[0]?.Close();
            }
            else
            {
                MessageBox.Show("Fel användarnamn eller lösenord.", "Fel", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ForgotPassword(object parameter) { /* Din logik */ }

        private void Register(object parameter) { /* Din logik */ }
    }
}
