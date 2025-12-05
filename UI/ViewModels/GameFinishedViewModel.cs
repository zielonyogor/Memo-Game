using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NR155910155992.MemoGame.UI.ViewModels
{
    public class GameFinishedViewModel : ViewModelBase
    {
        public int TotalPairs { get; }

        public ICommand BackToMenu { get; }

        public GameFinishedViewModel(Action goBackToMainMenu) 
        {
            //add timeplayed
            //BackToMenu = new RelayCommand((_) => OnBackToMenu());
            BackToMenu = new RelayCommand((_) => goBackToMainMenu());


        }

    }
}
