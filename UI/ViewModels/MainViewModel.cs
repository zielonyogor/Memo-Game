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
            _currentView = new MenuViewModel(StartGame);
		}

		private void ShowMenu()
		{
			Debug.WriteLine("Returning to menu...");
            CurrentView = new MenuViewModel(StartGame);
		}

		private void StartGame()
		{
			CurrentView = new GameViewModel(_gameManager, ShowMenu);
		}
	}
}
