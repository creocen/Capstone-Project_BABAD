using UnityEngine;
using Core.PlayerInput;
using Core.State_Machine;
using Minigame.TowerBuilder;

namespace Core.InputStates
{
    public class TowerBuildingState : IState
    {
        readonly InputReader inputReader;

        protected DropBlock player;
        protected Animator animator;

        protected internal TowerBuildingState(DropBlock player, Animator animator, InputReader input)
        {
            this.player = player;
            this.animator = animator;
            this.inputReader = input;
        }

        public virtual void OnEnter() { inputReader.EnableTowerBuilderInput(); }
        public virtual void Update() { }
        public virtual void FixedUpdate() { }
        public virtual void OnExit() { }
    }
}
