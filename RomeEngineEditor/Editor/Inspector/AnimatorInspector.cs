using RomeEngine;
using RomeEngine.IO;

namespace RomeEngineEditor
{
    public sealed class AnimatorInspector : ObjectInspector
    {
        public override bool CanInspect(ISerializable inspectedObject)
        {
            return inspectedObject is Animator;
        }
        protected override void AfterInspect(ISerializable inspectedObject, InspectorMenu inspectorMenu, EditorCanvas canvas)
        {
            Animator animator = (Animator)inspectedObject;
            var animationEditRect = inspectorMenu.GetNextRect();
            var textOptions = new TextOptions() { FontSize = 18f, Alignment = TextAlignment.MiddleCenter };
            if (animator.Animation != null)
            {
                animationEditRect.SplitHorizontal(out Rect editRect, out Rect setNullRect);
                animationEditRect = editRect;
                if (canvas.DrawButton("Set null", setNullRect, textOptions))
                {
                    animator.PlayAnimation(null);
                }
            }
            if (canvas.DrawButton("Edit", animationEditRect, textOptions))
            {
                var screenSize = Screen.Size;
                EditorMenu.ShowMenu<AnimationMenu>(canvas, menu => animator.PlayAnimation(menu.Animation))
                    .Initialize(animator, animator.Animation, new Rect(screenSize.x * 0.25f, screenSize.y * 0.66f, screenSize.x * 0.5f, screenSize.y * 0.34f));
            }
            if (animator.Animation != null)
            {
                inspectorMenu.AllocateField(out Rect playRect, out Rect stopRect);

                if (canvas.DrawButton("Play", playRect, textOptions))
                {
                    animator.PlayAnimation(animator.Animation);
                }
                if (canvas.DrawButton("Stop", stopRect, textOptions))
                {
                    animator.Stop();
                }
            }
        }
    }
}