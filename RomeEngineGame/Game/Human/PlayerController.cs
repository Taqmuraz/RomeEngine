using RomeEngine;

namespace OneEngineGame
{
    public sealed class PlayerController : HumanController, IControlAgent
    {
        float orthographicSize = 5f;
        Camera camera;
        EmptyControlAgentAction emptyAction = new EmptyControlAgentAction();
        CustomControlAgentAction attackAction_0 = new CustomControlAgentAction(agent => agent.MoveToState("RetreatAttack"));

        [BehaviourEvent]
        void Start()
        {
            camera = Camera.Cameras[0];
            camera.OrthographicMultiplier = orthographicSize;
        }

        public override IControlAgent GetControlAgent() => this;

        public Vector2 InputMovement => Input.GetWASD();

        public IControlAgentAction GetAction()
        {
            if (Input.GetKeyDown(KeyCode.Q)) return attackAction_0;
            return emptyAction;
        }
    }
}
