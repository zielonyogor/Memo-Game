internal class Program
{
	private static void Main(string[] args)
	{
		var daoObject = new NR155910155992.MemoGame.Dao.SqliteDAO(); // this is not proper way for our assignment, this is just for testing purpose
		var result = daoObject.GetAllCards();
		foreach (var item in result)
		{
			Console.WriteLine($"test: {item.Id}, {item.Name}, {item.ImagePath}");
		}
	}
}