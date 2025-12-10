using NR155910155992.MemoGame.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NR155910155992.MemoGame.UI.ViewModels
{
	class UsersViewModel : ViewModelBase
	{
		private readonly IGameManager _gameManager;
		private readonly Action _returnToMenu;

		public ObservableCollection<IUserProfile> Users { get; set; }

		private string _newUserName;
		public string NewUserName {
			get => _newUserName;
			set { _newUserName = value; OnPropertyChanged(); }
		}

		public ICommand SelectUserCommand { get; }
		public ICommand AddUserCommand { get; }
		public ICommand BackCommand { get; }

		public UsersViewModel(IGameManager gameManager, Action returnToMenu)
		{
			_gameManager = gameManager;
			_returnToMenu = returnToMenu;
			Users = new ObservableCollection<IUserProfile>(_gameManager.GetAllUserProfiles());
			SelectUserCommand = new RelayCommand<IUserProfile>((user) => SelectUser(user));
			AddUserCommand = new RelayCommand((_) => AddUser());
			BackCommand = new RelayCommand((_) => Back());
		}

		private void SelectUser(IUserProfile user)
		{
			_gameManager.SetCurrentUserProfile(user);
			_returnToMenu();
		}

		private void AddUser()
		{
			if (string.IsNullOrWhiteSpace(NewUserName))
				return;
			//var newUser = _gameManager.CreateUserProfile(NewUserName.Trim());
			//Users.Add(newUser);
			NewUserName = string.Empty;
		}

		private void Back()
		{
			_returnToMenu();
		}
	}
}
