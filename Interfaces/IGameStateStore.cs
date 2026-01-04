using NR155910155992.MemoGame.Core;

namespace NR155910155992.MemoGame.Interfaces
{
    public interface IGameStateStore
    {
        GameState LoadState();
        void SaveState(GameState state);
    }
}
