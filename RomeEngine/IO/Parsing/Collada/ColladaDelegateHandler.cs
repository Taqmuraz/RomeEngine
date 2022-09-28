using System;

namespace RomeEngine.IO
{
    public sealed class ColladaDelegateNodeHandler : IColladaNodeHandler
    {
        Action<IColladaNode> actionStart;
        Action<IColladaNode> actionEnd;
        Func<IColladaNode, bool> condition;

        public ColladaDelegateNodeHandler(string nodeType, Action<IColladaNode> actionStart, Action<IColladaNode> actionEnd) :
            this(node => node.NodeType == nodeType, actionStart, actionEnd)
        {
        }
        public ColladaDelegateNodeHandler(Func<IColladaNode, bool> condition, Action<IColladaNode> actionStart, Action<IColladaNode> actionEnd)
        {
            this.condition = condition;
            this.actionStart = actionStart;
            this.actionEnd = actionEnd;
        }

        public bool CanParse(IColladaNode node) => condition(node);

        public void ParseStart(IColladaNode node)
        {
            actionStart?.Invoke(node);
        }
        public void ParseEnd(IColladaNode node)
        {
            actionEnd?.Invoke(node);
        }
    }
}
