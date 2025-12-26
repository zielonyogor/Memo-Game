using NR155910155992.MemoGame.Interfaces;
using NR155910155992.MemoGame.UI.Commands;
using NR155910155992.MemoGame.UI.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace NR155910155992.MemoGame.UI.ViewModels
{
	class UsersViewModel : ViewModelBase
	{
		private readonly IGameManager _gameManager;
		private readonly INavigationService _menuNavigationService;

		public ObservableCollection<UserItemViewModel> Users { get; }

		private string _newUserName;
		public string NewUserName {
			get => _newUserName;
			set { _newUserName = value; OnPropertyChanged(); }
		}

		public ICommand SelectUserCommand { get; }
		public ICommand AddUserCommand { get; }
		public ICommand BackCommand { get; }

		public UsersViewModel(IGameManager gameManager, INavigationService menuNavigationService)
		{
			_gameManager = gameManager;
			_menuNavigationService = menuNavigationService;

			Users = new ObservableCollection<UserItemViewModel>(
				_gameManager.GetAllUserProfiles()
					.Select(u => new UserItemViewModel(u, _gameManager, this))
			);

			AddUserCommand = new RelayCommand((_) => AddUser());
			BackCommand = new RelayCommand((_) => Back());
		}

		private void AddUser()
		{
			if (string.IsNullOrWhiteSpace(NewUserName))
				return;

			var newUser = _gameManager.CreateNewUserProfile(NewUserName.Trim());
			var newUserItem = new UserItemViewModel(newUser, _gameManager, this);
			Users.Add(newUserItem);

			NewUserName = string.Empty;
		}

		private void Back()
		{
			_menuNavigationService.Navigate();
		}

		public void RemoveUser(UserItemViewModel user)
		{
			Users.Remove(user);
		}

		public void BackToMenu()
		{
			_menuNavigationService.Navigate();
		}

	}
}
