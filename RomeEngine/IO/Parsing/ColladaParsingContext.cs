using System.Collections.Generic;
using System.Linq;

namespace RomeEngine.IO
{
    public abstract class ColladaParsingContext<TContext, TElement> : ColladaStackContainingObject<TElement>, IColladaParsingStage where TContext : IColladaParsingContext
    {
        Dictionary<string, IColladaNodeHandler<TContext>> handlers;

        public ColladaParsingContext()
        {
            handlers = CreateHandlers().ToDictionary(h => h.Name);
        }

        public void ParseStart(IColladaNode node)
        {
            if (handlers.TryGetValue(node.GetName(), out var handler))
            {
                handler.ParseStart(GetContext(), node);
            }
        }
        public void ParseEnd(IColladaNode node)
        {
            if (handlers.TryGetValue(node.GetName(), out var handler))
            {
                handler.ParseEnd(GetContext(), node);
            }
        }

        protected abstract IEnumerable<IColladaNodeHandler<TContext>> CreateHandlers();
        protected abstract TContext GetContext();
        public abstract void UpdateGameObject(GameObject gameObject, IColladaParsingInfo parsingInfo);
        public abstract string RootNodeName { get; }
    }
}
