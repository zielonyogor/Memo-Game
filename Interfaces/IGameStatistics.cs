using NR155910155992.MemoGame.Core;

namespace NR155910155992.MemoGame.Interfaces
{
	public interface IGameStatistics
	{
		public int Id { get; }
		public DateTime GameDate { get; }
		public TimeSpan Duration { get; }
		public int CardsUncovered { get; }
		public GameType GameType { get; }

		// public IUserProfile UserProfile { get; }
	}
}
