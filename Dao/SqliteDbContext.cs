using Microsoft.EntityFrameworkCore;
using NR155910155992.MemoGame.Dao.Models;

namespace NR155910155992.MemoGame.Dao
{
	internal class SqliteDbContext : DbContext
	{
		public DbSet<Card> Cards { get; set; }
		public DbSet<UserProfile> UserProfiles { get; set; }
		public DbSet<GameSession> GameSessions { get; set; }
		public DbSet<PlayerGameResult> PlayerGameResults { get; set; }

		public SqliteDbContext()
		{
		}

		protected override void OnConfiguring(DbContextOptionsBuilder options)
		{
			// local app data folder for database storage
			string folder = Path.Combine(
				Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
				"MemoGame"
			);

			Directory.CreateDirectory(folder);

			string dbPath = Path.Combine(folder, "game.db");
			options.UseSqlite($"Data Source={dbPath}");

			Console.WriteLine($"Database path: {dbPath}");
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Card>().HasData(
				new Card { Id = 1, Name = "Card 1", ImagePath = "images/card1.png" },
				new Card { Id = 2, Name = "Card 2", ImagePath = "images/card2.png" },
				new Card { Id = 3, Name = "Card 3", ImagePath = "images/card3.png" }
			);
			modelBuilder.Entity<UserProfile>().HasData(
				new UserProfile { Id = 1, UserName = "Player1" }
			);
			modelBuilder.Entity<GameSession>().HasData(
				new GameSession { 
					Id = 1, 
					GameDate = new DateTime(2025, 1, 1, 12, 0, 0),
					Duration = TimeSpan.FromSeconds(90), 
					GameMode = Core.GameMode.Pairs,
					GameType = Core.GameType.Solo
				}
			);
			modelBuilder.Entity<PlayerGameResult>().HasData(
				new PlayerGameResult { Id = 1, GameSessionId = 1, UserProfileId = 1, IsWinner = true, CardsUncovered = 12 }
			);
		}
	}
}
