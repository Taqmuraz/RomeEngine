using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OneEngine;
using OneEngine.IO;
using OneEngine.UI;

namespace OneEngineGame
{
    public sealed class AnimationMenu : EditorMenu
    {
        public Animator Animator { get; set; }
        public Animation Animation { get; set; }
        Animation initialAnimation;

        public void Initialize(Animator animator, Animation animation, Rect rect)
        {
            Animator = animator;
            Animation = initialAnimation = animation;
            Rect = rect;
        }

        public override void Draw(EditorCanvas canvas)
        {
            Rect rect = Rect;

            float elementHeight = rect.Height * 0.15f;
            float elementWidth = elementHeight * 6f;

            (string title, Func<bool> condition, Action action)[] buttons = new (string, Func<bool>, Action)[]
            {
                ("Import animation", () => true, () => Engine.Instance.Runtime.ShowFileOpenDialog("./", "Select animation", file => Animation = (Animation)new Serializer().DeserializeFile(file))),
                ("Export animation", () => Animation != null, () => Engine.Instance.Runtime.ShowFileWriteDialog("./", $"Animation.{Serializer.BinaryFormatExtension}", "Select animation", file => new Serializer().SerializeFile(Animation, file))),
                ("Create new animation", () => true, () => Animation = new Animation()),
                ("Play", () => Animator != null && Animation != null, () => Animator.PlayAnimation(Animation)),
                ("Stop", () => Animator != null, () => Animator.Stop()),
                ("Apply", () => true, () => Close()),
                ("Cancel", () => true, () => { Animation = initialAnimation; Close(); }),
            };

            canvas.DrawRect(rect);

            for (int i = 0; i < buttons.Length; i++)
            {
                var button = buttons[i];
                if (button.condition() && canvas.DrawButton(button.title, new Rect(rect.X, rect.Y + elementHeight * i, elementWidth, elementHeight), TextOptions.Default))
                {
                    button.action();
                }
            }
        }
    }
}