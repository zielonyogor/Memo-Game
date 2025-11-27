using System.Windows.Input;

namespace NR155910155992.MemoGame.UI.ViewModels
{
	public class MenuViewModel : ViewModelBase
	{
		public string UserText { get; set; } = "Current User: Guest";

		public ICommand StartGameCommand { get; }
		public ICommand ChangeCurrentUser { get; }

		public MenuViewModel(Action startGameNavigation)
		{
			StartGameCommand = new RelayCommand((_) => startGameNavigation());
			ChangeCurrentUser = new RelayCommand((_) => ChangeUser());
		}

		private void ChangeUser()
		{
			// Logic to change the current user profile
			// This could involve navigating to a user selection view
		}
	}
}
