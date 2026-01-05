using NR155910155992.MemoGame.Interfaces;
using NR155910155992.MemoGame.UI.Commands;
using NR155910155992.MemoGame.UI.Services;
using System.Windows.Input;

namespace NR155910155992.MemoGame.UI.ViewModels
{
    public class GameFinishedViewModel : ViewModelBase
    {
		private readonly IGameManager _gameManager;

		public int TotalPairs { get; }
		public string TimeFormatted { get; }

		public ICommand BackToMenu { get; }

		public GameFinishedViewModel(
			IGameManager gameManager,
			INavigationService backToMenuNavigationService)
		{
			_gameManager = gameManager;

			var result = _gameManager.GetCurrentGameResult();
			TotalPairs = result.TotalPairs;
			TimeFormatted = result.ElapsedTime.ToString(@"mm\:ss");

			BackToMenu = new RelayCommand(_ => backToMenuNavigationService.Navigate());
		}
	}
}
