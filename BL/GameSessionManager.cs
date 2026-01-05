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
		private System.Timers.Timer _timer;

		// Game State
		public TimeSpan TimeElapsed { get; private set; }
		private GameMode _gameMode;
		private GameType _gameType;
		private DateTime _date;

		// Match Logic State
		private int _matchedPairsCount = 0;
		private int _totalPairs;

		public event EventHandler<TimeSpan> TimeUpdated;

		public GameSessionManager(IDataAccessObject dao, IGameStateStore gameStateStore)
		{
			_dao = dao;
            _gameStateStore = gameStateStore;
			_timer = new System.Timers.Timer(1000);
			_timer.Elapsed += TimerElapsed;
			_timer.AutoReset = true;
            
            RestoreSession();
		}

		public void StartNewSession(GameMode mode, GameType type, int totalPairs)
		{
			var state = _gameStateStore.LoadState();
			state.GameMode = mode;
			state.GameType = type;
			state.TotalPairs = totalPairs;
			state.StartTime = DateTime.Now;

			_gameMode = mode;
			_gameType = type;
			_totalPairs = totalPairs;
			_date = DateTime.Now;

			// Reset State
			TimeElapsed = TimeSpan.Zero;
			_matchedPairsCount = 0;
			_timer.Start();
            
			SaveState(state);
		}

        public void RestoreSession()
        {
            var state = _gameStateStore.LoadState();
			_gameMode = state.GameMode;
			_gameType = state.GameType;
			_totalPairs = state.TotalPairs;
			_date = state.StartTime;
			_matchedPairsCount = state.MatchedPairsCount;

			if (state.IsGameActive)
			{
				// if active catch up the time lost between requests
				TimeElapsed = state.TimeElapsed + (DateTime.Now - state.LastUpdatedTime);
				if (!_timer.Enabled)
				{
					_timer.Start();
				}
			}
			else
			{
				TimeElapsed = state.TimeElapsed;
			}
		}

		private void SaveState(GameState state)
		{
			state.GameMode = _gameMode;
			state.GameType = _gameType;
			state.TotalPairs = _totalPairs;
			state.StartTime = _date;
			state.TimeElapsed = TimeElapsed;
			state.LastUpdatedTime = DateTime.Now;
			state.MatchedPairsCount = _matchedPairsCount;
			state.IsGameActive = true;

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
				SaveState(state);
				return boardState;
			}

			if (alreadyChosenCardId == clickedCard.CardId)
			{
				// MATCH
				_matchedPairsCount++;
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
				if (_matchedPairsCount >= _totalPairs)
				{
					_timer.Stop();

					state.IsGameActive = false;
					state.BoardState.IsFinished = true;

					_gameStateStore.SaveState(state);
				}
				else
				{
					SaveState(state);
				}
				return boardState;
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
				SaveState(state);
				return boardState;
			}
		}


		public void SaveSession(IEnumerable<IUserProfile> users)
		{
			_timer.Stop();
            
            var state = _gameStateStore.LoadState();
            state.IsGameActive = false;
            _gameStateStore.SaveState(state);
            
			_dao.CreateGameSession(_date, TimeElapsed, _gameType, _gameMode, users: users, _totalPairs * 2);
		}

        public GameResult GetGameResult()
        {
            return new GameResult(_matchedPairsCount, TimeElapsed);
        }

		private void TimerElapsed(object? sender, ElapsedEventArgs e)
		{
			TimeElapsed = TimeElapsed.Add(TimeSpan.FromSeconds(1));
            var state = _gameStateStore.LoadState();
            state.TimeElapsed = TimeElapsed;
            _gameStateStore.SaveState(state);
            
			TimeUpdated?.Invoke(this, TimeElapsed);
		}
	}
}
