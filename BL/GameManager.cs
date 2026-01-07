using NR155910155992.MemoGame.Core;
using NR155910155992.MemoGame.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;

namespace NR155910155992.MemoGame.BL
{
	public class GameManager : IGameManager
	{
		private readonly IDataAccessObject _dao;
		private readonly UserProfileManager _userController;
		private readonly GameBoard _gameBoard;
		private readonly GameHistoryManager _historyManager;
		private readonly GameSessionManager _sessionManager;
		private readonly CardManager _cardManager;
        private readonly IGameStateStore _gameStateStore;

		public GameManager(IConfiguration configuration, IGameStateStore? gameStateStore = null)
		{
			var loader = new LibraryLoader(configuration);
			_dao = loader.LoadObjectFromLibrary<IDataAccessObject>(LibraryKey.Dao);

            _gameStateStore = gameStateStore ?? new InMemoryGameStateStore();

			_userController = new UserProfileManager(_dao);
			_gameBoard = new GameBoard(_dao);
			_historyManager = new GameHistoryManager(_dao);
			_sessionManager = new GameSessionManager(_dao, _gameStateStore);
			_cardManager = new CardManager(_dao);

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
			state.MatchedPairsCount = 0;
			state.TimeElapsed = TimeSpan.Zero;
			_gameStateStore.SaveState(state);
		}

		public ICard[,] GetRandomCardsPositionedOnBoard(int rows, int cols)
		{
            var state = _gameStateStore.LoadState();	

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

		private ICard[,] ReconstructBoard(int rows, int cols, GameState state)
		{
			var board = new ICard[rows, cols];
			var allCards = _cardManager.GetAllCards().ToDictionary(c => c.Id);

			for (int r = 0; r < rows; r++)
			{
				for (int c = 0; c < cols; c++)
				{
					var id = state.BoardState.Fields[r, c].CardId;
					if (allCards.ContainsKey(id))
					{
						board[r, c] = allCards[id];
					}
				}
			}
			return board;
		}

		public void StartNewGame(GameMode gameMode, GameType gameType)
		{
            var state = _gameStateStore.LoadState();
			int totalPairs = _gameBoard.CalculatePairs(
				 state.BoardState.Rows,
				 state.BoardState.Cols
			);

			_sessionManager.StartNewSession(gameMode, gameType, totalPairs);
		}

		public BoardState OnCardClicked(int row, int col)
		{
			var boardState = _sessionManager.ProcessCardClick(row, col);
			// Save session if finished
			if (boardState.IsFinished)
			{
				var currentUser = GetCurrentUserProfile();

				var users = new List<IUserProfile>();
				if (currentUser != null)
				{
					users.Add(currentUser);
				}
				_sessionManager.SaveSession(users);
			}

			return boardState;
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
		public int GetUsersCount() => _userController.GetAllUserProfiles().Count();

		// Card management
		public IEnumerable<ICard> GetAllCards() => _cardManager.GetAllCards();
		public ICard GetCardById(int cardId) => _cardManager.GetCardById(cardId);
		public ICard CreateNewCard(string imagePath, string name) => _cardManager.CreateNewCard(imagePath, name);
		public ICard CreateNewCard(Stream fileStream, string fileName, string name) => _cardManager.CreateNewCard(fileStream, fileName, name);
		public void DeleteCard(int cardId) => _cardManager.DeleteCard(cardId);
		public void UpdateCardName(int cardId, string newName) => _cardManager.UpdateCardName(cardId, newName);

        public TimeSpan GetTimeElapsed() => _sessionManager.GetCurrentDuration();
		public GameResult GetCurrentGameResult() => _sessionManager.GetGameResult();
	}
}