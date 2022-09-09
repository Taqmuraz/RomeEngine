using RomeEngine;

namespace OneEngineGame
{
    public interface IState<TKey> : IEventsHandler
    {
        TKey GetStateKey();
        TKey GetNextStateKey();
    }
}
