using FitnessTrack.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FitnessTrack.ViewModel
{
    public class SplashScreenViewModel : ViewModelBase
    {
        public ICommand GetStartedCommand { get; }

        public SplashScreenViewModel(Action openMainWindow)
        {
            GetStartedCommand = new RelayCommand(o => openMainWindow());
        }
    }
}