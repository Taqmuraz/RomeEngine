using RomeEngine;

namespace RomeEngineGame
{
    public interface IState<TKey> : IEventsHandler
    {
        TKey GetStateKey();
        TKey GetNextStateKey();
    }
}
