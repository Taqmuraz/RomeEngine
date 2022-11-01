using RomeEngine;

namespace RomeEngineEditor
{
    public interface IState<TKey> : IEventsHandler
    {
        TKey GetStateKey();
        TKey GetNextStateKey();
    }
}
