using System;
using System.Collections.Generic;

namespace RomeEngine
{
    public sealed class CustomTreeAcceptor<TLocatable> : ITreeAcceptor<TLocatable> where TLocatable : ILocatable
    {
        Action<IEnumerable<TLocatable>> accept;
        Func<Bounds, bool> insideBox;

        public CustomTreeAcceptor(Action<IEnumerable<TLocatable>> accept, Func<Bounds, bool> insideBox)
        {
            this.accept = accept;
            this.insideBox = insideBox;
        }

        public void AcceptLocatables(IEnumerable<TLocatable> locatable) => accept(locatable);
        public bool IsInsideBox(Bounds box) => insideBox(box);
    }
}