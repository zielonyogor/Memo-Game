using NR155910155992.MemoGame.Interfaces;
using NR155910155992.MemoGame.UI.Commands;
using NR155910155992.MemoGame.UI.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace NR155910155992.MemoGame.UI.ViewModels
{
    public class GameSessionViewModel : ViewModelBase
    {
		private readonly IGameManager _gameManager;

		public ICommand BackToMenu { get; }

		public ObservableCollection<GameSessionItemViewModel> GameSessions { get; } = new();

		public GameSessionViewModel(
			IGameManager gameManager,
			INavigationService backToMenuNavigationService)
		{
			_gameManager = gameManager;
			BackToMenu = new RelayCommand(_ => backToMenuNavigationService.Navigate());

			foreach (var session in _gameManager.GetAllGameSessions())
			{
				GameSessions.Add(new GameSessionItemViewModel(session));
			}
		}
	}
}
