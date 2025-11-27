using NR155910155992.MemoGame.Core;
using NR155910155992.MemoGame.Interfaces;
using NR155910155992.MemoGame.UI.ViewModels;
using System.Windows;

namespace NR155910155992.MemoGame.UI
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{

		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);

			// Load business logic
			IGameManager gameManager = LibraryLoader.LoadObjectFromLibrary<IGameManager>(LibraryKey.Bl);

			var mainViewModel = new MainViewModel(gameManager);
			MainWindow mainWindow = new MainWindow();
			mainWindow.DataContext = mainViewModel;
			mainWindow.Show();
		}
	}

}
