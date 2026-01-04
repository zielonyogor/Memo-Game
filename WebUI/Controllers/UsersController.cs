using Microsoft.AspNetCore.Mvc;
using NR155910155992.MemoGame.Interfaces;
using NR155910155992.MemoGame.WebUI.Models;
using System.Diagnostics;

namespace NR155910155992.MemoGame.WebUI.Controllers
{
	public class UsersController : Controller
	{
		private readonly IGameManager _gameManager;

		public UsersController(IGameManager gameManager)
		{
			_gameManager = gameManager;
		}

		// GET: UsersController
		public ActionResult Index()
		{
			var users = _gameManager.GetAllUserProfiles();
			List<UserViewModel> userList = new List<UserViewModel>();
			foreach (var user in users)
			{
				userList.Add(new UserViewModel(user));
			}
			return View(userList);
		}

		// POST: UsersController/Select/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Select(int id)
		{
			IUserProfile? user = _gameManager.GetAllUserProfiles().FirstOrDefault(u => u.Id == id);
			if (user == null)
			{
				return RedirectToAction(nameof(Index));
			}
			Debug.WriteLine($"Selected user: {user.UserName} (ID: {user.Id})");
			_gameManager.SetCurrentUserProfile(user);
			Debug.WriteLine($"Current user set to: {_gameManager.GetCurrentUserProfile()?.UserName} (ID: {_gameManager.GetCurrentUserProfile()?.Id})");
			return RedirectToAction("Index", "Home");
		}

		// GET: UsersController/Create
		public ActionResult Create()
		{
			return View();
		}

		// POST: UsersController/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(IFormCollection collection)
		{
			string? userName = collection["UserName"];
			if (string.IsNullOrWhiteSpace(userName))
			{
				return View();
			}
			_gameManager.CreateNewUserProfile(userName);
			
			return RedirectToAction(nameof(Index));
		}

		// GET: UsersController/Edit/5
		public ActionResult Edit(int id)
		{
			IUserProfile? user = _gameManager.GetAllUserProfiles().FirstOrDefault(u => u.Id == id);
			if(user == null)
			{
				return NotFound();
			}
			UserViewModel userModel = new UserViewModel(user);
			return View(userModel);
		}

		// POST: UsersController/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(int id, IFormCollection collection)
		{
			string? newUserName = collection["UserName"];
			if (string.IsNullOrWhiteSpace(newUserName))
			{
				return View();
			}

			IUserProfile? user = _gameManager.GetAllUserProfiles().FirstOrDefault(u => u.Id == id);
			if (user == null)
			{
				return NotFound();
			}

			_gameManager.UpdateUserProfile(user, newUserName);
			return RedirectToAction(nameof(Index));
		}

		// POST: UsersController/Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(int id)
		{
			_gameManager.DeleteUserProfile(id);
			return RedirectToAction(nameof(Index));
		}
	}
}
