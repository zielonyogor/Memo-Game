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
		private bool _isProcessingMismatch = false;

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
			// Ignore clicks if we are waiting for a mismatch animation or if game is done
			if (_isProcessingMismatch)
				return;

			// 1. First Card Logic
			if (_firstRevealedCardId == null)
			{
				_firstRevealedCardId = clickedCardId;
				return;
			}

			// 2. Second Card Logic (Match Check)
			if (_firstRevealedCardId == clickedCardId)
			{
				// MATCH
				_matchedPairsCount++;
				CardMatched?.Invoke(this, clickedCardId);
				Debug.WriteLine($"Matched: {clickedCardId}");

				_firstRevealedCardId = null; // Reset for next pair

				// Win Condition
				if (_matchedPairsCount >= _totalPairs)
				{
					_timer.Stop(); // Stop timer immediately
					GameFinished?.Invoke(this, EventArgs.Empty);
				}
			}
			else
			{
				// MISMATCH
				_isProcessingMismatch = true; // Lock input
				Debug.WriteLine($"No match: {_firstRevealedCardId} vs {clickedCardId}");

				// Wait for user to see the cards (UI logic requires delay)
				await Task.Delay(1000);

				CardMismatched?.Invoke(this, EventArgs.Empty);

				_isProcessingMismatch = false; // Unlock input
				_firstRevealedCardId = null;
			}
		}

		public void SaveSession(IEnumerable<IUserProfile> users)
		{
			_timer.Stop();
			_dao.CreateGameSession(_date, TimeElapsed, _gameType, _gameMode, users: users);
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
