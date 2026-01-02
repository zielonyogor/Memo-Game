using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NR155910155992.MemoGame.Interfaces;
using NR155910155992.MemoGame.WebUI.Models;

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
			return View(new GameSettingModel());
		}

		[HttpPost]
		public IActionResult Start(GameSettingModel model)
		{
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
						cardList.Add(new CardViewModel(card));
					}
				}
			}

			_gameManager.StartNewGame(Core.GameMode.Pairs, Core.GameType.Solo);
			return View(cardList);
		}

		[HttpPost]
		public async Task<IActionResult> Flip(int id)
		{
			var card = _gameManager.GetAllCards().FirstOrDefault(c => c.Id == id);
			var result = await _gameManager.OnCardClicked(id);

			bool resetRequired = false;
			if (result == Core.ClickResult.Mismatch)
			{
				resetRequired = true;
			}
			else if (result == Core.ClickResult.Match)
			{
				// Handle match
			}

			return Json(new
			{
				success = true,
				imageUrl = Url.Action("GetImage", "CardItems", new { id = card.Id }),
				isMatch = result == Core.ClickResult.Match,
				resetRequired = resetRequired,
				isGameOver = false // You might want to check if game is finished
			});
		}

		[HttpPost]
		public IActionResult ResolveMismatch()
		{
			_gameManager.ResolveMismatch();
			return Ok();
		}

		// GET: Game/Result
		[HttpGet]
		public IActionResult Result(double timeElapsed, int pairs)
		{
			var timeSpan = TimeSpan.FromSeconds(timeElapsed);

			var model = new GameResultViewModel
			{
				TimeElapsed = timeSpan,
				PairsMatched = pairs,
			};

			return View(model);
		}
	}
}
