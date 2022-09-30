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
                Vector3 dir = Input.GetWASD();
                return Camera.ActiveCamera.Transform.LocalToWorld.MultiplyDirection(new Vector3(dir.x, 0f, dir.y));
            }
        }

        [BehaviourEvent]
        void Update()
        {
            var camera = Camera.ActiveCamera;
            Vector2 mouse = Input.MouseDelta * 15f * Time.DeltaTime;
            camera.Transform.LocalRotation += new Vector3(mouse.y, mouse.x, 0f);

            camera.Transform.Position = Transform.Position - camera.Transform.Forward * 4f + Vector3.up;
            Input.CursorState = CursorState.HiddenAndLocked;

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Engine.Quit();
            }
        }

        public IControlAction GetAction()
        {
            if (Input.GetKeyDown(KeyCode.Q)) return attackAction_0;
            if (Input.GetKeyDown(KeyCode.E)) return attackAction_1;
            if (Input.GetKey(KeyCode.Space))
            {
                if (Input.GetKey(KeyCode.W)) return highBlockEnterAction;
                if (Input.GetKey(KeyCode.S)) return lowBlockEnterAction;
                if (Input.GetKeyDown(KeyCode.MouseL)) return shieldStrikeAction;
                return blockEnterAction;
            }
            return resetAction;
        }
    }
}
