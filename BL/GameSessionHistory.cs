using NR155910155992.MemoGame.Core;
using NR155910155992.MemoGame.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;


namespace NR155910155992.MemoGame.BL
{
    internal class GameSessionHistory : IGameSessionHistory
    {
        IDataAccessObject _dao;

        public GameSessionHistory(IConfiguration configuration)
        {
            var loader = new LibraryLoader(configuration);
            _dao = loader.LoadObjectFromLibrary<IDataAccessObject>(LibraryKey.Dao);

        }


        public IEnumerable<IGameSession> GetAllGameSessions()
        {
            Debug.WriteLine($"In BS number of gs: {_dao.GetAllGameSessions().Count()}");
            return _dao.GetAllGameSessions();
        }


    }
}
