using System;
using System.IO;
using RomeEngine;
using RomeEngine.UI;
using System.Linq;

namespace OneEngineGame
{
    public sealed class FileSearchMenu : EditorMenu
    {
        int positionOffset = 0;
        int elementsOnPage = 10;
        string Root
        {
            get => root;
            set
            {
                root = value;
                positionOffset = 0;
            }
        }
        string root;
        string header;

        public FileSearchMenu Initialize(string rootFolder, string header)
        {
            Root = rootFolder;
            this.header = header;
            return this;
        }

        public string File { get; private set; }

        public override void Draw(EditorCanvas canvas)
        {
            try
            {
                string[] files = Directory.GetFiles(Root).OrderBy(f => f).ToArray();
                string[] folders = Directory.GetDirectories(Root).OrderBy(f => f).ToArray();
                int totalLength = files.Length + folders.Length;
                float startX = 50f;
                float startY = 50f;
                float elementWidth = 600f;
                float elementHeight = 30f;
                float outline = 10f;
                TextOptions textOptions = new TextOptions() { FontSize = 15f };

                canvas.DrawRect(new Rect(startX - outline * 0.5f, startY - outline * 0.5f, elementWidth + outline, elementHeight * (elementsOnPage + 2) + outline), Color32.black);
                
                canvas.DrawText(header + $"\n{Root}", new Rect(startX, startY, elementWidth, elementHeight), Color32.white, textOptions);

                for (int i = positionOffset; i < Math.Min(totalLength, elementsOnPage) - positionOffset; i++)
                {
                    int index = i - positionOffset;
                    string text;
                    Action action;
                    if (i < folders.Length)
                    {
                        text = Path.GetFileName(folders[i]);
                        action = () => Root = folders[i];
                    }
                    else if (i < folders.Length + files.Length)
                    {
                        text = Path.GetFileName(files[i - folders.Length]);
                        action = () =>
                        {
                            File = files[i - folders.Length];
                            Close();
                        };
                    }
                    else continue;

                    if (canvas.DrawButton(text,
                            new Rect(startX, startY + elementHeight * (index + 2), elementWidth, elementHeight),
                            Color32.white, Color32.gray, Color32.blue, Color32.green, textOptions))
                    {
                        action();
                    }
                }

                (string text, Action action)[] buttons =
                {
                    ("To parent folder", () => Root = Directory.GetParent(Root).FullName),
                    ("Scroll up", () => positionOffset = Math.Max(positionOffset - 1, 0)),
                    ("Scroll down", () => positionOffset = Math.Min(positionOffset + 1, totalLength - elementsOnPage)),
                };

                for (int i = 0; i < buttons.Length; i++)
                {
                    if (canvas.DrawButton(buttons[i].text, new Rect(startX + elementWidth, startY + elementHeight * (i + 1), elementWidth * 0.5f, elementHeight), Color32.white, Color32.blue, Color32.green, Color32.blue, textOptions))
                    {
                        buttons[i].action();
                    }
                }
            }
            catch (Exception ex)
            {
                System.IO.File.WriteAllText("./error.txt", ex.ToString());
                throw ex;
            }
        }
    }
}