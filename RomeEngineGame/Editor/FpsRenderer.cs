using RomeEngine;
using RomeEngine.UI;


namespace RomeEngineGame
{
    public sealed class FpsRenderer : Component
    {
        Canvas canvas;

        [BehaviourEvent]
        void Start ()
        {
            canvas = GameObject.AddComponent<Canvas>();
        }
        [BehaviourEvent]
        void Update()
        {
            canvas.DrawText($"Time : {Time.CurrentTime}", new Rect(50, 100, 150, 50), Color32.black, TextOptions.Default);
            canvas.DrawText($"Delta time : {Time.DeltaTime}", new Rect(50, 50, 150, 50), Color32.black, TextOptions.Default);
            canvas.DrawButton($"System time : {System.DateTime.Now}", new Rect(50, 150, 150, 50), Color32.white, Color32.gray, Color32.green, Color32.red, TextOptions.Default);
        }
    }
}
