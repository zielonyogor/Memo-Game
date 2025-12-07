using NR155910155992.MemoGame.Interfaces;
using System.Diagnostics;

namespace NR155910155992.MemoGame.UI.ViewModels
{
	public class MainViewModel : ViewModelBase
	{
		private readonly IGameManager _gameManager;
		private readonly IGameSessionHistory _gameSessionHistory;
        private ViewModelBase _currentView;
		public ViewModelBase CurrentView {
			get => _currentView;
			set { _currentView = value; OnPropertyChanged(); }
		}

		public MainViewModel(IGameManager gameManager, IGameSessionHistory gameSessionHistory)
		{
			_gameManager = gameManager;
			_gameSessionHistory = gameSessionHistory;
            _currentView = new MenuViewModel(StartGame, ShowSessionHistory);
		}

		private void ShowMenu()
		{
			Debug.WriteLine("Returning to menu...");
            CurrentView = new MenuViewModel(StartGame, ShowSessionHistory);
		}

		private void StartGame()
		{
			CurrentView = new GameViewModel(_gameManager, ShowMenu);
		}

		private void ShowSessionHistory()
		{
            CurrentView = new GameSessionViewModel(_gameSessionHistory, ShowMenu);
        }
    }
}
