using Microsoft.Win32;
using NR155910155992.MemoGame.Core;
using NR155910155992.MemoGame.Interfaces;
using NR155910155992.MemoGame.UI.Commands;
using NR155910155992.MemoGame.UI.Services;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace NR155910155992.MemoGame.UI.ViewModels
{
	class CardListViewModel : ViewModelBase
	{
		private readonly IGameManager _gameManager;

		public ICommand BackToMenu { get; }
		public ICommand AddCardCommand { get; }

		public ObservableCollection<CardListItemViewModel> Cards { get; } = new();

		public CardListViewModel(
			IGameManager gameManager,
			INavigationService backToMenuNavigationService)
		{
			_gameManager = gameManager;

			BackToMenu = new RelayCommand(_ => backToMenuNavigationService.Navigate());
			AddCardCommand = new RelayCommand(_ => AddNewCard());

			LoadCards();
		}

		private void LoadCards()
		{
			Cards.Clear();
			foreach (var card in _gameManager.GetAllCards())
			{
				Cards.Add(new CardListItemViewModel(_gameManager, card, this));
			}
		}

		private void AddNewCard()
		{
			var openFileDialog = new OpenFileDialog
			{
				Filter = "Image files (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg|All files (*.*)|*.*",
				Title = "Select an image for the new card"
			};

			if (openFileDialog.ShowDialog() == true)
			{
				try
				{
					string sourcePath = openFileDialog.FileName;
					var newCard = _gameManager.CreateNewCard(sourcePath, "New Card");

					Cards.Add(new CardListItemViewModel(_gameManager, newCard, this));
				}
				catch (Exception ex)
				{
					MessageBox.Show($"Error adding image: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				}
			}
		}

		public void RemoveCard(CardListItemViewModel card)
		{
			if (card != null && Cards.Contains(card))
			{
				Cards.Remove(card);
			}
		}
	}
}
