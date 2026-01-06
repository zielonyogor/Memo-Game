using NR155910155992.MemoGame.Interfaces;
using NR155910155992.MemoGame.UI.Commands;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace NR155910155992.MemoGame.UI.ViewModels
{
	class UserItemViewModel : ViewModelBase
	{
		private readonly IGameManager _gameManager;
		private readonly UsersViewModel _parent;

		public IUserProfile User { get; }

		public string UserName {
			get => User.UserName;
			set
			{
				if (User.UserName != value)
				{
					User.UserName = value;
					OnPropertyChanged();
				}
			}
		}

		private bool _isEditing;
		public bool IsEditing {
			get => _isEditing;
			set { _isEditing = value; OnPropertyChanged(); }
		}

		private string _editedName;
		public string EditedName {
			get => _editedName;
			set { _editedName = value; OnPropertyChanged(); }
		}

		public ICommand SelectCommand { get; }
		public ICommand DeleteCommand { get; }
		public ICommand StartEditCommand { get; }
		public ICommand ConfirmEditCommand { get; }
		public ICommand CancelEditCommand { get; }

		public UserItemViewModel(
			IUserProfile user,
			IGameManager gameManager,
			UsersViewModel parent)
		{
			User = user;
			_gameManager = gameManager;
			_parent = parent;

			IsEditing = false;
			EditedName = user.UserName;

			SelectCommand = new RelayCommand(_ => Select());
			DeleteCommand = new RelayCommand(_ => Delete());
			StartEditCommand = new RelayCommand(_ => StartEdit());
			ConfirmEditCommand = new RelayCommand(_ => ConfirmEdit());
			CancelEditCommand = new RelayCommand(_ => CancelEdit());
		}

		private void Select()
		{
			_gameManager.SetCurrentUserProfile(User);
			_parent.Back();
		}

		private void Delete()
		{
			if (_gameManager.GetUsersCount() <= 1)
			{
				MessageBox.Show("Cannot delete the only user left.", "Delete Blocked", MessageBoxButton.OK, MessageBoxImage.Warning);
				return;
			}
			_gameManager.DeleteUserProfile(User.Id);
			_parent.RemoveUser(this);
		}

		private void StartEdit()
		{
			EditedName = UserName;
			IsEditing = true;
			Debug.WriteLine("Started editing");
		}

		private void ConfirmEdit()
		{
			if (!string.IsNullOrWhiteSpace(EditedName))
				UserName = EditedName.Trim();

			IsEditing = false;
			_gameManager.UpdateUserProfile(User, EditedName.Trim());
		}

		private void CancelEdit()
		{
			IsEditing = false;
		}
	}

}
