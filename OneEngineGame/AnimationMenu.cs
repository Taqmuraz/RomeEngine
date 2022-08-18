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

        int scroll;

        public void Initialize(Animator animator, Animation animation, Rect rect)
        {
            Animator = animator;
            Animation = initialAnimation = animation;
            Rect = rect;
        }

        public override void Draw(EditorCanvas canvas)
        {
            Rect rect = Rect;

            float scrollWidth = 30f;
            float panelWidth = rect.Width - scrollWidth;

            float elementHeight = rect.Height * 0.125f;
            float elementWidth = panelWidth * 0.25f;

            AnimationFrame CreateFrame(float timeCode)
            {
                return new AnimationFrame(Animator.Bones.Select(b => new AnimationFrameElement(b.Name, b.LocalRotation)).ToArray(), timeCode);
            }

            (string title, Func<bool> condition, Action action)[] buttons = new (string, Func<bool>, Action)[]
            {
                ("Import animation", () => true, () => Engine.Instance.Runtime.ShowFileOpenDialog("./", "Select animation", file => Animation = (Animation)new Serializer().DeserializeFile(file))),
                ("Export animation", () => Animation != null, () => Engine.Instance.Runtime.ShowFileWriteDialog("./", $"Animation{Serializer.BinaryFormatExtension}", "Select animation", file => new Serializer().SerializeFile(Animation, file))),
                ("Create new animation", () => true, () => Animation = new FrameBasedAnimation(new [] { CreateFrame(0f) })),
                ("Add frame", () => Animation is FrameBasedAnimation, () =>
                {
                    var fb = (FrameBasedAnimation)Animation;
                    Animation = new FrameBasedAnimation(fb.Frames.Append(CreateFrame(fb.Length + 0.25f)).ToArray());
                }),
                ("Play", () => Animator != null && Animation != null, () => Animator.PlayAnimation(Animation)),
                ("Stop", () => Animator != null, () => Animator.Stop()),
                ("Apply", () => true, () => Close()),
                ("Cancel", () => true, () => { Animation = initialAnimation; Close(); }),
            };

            canvas.DrawRect(rect);
            int buttonPosition = 0;

            for (int i = 0; i < buttons.Length; i++)
            {
                var button = buttons[i];
                if (button.condition() && canvas.DrawButton(button.title, new Rect(rect.X, rect.Y + elementHeight * buttonPosition++, elementWidth, elementHeight), TextOptions.Default))
                {
                    button.action();
                }
            }

            if (Animation is FrameBasedAnimation frameBased)
            {
                void ChangeFrame(AnimationFrame src, AnimationFrame dst)
                {
                    var frames = frameBased.Frames;
                    Animation = new FrameBasedAnimation(frames.Select(f => f == src ? dst : f).ToArray());
                }

                canvas.DrawRect(new Rect(rect.X + elementWidth, rect.Y, rect.Width - elementWidth, rect.Height), Color32.white * 0.8f);

                int index = 0;
                foreach (var frame in frameBased.Frames)
                {
                    if (index - scroll >= 0)
                    {
                        float posY = rect.Y + (index - scroll) * elementHeight;

                        Rect timecodeRect;
                        if (canvas.DrawButton($"({index}) time code : " + frame.TimeCode.ToString(), timecodeRect = new Rect(rect.X + elementWidth, posY, elementWidth, elementHeight), TextOptions.Default))
                        {
                            ShowMenu<StringInputMenu>(canvas, menu => ChangeFrame(frame, new AnimationFrame(frame.FrameElements.ToArray(), menu.InputString.ToFloat())), timecodeRect);
                        }
                        if (canvas.DrawButton("Apply frame", new Rect(rect.X + elementWidth * 2f, posY, elementWidth, elementHeight), TextOptions.Default))
                        {
                            AnimationFrame.ApplyBlended(frame, frame, Animator.Bones.ToDictionary(b => b.Name), 0f);
                            //Animator.PlayAnimationFrame(Animation, frame.TimeCode);
                        }
                        if (canvas.DrawButton("Write frame", new Rect(rect.X + elementWidth * 3f, posY, elementWidth, elementHeight), TextOptions.Default))
                        {
                            AnimationFrame newFrame = CreateFrame(frame.TimeCode);
                            ChangeFrame(frame, newFrame);
                        }
                    }
                    index++;
                }

                index = frameBased.Frames.Length;
                int elementsMax = (int)(rect.Height / elementHeight);
                if (index > elementsMax)
                {
                    scroll = (int)canvas.DrawScrollbar(GetHashCode(), 0f, index - elementsMax, scroll, new Rect(rect.X + elementWidth * 4f, rect.Y, scrollWidth, rect.Height), 1, Color32.gray);
                }
                else
                {
                    scroll = 0;
                }
            }
        }
    }
}