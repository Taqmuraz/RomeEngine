namespace RomeEngine
{
    public sealed class CompositeGameEntityBuilder
    {
        CompositeGameEntity entity;
        public CompositeGameEntityBuilder(string name)
        {
            entity = new CompositeGameEntity();
        }
        public CompositeGameEntityBuilder AppendInnerEntity(IGameEntity entity)
        {
            this.entity.AppendEntity(entity);
            return this;
        }
        public IGameEntity Build() => entity;
    }
}
