using NR155910155992.MemoGame.Core;
using NR155910155992.MemoGame.Interfaces;
using Microsoft.Extensions.Configuration;

namespace NR155910155992.MemoGame.BL
{
	public class GameManager : IGameManager
	{
		private readonly IDataAccessObject _dao;
		private readonly UserProfileController _userController;
		private readonly GameBoard _gameBoard;
		private readonly GameHistoryManager _historyManager;
		private readonly GameSessionManager _sessionManager;
		private readonly CardManager _cardManager;

		public event EventHandler<int>? CardsMatched;
		public event EventHandler? CardsMismatched;
		public event EventHandler? GameFinished;
		public event EventHandler<TimeSpan>? TimeUpdated;

		public bool IsShowingChoosenCards => _sessionManager.IsProcessingMismatch;

		public GameManager(IConfiguration configuration)
		{
			var loader = new LibraryLoader(configuration);
			_dao = loader.LoadObjectFromLibrary<IDataAccessObject>(LibraryKey.Dao);

			_userController = new UserProfileController(_dao);
			_gameBoard = new GameBoard(_dao);
			_historyManager = new GameHistoryManager(_dao);
			_sessionManager = new GameSessionManager(_dao);
			_cardManager = new CardManager(_dao);

			_sessionManager.TimeUpdated += (s, e) => TimeUpdated?.Invoke(this, e);
			_sessionManager.CardMatched += (s, id) => CardsMatched?.Invoke(this, id);
			_sessionManager.CardMismatched += (s, e) => CardsMismatched?.Invoke(this, EventArgs.Empty);
			_sessionManager.GameFinished += OnSessionFinished;
		}

		public ICard[,] GetRandomCardsPositionedOnBoard(int rows, int cols)
		{
			return _gameBoard.GenerateBoard(rows, cols);
		}

		public void StartNewGame(GameMode gameMode, GameType gameType)
		{
			int totalPairs = _gameBoard.TotalPairs;

			if (totalPairs == 0)
				throw new InvalidOperationException("Board not generated yet.");

			_sessionManager.StartNewSession(gameMode, gameType, totalPairs);
		}

		public async Task OnCardClicked(int clickedCardId)
		{
			await _sessionManager.ProcessCardClick(clickedCardId);
		}

		private void OnSessionFinished(object? sender, EventArgs e)
		{
			var users = _userController.GetCurrentlyPlayingUsers();
			_sessionManager.SaveSession(users);
			GameFinished?.Invoke(this, EventArgs.Empty);
		}

		public IEnumerable<ICard> GetRandomSetOfCards(int count) => _gameBoard.GetRandomSetOfCards(count);
		public int GetCardsCount() => _gameBoard.GetTotalCardsCount();
		public IEnumerable<IGameSession> GetAllGameSessions() => _historyManager.GetAllGameSessions();

		public IUserProfile? GetCurrentUserProfile() => _userController.GetCurrentUserProfile();
		public void SetCurrentUserProfile(IUserProfile profile) => _userController.SetCurrentUserProfile(profile);
		public IEnumerable<IUserProfile> GetAllUserProfiles() => _userController.GetAllUserProfiles();
		public IUserProfile CreateNewUserProfile(string name) => _userController.CreateNewUserProfile(name);
		public void DeleteUserProfile(IUserProfile profile) => _userController.DeleteUserProfile(profile);
		public void UpdateUserProfile(IUserProfile profile, string name) => _userController.UpdateUserProfile(profile, name);

		public IEnumerable<ICard> GetAllCards() => _cardManager.GetAllCards();
		public ICard CreateNewCard(string imagePath, string name) => _cardManager.CreateNewCard(imagePath, name);
		public void DeleteCard(ICard card) => _cardManager.DeleteCard(card);
		public void UpdateCardName(ICard card, string newName) => _cardManager.UpdateCardName(card, newName);
	}
}