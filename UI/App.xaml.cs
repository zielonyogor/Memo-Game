using NR155910155992.MemoGame.Core;
using NR155910155992.MemoGame.Interfaces;
using NR155910155992.MemoGame.UI.ViewModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
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
			var builder = new ConfigurationBuilder()
					.SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
					.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
			IConfiguration config = builder.Build();

			var libraryLoader = new LibraryLoader(config);

			IGameManager gameManager = libraryLoader.LoadObjectFromLibrary<IGameManager>(
				LibraryKey.Bl,
				new object[] { config }
			);

			var mainViewModel = new MainViewModel(gameManager);
			MainWindow mainWindow = new MainWindow();
			mainWindow.DataContext = mainViewModel;
			mainWindow.Show();
		}
	}

}
