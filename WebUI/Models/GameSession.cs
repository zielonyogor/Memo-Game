using NR155910155992.MemoGame.Core;
using NR155910155992.MemoGame.Interfaces;

namespace NR155910155992.MemoGame.WebUI.Models
{
	public class GameSession: IGameSession
	{
		public int Id { get; set; }
		public DateTime GameDate { get; set; }
		public TimeSpan Duration { get; set; }
		public GameType GameType { get; set; }
		public GameMode GameMode { get; set; }

		public IEnumerable<IPlayerGameResult> PlayerResults { get; }

		public IEnumerable<string> Players { get; set; }

		public GameSession(IGameSession gameSession)
		{
			Id = gameSession.Id;
			GameDate = gameSession.GameDate;
			Duration = gameSession.Duration;
			GameType = gameSession.GameType;
			GameMode = gameSession.GameMode;
			PlayerResults = gameSession.PlayerResults;
			Players = gameSession.PlayerResults.Select(pr => pr.User.UserName).ToList();
		}
	}
}
