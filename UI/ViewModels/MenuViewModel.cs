using NR155910155992.MemoGame.Interfaces;
using System.Windows.Input;

namespace NR155910155992.MemoGame.UI.ViewModels
{
	public class MenuViewModel : ViewModelBase
	{
		private IGameManager _gameManager;

		public string UserText => $"Current User: {_gameManager.GetCurrentUserProfile()?.UserName ?? "Guest"}";

		public ICommand StartGameCommand { get; }
		public ICommand ShowUserSelection { get; }
		public ICommand ShowSessionHistory { get; }
        public MenuViewModel(IGameManager gameManager, Action startGameNavigation, Action startShowingSessions, Action showUserSelection)
		{
			_gameManager = gameManager;
			StartGameCommand = new RelayCommand((_) => startGameNavigation());
			ShowSessionHistory = new RelayCommand((_) => startShowingSessions());
			ShowUserSelection = new RelayCommand((_) => showUserSelection());
		}
	}
}
