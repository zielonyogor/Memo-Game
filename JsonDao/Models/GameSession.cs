using NR155910155992.MemoGame.Core;
using NR155910155992.MemoGame.Interfaces;

namespace NR155910155992.MemoGame.JsonDao.Models
{
	internal class GameSession : IGameSession
	{
		public int Id { get; set; }
		public DateTime GameDate { get; set; }
		public TimeSpan Duration { get; set; }
		public GameType GameType { get; set; }
		public GameMode GameMode { get; set; }

		public List<PlayerGameResult> PlayerResultsConcrete { get; set; } = new List<PlayerGameResult>();

		[System.Text.Json.Serialization.JsonIgnore]
		public IEnumerable<IPlayerGameResult> PlayerResults => PlayerResultsConcrete;
	}
}
