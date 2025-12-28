using NR155910155992.MemoGame.Core;
using NR155910155992.MemoGame.Interfaces;
using System.Diagnostics;
using System.Timers;

namespace NR155910155992.MemoGame.BL
{
	internal class GameSessionManager
	{
		private readonly IDataAccessObject _dao;
		private System.Timers.Timer _timer;

		// Game State
		public TimeSpan TimeElapsed { get; private set; }
		private GameMode _gameMode;
		private GameType _gameType;
		private DateTime _date;

		// Match Logic State
		private int _matchedPairsCount = 0;
		private int _totalPairs;
		private int? _firstRevealedCardId = null;
		private bool _isProcessingMismatch = false; // probably should move this logic to UI layer

		// Events
		public event EventHandler<TimeSpan>? TimeUpdated;
		public event EventHandler<int>? CardMatched;
		public event EventHandler? CardMismatched;
		public event EventHandler? GameFinished;

		public bool IsProcessingMismatch => _isProcessingMismatch;

		public GameSessionManager(IDataAccessObject dao)
		{
			_dao = dao;
			_timer = new System.Timers.Timer(1000);
			_timer.Elapsed += TimerElapsed;
			_timer.AutoReset = true;
		}

		public void StartNewSession(GameMode mode, GameType type, int totalPairs)
		{
			_gameMode = mode;
			_gameType = type;
			_totalPairs = totalPairs;
			_date = DateTime.Now;

			// Reset State
			TimeElapsed = TimeSpan.Zero;
			_matchedPairsCount = 0;
			_firstRevealedCardId = null;
			_isProcessingMismatch = false;

			_timer.Start();
		}

		public async Task ProcessCardClick(int clickedCardId)
		{
			if (_isProcessingMismatch)
				return;

			if (_firstRevealedCardId == null)
			{
				_firstRevealedCardId = clickedCardId;
				return;
			}

			if (_firstRevealedCardId == clickedCardId)
			{
				// MATCH
				_matchedPairsCount++;
				CardMatched?.Invoke(this, clickedCardId);
				Debug.WriteLine($"Matched: {clickedCardId}");

				_firstRevealedCardId = null;

				if (_matchedPairsCount >= _totalPairs)
				{
					_timer.Stop();
					GameFinished?.Invoke(this, EventArgs.Empty);
				}
			}
			else
			{
				// MISMATCH
				_isProcessingMismatch = true;
				Debug.WriteLine($"No match: {_firstRevealedCardId} vs {clickedCardId}");

				await Task.Delay(1000);

				CardMismatched?.Invoke(this, EventArgs.Empty);

				_isProcessingMismatch = false;
				_firstRevealedCardId = null;
			}
		}

		public void SaveSession(IEnumerable<IUserProfile> users)
		{
			_timer.Stop();
			_dao.CreateGameSession(_date, TimeElapsed, _gameType, _gameMode, users: users, _totalPairs * 2);
		}

		private void TimerElapsed(object? sender, ElapsedEventArgs e)
		{
			TimeElapsed = TimeElapsed.Add(TimeSpan.FromSeconds(1));
			TimeUpdated?.Invoke(this, TimeElapsed);
		}

		public void Dispose()
		{
			_timer.Stop();
			_timer.Dispose();
		}
	}
}
