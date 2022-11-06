using RomeEngine;

namespace RomeEngineEditor
{
    public abstract class HumanMovementState : HumanState
    {
        protected abstract string GetMovementAnimationName();
        protected abstract string GetIdleAnimationName();
        protected abstract float MovementSpeed { get; }

        [BehaviourEvent]
        void PhysicsUpdate()
        {
            var inputMovement = HumanController.GetControlAgent().InputMovement.Normalized;

            var body = HumanController.Collider.PhysicalBody;
            Vector3 moveDirection = new Vector3(inputMovement.x, 0f, inputMovement.z);
            Vector3 desiredVelocity = moveDirection * MovementSpeed;
            desiredVelocity.y = body.Velocity.y;

            Vector3 force = (desiredVelocity - body.Velocity) * body.Mass;
            body.ApplyForce(force);
        }

        [BehaviourEvent]
        void Update()
        {
            var inputMovement = HumanController.GetControlAgent().InputMovement.Normalized;

            if (inputMovement != new Vector2())
            {
                HumanController.HumanAnimator.PlayAnimationWithTransition(GetMovementAnimationName());
                HumanController.Transform.Rotation = Matrix4x4.LookRotation(Vector3.Lerp(HumanController.Transform.Forward, inputMovement.WithY(0f).Normalized, Time.DeltaTime * 15f), Vector3.up).GetEulerRotation();
            }
            else
            {
                HumanController.HumanAnimator.PlayAnimationWithTransition(GetIdleAnimationName());
            }
        }
    }
}
