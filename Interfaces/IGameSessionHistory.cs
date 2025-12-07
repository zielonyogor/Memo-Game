using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NR155910155992.MemoGame.Interfaces
{
    public interface IGameSessionHistory
    {
        public IEnumerable<IGameSession> GetAllGameSessions();
    }
}

