using RomeEngine;

namespace RomeEngineGame
{
    public sealed class PlayerController : HumanController, IControlAgent
    {
        IControlAction emptyAction = new EmptyControlAgentAction();
        IControlAction resetAction = new CustomControlAgentAction(agent => agent.MoveToState("SwordDefault"));

        IControlAction attackAction_0 = new CustomControlAgentAction(agent => agent.MoveToState("RetreatAttack"));
        IControlAction attackAction_1 = new CustomControlAgentAction(agent => agent.MoveToState("AirAttack"));

        IControlAction blockEnterAction = new CustomControlAgentAction(agent => agent.MoveToState("StandardBlock"));
        IControlAction lowBlockEnterAction = new CustomControlAgentAction(agent => agent.MoveToState("LowBlock"));
        IControlAction highBlockEnterAction = new CustomControlAgentAction(agent => agent.MoveToState("HighBlock"));

        IControlAction shieldStrikeAction = new CustomControlAgentAction(agent => agent.MoveToState("ShieldStrike"));

        public override IControlAgent GetControlAgent() => this;

        public Vector3 InputMovement
        {
            get
            {
                Vector2 dir = Input.GetWASD();

                Vector3 fwd = Camera.ActiveCamera.Transform.Forward.WithY(0f).normalized;
                Vector3 right = Camera.ActiveCamera.Transform.Right.WithY(0f).normalized;

                return right * dir.x + fwd * dir.y;
            }
        }

        [BehaviourEvent]
        void Update()
        {
            var camera = Camera.ActiveCamera;
            Vector2 mouse = Input.MouseDelta * 15f * Time.DeltaTime;
            Vector3 euler = camera.Transform.LocalRotation;
            euler += new Vector3(mouse.y, mouse.x, 0f);
            euler.x = Mathf.Clamp(euler.x, -30f, 80f);
            camera.Transform.LocalRotation = euler;

            camera.Transform.Position = Transform.Position - camera.Transform.Forward * 4f + Vector3.up;
            Input.CursorState = CursorState.HiddenAndLocked;

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Engine.Quit();
            }
        }

        public IControlAction GetAction()
        {
            return resetAction;
        }
    }
}
