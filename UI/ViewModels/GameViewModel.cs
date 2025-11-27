using System.Windows.Input;

namespace NR155910155992.MemoGame.UI.ViewModels
{
	public class GameViewModel : ViewModelBase
	{
		public ICommand StartGameCommand { get; }

		public GameViewModel(Action goBackToMainMenu)
		{
			// Initialize game state here
			StartGameCommand = new RelayCommand((_) => goBackToMainMenu());
		}
	}
}
