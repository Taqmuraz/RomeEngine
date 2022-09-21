using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RomeEngine;
using RomeEngine.IO;
using RomeEngine.UI;

namespace RomeEngineGame
{
    public sealed class AnimationMenu : EditorMenu
    {
        public Animator2D Animator { get; set; }
        public Animation2D Animation { get; set; }
        Animation2D initialAnimation;

        int scroll;

        public void Initialize(Animator2D animator, Animation2D animation, Rect rect)
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

            Animation2DFrame CreateFrame(float timeCode)
            {
                return new Animation2DFrame(Animator.Bones.Select(b => new Animation2DFrameElement(b.Name, b.LocalRotation, b.LocalPosition)).ToArray(), timeCode);
            }

            (string title, Func<bool> condition, Action action)[] buttons = new (string, Func<bool>, Action)[]
            {
                ("Import animation", () => true, () => Engine.Instance.Runtime.ShowFileOpenDialog("./", "Select animation", file => Animation = (Animation2D)new Serializer().DeserializeFile(file))),
                ("Export animation", () => Animation != null, () => Engine.Instance.Runtime.ShowFileWriteDialog("./", $"Animation{Serializer.BinaryFormatExtension}", "Select animation", file => new Serializer().SerializeFile(Animation, file))),
                ("Create new animation", () => true, () => Animation = new FrameBasedAnimation2D(new [] { CreateFrame(0f) })),
                ("Add frame", () => Animation is FrameBasedAnimation2D, () =>
                {
                    var fb = (FrameBasedAnimation2D)Animation;
                    Animation = new FrameBasedAnimation2D(fb.Frames.Append(CreateFrame(fb.Length + 0.25f)).ToArray());
                }),
                ("Play", () => Animator != null && Animation != null, () => Animator.PlayAnimation(Animation)),
                ("Stop", () => Animator != null, () => Animator.Stop()),
                ("Apply", () => true, () => Close()),
                ("Change inverval", () => Animation is FrameBasedAnimation2D, () =>
                {
                    ShowMenu<StringInputMenu>(canvas, menu => 
                    {
                        float interval = menu.InputString.ToFloat();
                        if (Animation is FrameBasedAnimation2D frameBasedAnimation)
                        {
                            var frames = frameBasedAnimation.Frames.ToArray();
                            for (int i = 0; i < frames.Length; i++)
                            {
                                frames[i] = new Animation2DFrame(frames[i].FrameElements, i * interval);
                            }
                            Animation = new FrameBasedAnimation2D(frames);
                        }
                    });
                }),
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

            if (Animation is FrameBasedAnimation2D frameBased)
            {
                void ChangeFrame(Animation2DFrame src, Animation2DFrame dst)
                {
                    var frames = frameBased.Frames;
                    Animation = new FrameBasedAnimation2D(frames.Select(f => f == src ? dst : f).ToArray());
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
                            ShowMenu<StringInputMenu>(canvas, menu => ChangeFrame(frame, new Animation2DFrame(frame.FrameElements.ToArray(), menu.InputString.ToFloat())), timecodeRect);
                        }
                        if (canvas.DrawButton("Apply frame", new Rect(rect.X + elementWidth * 2f, posY, elementWidth, elementHeight), TextOptions.Default))
                        {
                            Animation2DFrame.ApplyBlended(frame, frame, Animator.Bones.ToDictionary(b => b.Name), 0f);
                            //Animator.PlayAnimationFrame(Animation, frame.TimeCode);
                        }
                        if (canvas.DrawButton("Write frame", new Rect(rect.X + elementWidth * 3f, posY, elementWidth, elementHeight), TextOptions.Default))
                        {
                            Animation2DFrame newFrame = CreateFrame(frame.TimeCode);
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