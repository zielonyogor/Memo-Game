using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NR155910155992.MemoGame.UI.ViewModels
{
    public class GameFinishedViewModel : ViewModelBase
    {
        public int TotalPairs { get; set;  }

        public ICommand BackToMenu { get; }
        private string _timeFormatted;
        public string TimeFormatted
        {
            get => _timeFormatted;
            set
            {
                _timeFormatted = value;
                OnPropertyChanged(nameof(TimeFormatted));
            }
        }

        public GameFinishedViewModel(Action goBackToMainMenu, TimeSpan timeElapsed, int matchedPairs) 
        {
            //add timeplayed
            //BackToMenu = new RelayCommand((_) => OnBackToMenu());
            BackToMenu = new RelayCommand((_) => goBackToMainMenu());
            TotalPairs = matchedPairs;

            TimeFormatted = timeElapsed.ToString(@"mm\:ss");
            Debug.WriteLine($"Game finished in {TimeFormatted}");

        }

    }
}
