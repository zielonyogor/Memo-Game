using NR155910155992.MemoGame.Interfaces;

namespace NR155910155992.MemoGame.JsonDao.Models
{
	internal class Card : ICard
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string ImagePath { get; set; }
	}
}
