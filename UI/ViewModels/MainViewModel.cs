using NR155910155992.MemoGame.Interfaces;
using System.Diagnostics;

namespace NR155910155992.MemoGame.UI.ViewModels
{
	public class MainViewModel : ViewModelBase
	{
		private readonly IGameManager _gameManager;
        private ViewModelBase _currentView;
		public ViewModelBase CurrentView {
			get => _currentView;
			set { _currentView = value; OnPropertyChanged(); }
		}

		public MainViewModel(IGameManager gameManager)
		{
			_gameManager = gameManager;
			ShowMenu();
		}

		private void ShowMenu()
		{
            CurrentView = new MenuViewModel(_gameManager, StartGame, ShowSessionHistory, ShowUserSelection);
		}

		private void StartGame()
		{
			CurrentView = new GameViewModel(_gameManager, ShowMenu);
		}

		private void ShowSessionHistory()
		{
            CurrentView = new GameSessionViewModel(_gameManager, ShowMenu);
        }

		private void ShowUserSelection()
		{
			CurrentView = new UsersViewModel(_gameManager, ShowMenu);
		}

	}
}
