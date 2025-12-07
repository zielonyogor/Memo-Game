using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using NR155910155992.MemoGame.Core;
using NR155910155992.MemoGame.Interfaces;
using NR155910155992.MemoGame.UI.ViewModels;
using System;
using System.Windows;

namespace NR155910155992.MemoGame.UI
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		private IServiceProvider _serviceProvider;
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
            IGameSessionHistory gameSessionHistory = libraryLoader.LoadObjectFromLibrary<IGameSessionHistory>(
                LibraryKey.Bl,
                new object[] { config }
            );

            var services = new ServiceCollection();
			services.AddSingleton<MainWindow>();
			services.AddSingleton<MainViewModel>();
			services.AddSingleton<IGameManager>(gameManager);
            services.AddSingleton<IGameSessionHistory>(gameSessionHistory);


            _serviceProvider = services.BuildServiceProvider();

            //var mainViewModel = new MainViewModel(gameManager);
			//MainWindow mainWindow = new MainWindow();
			MainWindow mainWindow = _serviceProvider.GetRequiredService<MainWindow>();

            //mainWindow.DataContext = mainViewModel;
			mainWindow.Show();
		}

	}

}
