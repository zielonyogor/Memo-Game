using NR155910155992.MemoGame.Interfaces;
using NR155910155992.MemoGame.UI.Commands;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;

namespace NR155910155992.MemoGame.UI.ViewModels
{
    public class GameSessionViewModel : ViewModelBase
    {
        public IGameManager _gameManager;
        public ICommand BackToMenu { get; }

        public ObservableCollection<IGameSession> GameSessions { get; } = new();
        public GameSessionViewModel(IGameManager gameManager, Action goBackToMainMenu)
        {
            _gameManager = gameManager;
            BackToMenu = new RelayCommand((_) => goBackToMainMenu());

            var sessions = _gameManager.GetAllGameSessions();
            foreach(var s in sessions)
            {
                GameSessions.Add(s);
            }
        }

    }

}
