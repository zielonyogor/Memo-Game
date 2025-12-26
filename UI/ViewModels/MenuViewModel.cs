using NR155910155992.MemoGame.Interfaces;
using NR155910155992.MemoGame.UI.Commands;
using NR155910155992.MemoGame.UI.Services;
using System.Windows.Input;

namespace NR155910155992.MemoGame.UI.ViewModels
{
	public class MenuViewModel : ViewModelBase
	{
		private IGameManager _gameManager;
		private readonly INavigationService _gameNavigationService;
		private readonly INavigationService _sessionHistoryNavigationService;
		private readonly INavigationService _usersNavigationService;

		public string UserText => $"Current User: {_gameManager.GetCurrentUserProfile()?.UserName ?? "Guest"}";

		public ICommand StartGameCommand { get; }
		public ICommand ShowUserSelection { get; }
		public ICommand ShowSessionHistory { get; }

        public MenuViewModel(
			IGameManager gameManager, 
			INavigationService gameNavigationService,
			INavigationService sessionHistoryNavigationService,
			INavigationService usersNavigationService
		)
		{
			_gameManager = gameManager;
			_gameNavigationService = gameNavigationService;
			_sessionHistoryNavigationService = sessionHistoryNavigationService;
			_usersNavigationService = usersNavigationService;

			StartGameCommand = new RelayCommand((_) => startGameNavigation());
			ShowSessionHistory = new RelayCommand((_) => startShowingSessions());
			ShowUserSelection = new RelayCommand((_) => showUserSelection());
		}

		private void startGameNavigation()
		{
			_gameNavigationService.Navigate();
		}

		private void startShowingSessions()
		{
			_sessionHistoryNavigationService.Navigate();
		}

		private void showUserSelection()
		{
			_usersNavigationService.Navigate();
		}
	}
}
