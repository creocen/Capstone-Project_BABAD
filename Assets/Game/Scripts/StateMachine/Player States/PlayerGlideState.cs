using UnityEngine;
using Core.InputStates;
using Core.PlayerInput;

namespace Core.Movement
{
    public class PlayerGlideState : PlayerBaseState
    {
        public PlayerGlideState(PlayerMovement player, Animator animator, InputReader inputReader) : base(player, animator, inputReader) { }

        public override void OnEnter()
        {
            base.OnEnter();
            animator.CrossFade(GlideHash, 0.01f);
        }

        public override void FixedUpdate()
        {
            player.HandleGlide();
            player.HandleMovement();
        }
    }

}