using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NR155910155992.MemoGame.Core;
using NR155910155992.MemoGame.Interfaces;
using NR155910155992.MemoGame.UI.Models;
using NR155910155992.MemoGame.UI.Services;
using NR155910155992.MemoGame.UI.Stores;
using NR155910155992.MemoGame.UI.ViewModels;
using System.Diagnostics;
using System.Windows;

namespace NR155910155992.MemoGame.UI
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		private IServiceProvider _serviceProvider;

		public App()
		{
			// Load business logic
			var builder = new ConfigurationBuilder()
					.SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
					.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
			IConfiguration config = builder.Build();

			var libraryLoader = new LibraryLoader(config);

			IGameManager gameManager = libraryLoader.LoadObjectFromLibrary<IGameManager>(
				LibraryKey.Bl,
				new object[] { config, null }
			);


			var services = new ServiceCollection();

			services.AddSingleton<IGameManager>(gameManager);

			services.AddSingleton<NavigationStore>();
			services.AddSingleton<ModalNavigationStore>();

			services.AddSingleton<MainViewModel>();

			services.AddTransient<MenuViewModel>(CreateMenuViewModel);

			services.AddTransient<GameSettingsViewModel>(s =>
				new GameSettingsViewModel(
					s.GetRequiredService<IGameManager>(),
					new ParameterNavigationService<GameSettings, GameViewModel>(
						s.GetRequiredService<NavigationStore>(),
						s.GetRequiredService<Func<GameSettings, GameViewModel>>()
					),
					new CloseModalNavigationService(s.GetRequiredService<ModalNavigationStore>())
				)
			);

			services.AddSingleton<GameFinishedViewModel>(s =>
				new GameFinishedViewModel(
					s.GetRequiredService<IGameManager>(),
					CreateGameFinishedBackToMenuService(s)
				)
			);

			services.AddSingleton<Func<GameSettings, GameViewModel>>(s =>
				settings => new GameViewModel(
					settings,
					s.GetRequiredService<IGameManager>(),
					CreateBackToMenuNavidationService(s),
					CreateGameFinishedNavigationService(s)
				)
			);

			services.AddTransient<GameSessionViewModel>(s =>
				new GameSessionViewModel(
					s.GetRequiredService<IGameManager>(),
					CreateBackToMenuNavidationService(s)
				)
			);

			services.AddTransient<UsersViewModel>(s =>
				new UsersViewModel(
					s.GetRequiredService<IGameManager>(),
					CreateBackToMenuNavidationService(s)
					)
			);

			services.AddTransient<CardListViewModel>(s =>
				new CardListViewModel(
					s.GetRequiredService<IGameManager>(),
					CreateBackToMenuNavidationService(s)
				)
			);

			services.AddSingleton<MainWindow>();

			_serviceProvider = services.BuildServiceProvider();
		}

		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);

			new NavigationService<MenuViewModel>(
				_serviceProvider.GetRequiredService<NavigationStore>(),
				() => _serviceProvider.GetRequiredService<MenuViewModel>())
				.Navigate();

			MainWindow mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
			mainWindow.Show();
		}

		private MenuViewModel CreateMenuViewModel(IServiceProvider s)
		{
			return new MenuViewModel(
				s.GetRequiredService<IGameManager>(),
				new ModalNavigationService<GameSettingsViewModel>(s.GetRequiredService<ModalNavigationStore>(), () => s.GetRequiredService<GameSettingsViewModel>()),
				new NavigationService<GameSessionViewModel>(s.GetRequiredService<NavigationStore>(), () => s.GetRequiredService<GameSessionViewModel>()),
				new NavigationService<UsersViewModel>(s.GetRequiredService<NavigationStore>(), () => s.GetRequiredService<UsersViewModel>()),
				new NavigationService<CardListViewModel>(s.GetRequiredService<NavigationStore>(), () => s.GetRequiredService<CardListViewModel>())
			);
		}

		private INavigationService CreateGameFinishedBackToMenuService(IServiceProvider s)
		{
			return new CloseModalAndNavigateService(
				new CloseModalNavigationService(
					s.GetRequiredService<ModalNavigationStore>()
				),

				new NavigationService<MenuViewModel>(
					s.GetRequiredService<NavigationStore>(),
					() => s.GetRequiredService<MenuViewModel>()
				)
			);
		}

		private NavigationService<MenuViewModel> CreateBackToMenuNavidationService(IServiceProvider s)
		{
			return new NavigationService<MenuViewModel>(
				s.GetRequiredService<NavigationStore>(),
				() => s.GetRequiredService<MenuViewModel>());
		}

		private ModalNavigationService<GameFinishedViewModel> CreateGameFinishedNavigationService(IServiceProvider s)
		{
			return new ModalNavigationService<GameFinishedViewModel>(
				s.GetRequiredService<ModalNavigationStore>(),
				() => s.GetRequiredService<GameFinishedViewModel>());
		}
	}
}
