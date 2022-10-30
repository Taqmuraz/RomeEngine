using RomeEngine.IO;
using System.Collections.Generic;

namespace RomeEngine
{
    public class CompositeGameEntity : SerializableEventsHandler, IGameEntity
    {
        [SerializeField] DynamicLinkedList<IGameEntity> innerEntities;
        protected IEnumerable<IGameEntity> InnerEntities => innerEntities;
        [SerializeField] public string Name { get; set; }

        public CompositeGameEntity()
        {
            innerEntities = new DynamicLinkedList<IGameEntity>();
        }
        public CompositeGameEntity(string name) : this()
        {
            Name = name;
        }

        public void AppendEntity(IGameEntity gameEntity)
        {
            innerEntities.Add(gameEntity);
        }
        public void AppendEntities(IEnumerable<IGameEntity> entities)
        {
            innerEntities.AddRange(entities);
        }
        public void RemoveEntity(IGameEntity gameEntity)
        {
            innerEntities.Remove(gameEntity);
        }

        protected sealed override void OnEventCall(string name)
        {
            BeforeCallEvent(name);
            foreach (var innerEntity in innerEntities) innerEntity.CallEvent(name);
            AfterCallEvent(name);
        }
        protected virtual void BeforeCallEvent(string name) { }
        protected virtual void AfterCallEvent(string name) { }

        protected virtual void Activate(IGameEntityActivityProvider activityProvider) { }
        protected virtual void Deactivate(IGameEntityActivityProvider activityProvider) { }

        void IGameEntity.Activate(IGameEntityActivityProvider activityProvider) => Activate(activityProvider);
        void IGameEntity.Deactivate(IGameEntityActivityProvider activityProvider) => Deactivate(activityProvider);
    }
}
