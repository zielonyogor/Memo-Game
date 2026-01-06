using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NR155910155992.MemoGame.Core;
using NR155910155992.MemoGame.Interfaces;
using NR155910155992.MemoGame.WebUI.Models;
using System.Diagnostics;

namespace NR155910155992.MemoGame.WebUI.Controllers
{
	public class GameController : Controller
	{
		private readonly IGameManager _gameManager;

		public GameController(IGameManager gameManager)
		{
			_gameManager = gameManager;
		}

		[HttpGet]
		public IActionResult Settings()
		{
			var model = new GameSettingModel
			{
				Rows = 2,
				Columns = 1,
				TotalLibraryCards = _gameManager.GetCardsCount()
			};

			return View(model);
		}

		[HttpPost]
		public IActionResult Start(GameSettingModel model)
		{
			_gameManager.ResetGame();

			TempData["Rows"] = model.Rows;
			TempData["Cols"] = model.Columns;

			return RedirectToAction(nameof(Index));
		}

		// GET: Game/
		[HttpGet]
		public IActionResult Index()
		{
			int rows = (int?)TempData.Peek("Rows") ?? 2;
			int cols = (int?)TempData.Peek("Cols") ?? 1;

			var board = _gameManager.GetRandomCardsPositionedOnBoard(rows, cols);

			var cardList = new List<CardViewModel>();
			for (int r = 0; r < rows; r++)
			{
				for (int c = 0; c < cols; c++)
				{
					var card = board[r, c];
					if (card != null)
					{
						var cardViewModel = new CardViewModel(card)
						{
							Row = r,
							Col = c
						};
						cardList.Add(cardViewModel);
					}
				}
			}

			_gameManager.StartNewGame(Core.GameMode.Pairs, Core.GameType.Solo);
			return View(cardList);
		}

		[HttpPost]
		public IActionResult Flip(int row, int col) // TODO: maybe model for response here?
		{
			BoardState state = _gameManager.OnCardClicked(row, col);

			var field = state.Fields[row, col];

			var cardData = _gameManager.GetCardById(field.CardId);

			string status;
			if (field.State == ClickResult.Match)
			{
				status = "match";
			}
			else if (field.State == ClickResult.FirstCard)
			{
				status = "wait";
			}
			else
			{
				status = "mismatch";
			}

			string imageUrl = Url.Action("GetImage", "CardItems", new { id = field.CardId }) ?? "Assets/Cards/missing_image.png";

			return Json(new
			{
				success = true,
				status = status,            // "wait", "match", or "mismatch"
				imageUrl = imageUrl,
				isFinished = state.IsFinished
			});
		}

		// GET: Game/Result
		[HttpGet]
		public IActionResult Result()
		{
            var result = _gameManager.GetCurrentGameResult();
			Debug.WriteLine($"Got results: {result.ElapsedTime} and {result.TotalPairs}");

			var model = new GameResultViewModel
			{
				TimeElapsed = result.ElapsedTime,
				PairsMatched = result.TotalPairs,
			};

			return View(model);
		}

        [HttpGet]
        public IActionResult GetTime()
        {
            var time = _gameManager.GetTimeElapsed();
            return Json(new { time = time.ToString(@"mm\:ss") });
        }
	}
}
