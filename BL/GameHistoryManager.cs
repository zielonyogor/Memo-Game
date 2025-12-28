using NR155910155992.MemoGame.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NR155910155992.MemoGame.BL
{
	internal class GameHistoryManager
	{
		private readonly IDataAccessObject _dao;

		public GameHistoryManager(IDataAccessObject dao)
		{
			_dao = dao;
		}
		public IEnumerable<IGameSession> GetAllGameSessionsForUser(IUserProfile userProfile)
		{
			return _dao.GetAllGameSessionsForUser(userProfile);
		}
	}
}
