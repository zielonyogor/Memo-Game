
namespace NR155910155992.MemoGame.UI.Models
{
	public sealed class GameResult
	{
		public int TotalPairs { get; }
		public TimeSpan ElapsedTime { get; }

		public GameResult(int totalPairs, TimeSpan elapsedTime)
		{
			TotalPairs = totalPairs;
			ElapsedTime = elapsedTime;
		}
	}

}
