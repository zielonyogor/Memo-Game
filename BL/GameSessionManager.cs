using NR155910155992.MemoGame.Core;
using NR155910155992.MemoGame.Interfaces;
using System.Diagnostics;
using System.Timers;

namespace NR155910155992.MemoGame.BL
{
	internal class GameSessionManager
	{
		private readonly IDataAccessObject _dao;
        private readonly IGameStateStore _gameStateStore;

		public GameSessionManager(IDataAccessObject dao, IGameStateStore gameStateStore)
		{
			_dao = dao;
            _gameStateStore = gameStateStore;
		}

		public void StartNewSession(GameMode mode, GameType type, int totalPairs)
		{
			var state = _gameStateStore.LoadState();

			state.GameMode = mode;
			state.GameType = type;
			state.TotalPairs = totalPairs;

			state.StartTime = DateTime.Now;
			state.LastUpdatedTime = DateTime.Now;
			state.TimeElapsed = TimeSpan.Zero;

			state.IsGameActive = true;
			state.MatchedPairsCount = 0;

			SaveState(state);
		}

		private void SaveState(GameState state)
		{
			if (state.IsGameActive)
			{
				state.TimeElapsed += (DateTime.Now - state.LastUpdatedTime);
				state.LastUpdatedTime = DateTime.Now;
			}
			_gameStateStore.SaveState(state);
		}

		public BoardState ProcessCardClick(int row, int col)
		{
			var state = _gameStateStore.LoadState();
			var boardState = state.BoardState;

			var clickedCard = boardState.Fields[row, col];

			if (clickedCard.State == ClickResult.Match || clickedCard.State == ClickResult.Null)
			{
				return boardState; // Ignore clicks on matched or already revealed cards, nearly impossible, jsut a sefeguard
			}

			// if there is first card ClickResult.FirstCard revealed
			var alreadyChosenCardId = boardState.Fields.Cast<BoardState.FieldState>()
				.FirstOrDefault(f => f.State == ClickResult.FirstCard)?.CardId;

			if (alreadyChosenCardId == null)
			{
				Debug.WriteLine($"First card chosen: {clickedCard.CardId}");
				boardState.Fields[row, col].State = ClickResult.FirstCard;
			}
			else if (alreadyChosenCardId == clickedCard.CardId)
			{
				// MATCH
				state.MatchedPairsCount++;
				boardState.Fields[row, col].State = ClickResult.Match;
				// Find the first card and mark it as matched too
				for (int r = 0; r < boardState.Rows; r++)
				{
					for (int c = 0; c < boardState.Cols; c++)
					{
						if (boardState.Fields[r, c].CardId == alreadyChosenCardId && boardState.Fields[r, c].State == ClickResult.FirstCard)
						{
							boardState.Fields[r, c].State = ClickResult.Match;
						}
					}
				}
				Debug.WriteLine($"Matched: {clickedCard.CardId}");
				if (state.MatchedPairsCount >= state.TotalPairs)
				{
					state.IsGameActive = false;
					state.BoardState.IsFinished = true;
				}
			}
			else
			{
				// MISMATCH
				Debug.WriteLine($"No match: {alreadyChosenCardId} vs {clickedCard.CardId}");
				boardState.Fields[row, col].State = ClickResult.Hidden;
				// Find the first card and hide it again
				for (int r = 0; r < boardState.Rows; r++)
				{
					for (int c = 0; c < boardState.Cols; c++)
					{
						if (boardState.Fields[r, c].CardId == alreadyChosenCardId && boardState.Fields[r, c].State == ClickResult.FirstCard)
						{
							boardState.Fields[r, c].State = ClickResult.Hidden;
						}
					}
				}
			}
			SaveState(state);
			return boardState;
		}

		public void RestoreSession()
		{
			var state = _gameStateStore.LoadState();
			state.IsGameActive = true;
			state.LastUpdatedTime = DateTime.Now;
			_gameStateStore.SaveState(state);
		}

		public void SaveSession(IEnumerable<IUserProfile> users)
		{
			var state = _gameStateStore.LoadState();

			state.IsGameActive = false;
			state.TimeElapsed = GetCurrentDuration();
			_gameStateStore.SaveState(state);

			_dao.CreateGameSession(
				state.StartTime,
				state.TimeElapsed,
				state.GameType,
				state.GameMode,
				users,
				state.TotalPairs * 2
			);
		}

		public GameResult GetGameResult()
		{
			var state = _gameStateStore.LoadState();
			return new GameResult(state.MatchedPairsCount, GetCurrentDuration());
		}

		public TimeSpan GetCurrentDuration()
		{
			var state = _gameStateStore.LoadState();

			if (!state.IsGameActive)
				return state.TimeElapsed;
			return state.TimeElapsed + (DateTime.Now - state.LastUpdatedTime);
		}
	}
	
}
