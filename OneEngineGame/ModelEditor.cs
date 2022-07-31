using System.Collections;
using OneEngine;
using OneEngine.UI;

namespace OneEngineGame
{
    public sealed class ModelEditor : Editor
    {
        protected override IEnumerator Routine()
        {
            var camera = Camera.Cameras[0];
            Canvas canvas = GameObject.AddComponent<Canvas>();

            Transform root = null;

            while (true)
            {
                yield return null;
                float elementWidth = Screen.Size.x * 0.25f;
                canvas.DrawRect(new Rect(0f, 0f, elementWidth, Screen.Size.y), Color32.gray);
            }
        }
    }
}