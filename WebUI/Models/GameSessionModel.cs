using NR155910155992.MemoGame.Core;
using NR155910155992.MemoGame.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace NR155910155992.MemoGame.WebUI.Models
{
	public class GameSessionModel: IGameSession
	{
		public int Id { get; set; }
		public DateTime GameDate { get; set; }
		public TimeSpan Duration { get; set; }

		[Display(Name = "Game type")]
		public GameType GameType { get; set; }
		
		[Display(Name = "Game mode")]
		public GameMode GameMode { get; set; }

		public IEnumerable<IPlayerGameResult> PlayerResults { get; }

		[Display(Name = "Date")]
		public string GameDateString => GameDate.ToString("dd-MM-yyyy");

		// UI specific properties
		[Display(Name = "Participants")]
		public string PlayersString => string.Join(", ", PlayerResults.Select(pr => pr.User.UserName));

		[Display(Name = "Duration")]
		public string DurationString => $"{(int)Duration.TotalMinutes:D2}:{Duration.Seconds:D2}";

		[Display(Name = "Who won?")]
		public string WinnerString => PlayerResults.Where(pr => pr.IsWinner).Select(pr => pr.User.UserName).FirstOrDefault() ?? "?";

		[Display(Name = "Cards uncovered")]
		public int CardsUncovered => PlayerResults.Sum(pr => pr.CardsUncovered);

		public GameSessionModel(IGameSession gameSession)
		{
			Id = gameSession.Id;
			GameDate = gameSession.GameDate;
			Duration = gameSession.Duration;
			GameType = gameSession.GameType;
			GameMode = gameSession.GameMode;
			PlayerResults = gameSession.PlayerResults;
		}
	}
}
