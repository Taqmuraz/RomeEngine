using System.Linq;

namespace RomeEngine.IO
{
    public sealed class ColladaAnimationBuilder : IColladaBuilder
    {
        public void BuildGameObject(GameObject gameObject, ColladaEntity rootEntity, ColladaParsingInfo info)
        {
            var animations = rootEntity["library_animations"]["animation"];

            if (animations.IsEmpty || !animations.First().Children.Any()) return;

            var colladaAnimation = new ColladaAnimation(animations);
            var animator = gameObject.AddComponent<Animator>();
            animator.PlayAnimation(colladaAnimation);
            gameObject.Transform.LocalScale = Vector3.one * 0.01f;
        }
    }
}
