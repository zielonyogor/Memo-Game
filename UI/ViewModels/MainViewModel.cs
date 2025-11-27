using System.Windows.Input;

namespace NR155910155992.MemoGame.UI.ViewModels
{
	public class MainViewModel : ViewModelBase
	{
		private ViewModelBase _currentView;
		public ViewModelBase CurrentView {
			get => _currentView;
			set { _currentView = value; OnPropertyChanged(); }
		}

		public MainViewModel()
		{
			ShowMenu();
		}

		private void ShowMenu()
		{
			CurrentView = new MenuViewModel(StartGame);
		}

		private void StartGame()
		{
			CurrentView = new GameViewModel(ShowMenu);
		}
	}
}
