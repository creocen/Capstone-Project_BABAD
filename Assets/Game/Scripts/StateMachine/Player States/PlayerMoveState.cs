using UnityEngine;
using Core.InputStates;
using Core.PlayerInput;

namespace Core.Movement
{
    public class PlayerMoveState : PlayerBaseState
    {
        public PlayerMoveState(PlayerMovement player, Animator animator, InputReader inputReader) : base(player, animator, inputReader) { }

        public override void OnEnter()
        {
            base.OnEnter();
            animator.CrossFade(MoveHash, CrossFadeDuration);
        }

        public override void FixedUpdate()
        {
            player.HandleMovement();
        }
    }
}
