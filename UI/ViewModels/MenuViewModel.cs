using System.Windows.Input;

namespace NR155910155992.MemoGame.UI.ViewModels
{
	public class MenuViewModel : ViewModelBase
	{
		public ICommand StartGameCommand { get; }

		public MenuViewModel(Action startGameNavigation)
		{
			StartGameCommand = new RelayCommand((_) => startGameNavigation());
		}
	}
}
