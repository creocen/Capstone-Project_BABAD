using UnityEngine;
using Core.InputStates;
using Core.PlayerInput;

namespace Core.Movement
{
    public class PlayerJumpState : PlayerBaseState
    {
        public PlayerJumpState(PlayerMovement player, Animator animator, InputReader inputReader) : base(player, animator, inputReader) { }

        public override void OnEnter()
        {
            base.OnEnter();
            animator.CrossFade(JumpHash, CrossFadeDuration);
        }

        public override void FixedUpdate()
        {
            player.canDashInAir = true;
            player.HandleMovement();
            player.HandleFall();

        }
    }
}
