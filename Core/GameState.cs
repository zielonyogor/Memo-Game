namespace NR155910155992.MemoGame.Core
{
	public class GameState
	{
		public bool IsGameActive { get; set; }
		public GameMode GameMode { get; set; }
		public GameType GameType { get; set; }
		public int TotalPairs { get; set; }
		public DateTime StartTime { get; set; }
		public TimeSpan TimeElapsed { get; set; }
		public DateTime LastUpdatedTime { get; set; }


		// Board State
		public int MatchedPairsCount { get; set; }
		public BoardState BoardState { get; set; } = new BoardState();

		// User
		public int? CurrentUserProfileId { get; set; }
	}
}
