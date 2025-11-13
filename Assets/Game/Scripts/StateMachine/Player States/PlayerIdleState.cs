using UnityEngine;
using Core.InputStates;
using Core.PlayerInput;

namespace Core.Movement
{
    public class PlayerIdleState : PlayerBaseState
    {
        public PlayerIdleState(PlayerMovement player, Animator animator, InputReader inputReader) : base(player, animator, inputReader) { }

        public override void OnEnter()
        {
            base.OnEnter();
            animator.CrossFade(IdleHash, 0.1f);
        }

        public override void FixedUpdate()
        {
            player.HandleMovement();
        }
    }
}
