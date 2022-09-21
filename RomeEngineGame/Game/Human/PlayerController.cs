using RomeEngine;

namespace RomeEngineGame
{
    public sealed class PlayerController : HumanController, IControlAgent
    {
        float orthographicSize = 5f;
        Camera2D camera;

        IControlAction emptyAction = new EmptyControlAgentAction();
        IControlAction resetAction = new CustomControlAgentAction(agent => agent.MoveToState("SwordDefault"));

        IControlAction attackAction_0 = new CustomControlAgentAction(agent => agent.MoveToState("RetreatAttack"));
        IControlAction attackAction_1 = new CustomControlAgentAction(agent => agent.MoveToState("AirAttack"));

        IControlAction blockEnterAction = new CustomControlAgentAction(agent => agent.MoveToState("StandardBlock"));
        IControlAction lowBlockEnterAction = new CustomControlAgentAction(agent => agent.MoveToState("LowBlock"));
        IControlAction highBlockEnterAction = new CustomControlAgentAction(agent => agent.MoveToState("HighBlock"));

        IControlAction shieldStrikeAction = new CustomControlAgentAction(agent => agent.MoveToState("ShieldStrike"));

        [BehaviourEvent]
        void Start()
        {
            camera = Camera2D.Cameras[0];
            camera.OrthographicMultiplier = orthographicSize;
        }

        public override IControlAgent GetControlAgent() => this;

        public Vector2 InputMovement => Input.GetWASD();

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
