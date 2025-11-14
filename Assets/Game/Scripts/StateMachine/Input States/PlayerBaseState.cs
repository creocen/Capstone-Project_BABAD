using UnityEngine;
using Core.State_Machine;
using Core.PlayerInput;
using Core.Movement;

namespace Core.InputStates
{
    public class PlayerBaseState : IState
    {
        readonly InputReader inputReader;


        protected PlayerMovement player;
        protected Animator animator;

        protected static readonly int IdleHash = Animator.StringToHash("playerIDLE");
        protected static readonly int MoveHash = Animator.StringToHash("playerMOVE");
        protected static readonly int JumpHash = Animator.StringToHash("playerJUMP");
        protected static readonly int DashHash = Animator.StringToHash("playerDASH");
        protected static readonly int GlideHash = Animator.StringToHash("playerGLIDE");

        protected const float CrossFadeDuration = 0.1f;

        protected internal PlayerBaseState(PlayerMovement player, Animator animator, InputReader inputReader)
        {
            this.player = player;
            this.animator = animator;
            this.inputReader = inputReader;
        }

        public virtual void OnEnter() {  }
        public virtual void Update() { }
        public virtual void FixedUpdate() { }
        public virtual void OnExit() { }
    }

}