using System;

namespace RomeEngine.IO
{
    public sealed class ColladaDelegateHandler<TContext> : IColladaNodeHandler<TContext> where TContext : IColladaParsingContext
    {
        Action<TContext, IColladaNode> actionStart;
        Action<TContext, IColladaNode> actionEnd;

        public ColladaDelegateHandler(string name, Action<TContext, IColladaNode> actionStart, Action<TContext, IColladaNode> actionEnd)
        {
            Name = name;
            this.actionStart = actionStart;
            this.actionEnd = actionEnd;
        }

        public string Name { get; }

        public void ParseStart(TContext context, IColladaNode node)
        {
            actionStart?.Invoke(context, node);
        }
        public void ParseEnd(TContext context, IColladaNode node)
        {
            actionEnd?.Invoke(context, node);
        }
    }
}
