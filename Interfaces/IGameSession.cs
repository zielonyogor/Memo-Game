using NR155910155992.MemoGame.Core;

namespace NR155910155992.MemoGame.Interfaces
{
	public interface IGameSession
	{
		public int Id { get; }
		public DateTime GameDate { get; set; }
		public TimeSpan Duration { get; set; }
		public GameType GameType { get; set; }
		public GameMode GameMode { get; set; }

		public IEnumerable<IPlayerGameResult> PlayerResults { get; }
	}
}
