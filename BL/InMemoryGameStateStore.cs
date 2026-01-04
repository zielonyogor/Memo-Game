using NR155910155992.MemoGame.Interfaces;
using NR155910155992.MemoGame.Core;

namespace NR155910155992.MemoGame.BL
{
    public class InMemoryGameStateStore : IGameStateStore
    {
        private GameState _state = new GameState();

        public GameState LoadState()
        {
            return _state;
        }

        public void SaveState(GameState state)
        {
            _state = state;
        }
    }
}
