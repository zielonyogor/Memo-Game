using NR155910155992.MemoGame.Core;
using NR155910155992.MemoGame.Interfaces;
using NR155910155992.MemoGame.UI.Commands;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace NR155910155992.MemoGame.UI.ViewModels
{
    class CardListItemViewModel : ViewModelBase
    {
		private readonly IGameManager _gameManager;
		private readonly CardListViewModel _parent;

		private ICard Card { get; }

        public string ImagePath => ImageUtility.ResolvePath(Card.ImagePath);
		public string Name {
			get => Card.Name;
			set
			{
				if (Card.Name != value)
				{
					Card.Name = value;
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

		public ICommand DeleteCommand { get; }
		public ICommand StartEditCommand { get; }
		public ICommand ConfirmEditCommand { get; }
		public ICommand CancelEditCommand { get; }

		public CardListItemViewModel(IGameManager gameManager, ICard card, CardListViewModel parent)
        {
            _gameManager = gameManager;
            _parent = parent;
            Card = card;

			IsEditing = false;
			EditedName = card.Name;

			DeleteCommand = new RelayCommand(_ => Delete());
			StartEditCommand = new RelayCommand(_ => StartEdit());
			ConfirmEditCommand = new RelayCommand(_ => ConfirmEdit());
			CancelEditCommand = new RelayCommand(_ => CancelEdit());
		}

		private void Delete()
		{
			var result = MessageBox.Show($"Are you sure you want to delete '{Card.Name}'?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
			if (result == MessageBoxResult.Yes)
			{
				_gameManager.DeleteCard(Card);
				_parent.RemoveCard(this);
			}
		}

		private void StartEdit()
		{
			EditedName = Name;
			IsEditing = true;
			Debug.WriteLine("Started editing card name");
		}

		private void ConfirmEdit()
		{
			if (!string.IsNullOrWhiteSpace(EditedName))
				Name = EditedName.Trim();

			IsEditing = false;
			_gameManager.UpdateCardName(Card, EditedName.Trim());
		}

		private void CancelEdit()
		{
			IsEditing = false;
		}
	}
}
