using Newtonsoft.Json;
using NR155910155992.MemoGame.Core;
using NR155910155992.MemoGame.Interfaces;

namespace NR155910155992.WebUI.Services
{
    public class SessionGameStateStore : IGameStateStore
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private const string SessionKey = "GameState";

        public SessionGameStateStore(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public GameState LoadState()
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            if (session == null) return new GameState();

            var json = session.GetString(SessionKey);
            if (string.IsNullOrEmpty(json)) return new GameState();

            return JsonConvert.DeserializeObject<GameState>(json) ?? new GameState();
        }

        public void SaveState(GameState state)
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            if (session == null) return;

            var json = JsonConvert.SerializeObject(state);
            session.SetString(SessionKey, json);
        }
    }
}
