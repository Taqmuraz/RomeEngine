using System.Collections.Generic;
namespace RomeEngine
{
    public interface ITreeAcceptor<TLocatable> : ILocatable
    where TLocatable : ILocatable
    {
        void AcceptLocatables(IEnumerable<TLocatable> locatable);
    }
}