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
				new Card { Id = 3, Name = "Card 3", ImagePath = "images/card3.png" },
				new Card { Id = 4, Name = "Card 4", ImagePath = "images/card4.png" }
			);
			modelBuilder.Entity<UserProfile>().HasData(
				new UserProfile { Id = 1, UserName = "Player1" },
				new UserProfile { Id = 2, UserName = "Player2" }
			);
			modelBuilder.Entity<GameSession>().HasData(
				new GameSession { 
					Id = 1, 
					GameDate = new DateTime(2025, 1, 1, 12, 0, 0),
					Duration = TimeSpan.FromSeconds(90), 
					GameMode = Core.GameMode.Pairs,
					GameType = Core.GameType.Solo
				},
				new GameSession { 
					Id = 2, 
					GameDate = new DateTime(2025, 1, 2, 15, 30, 0),
					Duration = TimeSpan.FromSeconds(150), 
					GameMode = Core.GameMode.Pairs,
					GameType = Core.GameType.Solo
				},
				new GameSession { 
					Id = 3, 
					GameDate = new DateTime(2025, 1, 3, 18, 45, 0),
					Duration = TimeSpan.FromSeconds(200), 
					GameMode = Core.GameMode.Pairs,
					GameType = Core.GameType.Multiplayer
				}
			);
			modelBuilder.Entity<PlayerGameResult>().HasData(
				new PlayerGameResult { Id = 1, GameSessionId = 1, UserProfileId = 1, IsWinner = true, CardsUncovered = 12 },
				new PlayerGameResult { Id = 2, GameSessionId = 2, UserProfileId = 2, IsWinner = false, CardsUncovered = 8 },
				new PlayerGameResult { Id = 3, GameSessionId = 3, UserProfileId = 1, IsWinner = true, CardsUncovered = 10 },
				new PlayerGameResult { Id = 4, GameSessionId = 3, UserProfileId = 2, IsWinner = false, CardsUncovered = 7 }
			);
		}
	}
}
