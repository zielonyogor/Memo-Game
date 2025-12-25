using NR155910155992.MemoGame.UI.Commands;
using NR155910155992.MemoGame.UI.Models;
using NR155910155992.MemoGame.UI.Services;
using System.Windows.Input;

namespace NR155910155992.MemoGame.UI.ViewModels
{
    public class GameFinishedViewModel : ViewModelBase
    {
		public int TotalPairs { get; }
		public string TimeFormatted { get; }

		public ICommand BackToMenu { get; }

		public GameFinishedViewModel(
			GameResult result,
			INavigationService backToMenuNavigationService)
		{
			TotalPairs = result.TotalPairs;
			TimeFormatted = result.ElapsedTime.ToString(@"mm\:ss");

			BackToMenu = new RelayCommand(_ => backToMenuNavigationService.Navigate());
		}
	}
}
