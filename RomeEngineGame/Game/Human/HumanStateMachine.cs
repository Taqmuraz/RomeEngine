using RomeEngine;
using System.Collections.Generic;

namespace OneEngineGame
{
    public class HumanStateMachine : StateMachine<string, HumanState>, IInitializable<HumanController>
    {
        protected HumanController HumanController { get; private set; }

        protected override IEnumerable<HumanState> CreateStates()
        {
            yield return new HumanSwordDefaultState().Initialize(HumanController);
            yield return new HumanRetreatAttackState().Initialize(HumanController);
        }

        public void Initialize(HumanController human)
        {
            HumanController = human;
            Initialize();
        }
    }
}
