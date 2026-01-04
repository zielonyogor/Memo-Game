namespace NR155910155992.MemoGame.Core
{
	public class BoardState
	{
		public bool IsFinished { get; set; } = false;
		public int Rows { get; set; }
		public int Cols { get; set; }

		public class FieldState
		{
			public int CardId { get; set; }
			public ClickResult State { get; set; }
		}

		public FieldState[,] Fields { get; set; }

	}
}
