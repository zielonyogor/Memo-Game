namespace NR155910155992.MemoGame.UI.Models
{
	public class GameSettings
	{
		public int Rows { get; }
		public int Columns { get; }

		public GameSettings(int rows, int columns) {
			Rows = rows; 
			Columns = columns;
		}
	}
}
