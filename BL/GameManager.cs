using NR155910155992.MemoGame.Core;
using NR155910155992.MemoGame.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;

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
        private readonly IGameStateStore _gameStateStore;

		public GameManager(IConfiguration configuration, IGameStateStore? gameStateStore = null)
		{
			Debug.WriteLine("Initializing GameManager...");
			var loader = new LibraryLoader(configuration);
			_dao = loader.LoadObjectFromLibrary<IDataAccessObject>(LibraryKey.Dao);

            _gameStateStore = gameStateStore ?? new InMemoryGameStateStore();

			_userController = new UserProfileController(_dao);
			_gameBoard = new GameBoard(_dao);
			_historyManager = new GameHistoryManager(_dao);
			_sessionManager = new GameSessionManager(_dao, _gameStateStore);
			_cardManager = new CardManager(_dao);

			_sessionManager.GameFinished += OnSessionFinished;
            
            // Restore user profile
            var state = _gameStateStore.LoadState();
            if (state.CurrentUserProfileId.HasValue)
            {
                var user = _userController.GetAllUserProfiles().FirstOrDefault(u => u.Id == state.CurrentUserProfileId.Value);
                if (user != null)
                {
                    _userController.SetCurrentUserProfile(user);
                }
            }
		}
		public void ResetGame()
		{
			var state = _gameStateStore.LoadState();
			state.IsGameActive = false;
			state.BoardState = new BoardState();
			_gameStateStore.SaveState(state);
		}

		public ICard[,] GetRandomCardsPositionedOnBoard(int rows, int cols)
		{
            var state = _gameStateStore.LoadState();

			// restore prev session if matching board exists
			if (state.IsGameActive && state.BoardState.Rows == rows && state.BoardState.Cols == cols && state.BoardState.Fields != null)
            {
                var board = new ICard[rows, cols];
                var allCards = _dao.GetAllCards().ToDictionary(c => c.Id);
                for (int r = 0; r < rows; r++)
                {
                    for (int c = 0; c < cols; c++)
                    {
                        var field = state.BoardState.Fields[r, c];
                        if (allCards.ContainsKey(field.CardId))
                        {
                            board[r, c] = allCards[field.CardId];
                        }
                    }
                }
                _sessionManager.RestoreSession();
                return board;
            }

			var newBoard = _gameBoard.GenerateBoard(rows, cols);
			// save new board state
			state.BoardState.Rows = rows;
            state.BoardState.Cols = cols;
            state.BoardState.Fields = new BoardState.FieldState[rows, cols];

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    state.BoardState.Fields[r, c] = new BoardState.FieldState()
					{
						CardId = newBoard[r, c]?.Id ?? 0,
						State = ClickResult.Hidden
					};
				}
            }
            state.IsGameActive = false;
            _gameStateStore.SaveState(state);
            
            return newBoard;
		}

		public void StartNewGame(GameMode gameMode, GameType gameType)
		{
			int totalPairs = _gameBoard.TotalPairs;
            var state = _gameStateStore.LoadState();
            
			if(totalPairs == 0)
				totalPairs = state.BoardState.Rows * state.BoardState.Cols / 2;
			if (totalPairs == 0)
				throw new InvalidOperationException("Board not generated yet.");

			_sessionManager.StartNewSession(gameMode, gameType, totalPairs);
		}

		public BoardState OnCardClicked(int row, int col)
		{
			return _sessionManager.ProcessCardClick(row, col);
		}

		private void OnSessionFinished(object? sender, EventArgs e)
		{
			var users = _userController.GetCurrentlyPlayingUsers();
			_sessionManager.SaveSession(users);
		}

		public int GetCardsCount() => _gameBoard.GetTotalCardsCount();
		public IEnumerable<IGameSession> GetAllGameSessionsForCurrentUser() => _historyManager.GetAllGameSessionsForUser(_userController.GetCurrentUserProfile());

		// User profile management
		public IUserProfile? GetCurrentUserProfile() => _userController.GetCurrentUserProfile();
		public void SetCurrentUserProfile(IUserProfile profile) 
        {
            _userController.SetCurrentUserProfile(profile);
            var state = _gameStateStore.LoadState();
            state.CurrentUserProfileId = profile?.Id;
            _gameStateStore.SaveState(state);
        }
		public IEnumerable<IUserProfile> GetAllUserProfiles() => _userController.GetAllUserProfiles();
		public IUserProfile CreateNewUserProfile(string name) => _userController.CreateNewUserProfile(name);
		public void DeleteUserProfile(int userProfileId) => _userController.DeleteUserProfile(userProfileId);
		public void UpdateUserProfile(IUserProfile profile, string name) => _userController.UpdateUserProfile(profile, name);

		// Card management
		public IEnumerable<ICard> GetAllCards() => _cardManager.GetAllCards();
		public ICard GetCardById(int cardId) => _cardManager.GetCardById(cardId);
		public ICard CreateNewCard(string imagePath, string name) => _cardManager.CreateNewCard(imagePath, name);
		public void DeleteCard(int cardId) => _cardManager.DeleteCard(cardId);
		public void UpdateCardName(int cardId, string newName) => _cardManager.UpdateCardName(cardId, newName);
	}
}