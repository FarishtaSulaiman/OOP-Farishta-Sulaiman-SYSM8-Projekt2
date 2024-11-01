using FitnessTrack.MVVM;
using FitnessTrack.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace FitnessTrack.ViewModel
{
    public class VerificationWindowViewModel : ViewModelBase
    {
        // Genererad kod för verifiering
        private string _generatedCode;
        public string GeneratedCode
        {
            get => _generatedCode;
            set
            {
                _generatedCode = value;
                OnPropertyChanged();
            }
        }

        public string EnteredCode { get; set; }
        public string VerificationMessage { get; set; }

        public ICommand VerifyCommand { get; }

        private readonly Action _onSuccess;

        public VerificationWindowViewModel(string generatedCode, Action onSuccess)
        {
            GeneratedCode = generatedCode;  // Sätt den genererade koden för visning
            _onSuccess = onSuccess;
            VerifyCommand = new RelayCommand(VerifyCode);
        }

        private void VerifyCode(object parameter)
        {
            if (EnteredCode == GeneratedCode)
            {
                _onSuccess.Invoke(); // Anropa den metod som loggar in användaren
                Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w is VerificationWindow)?.Close();
            }
            else
            {
                VerificationMessage = "Incorrect code. Please try again.";
                OnPropertyChanged(nameof(VerificationMessage));
            }
        }
    }
}
