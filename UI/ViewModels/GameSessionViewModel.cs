using NR155910155992.MemoGame.Interfaces;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;

namespace NR155910155992.MemoGame.UI.ViewModels
{
    public class GameSessionViewModel : ViewModelBase
    {
        public IGameSessionHistory _gameSessionHistory;
        public ICommand BackToMenu { get; }

        public ObservableCollection<IGameSession> GameSessions { get; } = new();
        public GameSessionViewModel(IGameSessionHistory gameSessionHistory, Action goBackToMainMenu)
        {
            _gameSessionHistory = gameSessionHistory;
            BackToMenu = new RelayCommand((_) => goBackToMainMenu());

            var sessions = _gameSessionHistory.GetAllGameSessions();
            foreach(var s in sessions)
            {
                GameSessions.Add(s);
            }
        }

    }

}
