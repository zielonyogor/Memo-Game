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

		// GET: GameController
		public ActionResult Index()
		{
			//propably will have to be changed to GetRandomCardsPositionedOnBoard
			var rawCards = _gameManager.GetRandomSetOfCards(8);

			var deck = new List<CardViewModel>();
			foreach (var card in rawCards)
			{
				deck.Add(new CardViewModel(card));
				deck.Add(new CardViewModel(card));
			}
			return View(deck); // we should probably not pass list of cards here but maybe a GameViewModel instead
		}

		// GET: GameController/Details/5
		public ActionResult Details(int id)
		{
			return View();
		}

		// GET: GameController/Create
		public ActionResult Create()
		{
			return View();
		}

		// POST: GameController/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(IFormCollection collection)
		{
			try
			{
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}

		// GET: GameController/Edit/5
		public ActionResult Edit(int id)
		{
			return View();
		}

		// POST: GameController/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(int id, IFormCollection collection)
		{
			try
			{
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}

		// GET: GameController/Delete/5
		public ActionResult Delete(int id)
		{
			return View();
		}

		// POST: GameController/Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(int id, IFormCollection collection)
		{
			try
			{
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}
	}
}
