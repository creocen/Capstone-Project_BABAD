using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Core.PlayerInput
{
    public class InputReader : MonoBehaviour, PlayerInputActions.IPlayerActions, PlayerInputActions.IUIActions, PlayerInputActions.ITowerBuilderActions
    {
        #region Player Movement Events
        public event Action JumpPressed;
        public event Action JumpReleased;
        public event Action DoubleJumpPressed;
        public event Action DashPressed;
        public event Action GlidePressed;
        public event Action GlideReleased;
        public event Action InteractPressed;
        #endregion

        #region Tower Builder Events
        public event Action DropBlockPressed;
        #endregion

        #region UI
        public event Action NextLinePressed;
        public event Action EndDialoguePressed;
        #endregion

        public float MoveInput { get; private set; }
        public bool isJumpReleased { get; private set; }

        PlayerInputActions inputActions;

        void Awake()
        {
            /*inputActions = new PlayerInputActions();
            inputActions.Player.SetCallbacks(this);
            inputActions.TowerBuilder.SetCallbacks(this);*/
            InitializeInputActions();
        }

        void OnEnable()
        {
            if (inputActions == null)
            {
                /*inputActions = new PlayerInputActions();
                inputActions.Player.SetCallbacks(this);*/

                InitializeInputActions();
            }
            //inputActions.Player.Enable();

            EnablePlayerInput(); // Starts with PlayerInput by default
        }

        void OnDisable()
        {
            /*if (inputActions != null)
            {
                inputActions.Player.Disable();
            }*/

            DisableAllInputs();
        }

        void OnDestroy()
        {
            if (inputActions != null)
            {
                //inputActions.Player.Disable();
                DisableAllInputs();
                inputActions.Dispose();
            }
        }

        void InitializeInputActions()
        {
            if (inputActions == null)
            {
                inputActions = new PlayerInputActions();
                inputActions.Player.SetCallbacks(this);
                inputActions.TowerBuilder.SetCallbacks(this);
                inputActions.UI.SetCallbacks(this);
            }
        }

        public void EnablePlayerInput()
        {
            if (inputActions == null) return;
            inputActions.TowerBuilder.Disable();
            inputActions.UI.Disable();
            inputActions.Player.Enable();
            Debug.Log("Player Input Enabled");
        }

        public void EnableTowerBuilderInput()
        {
            if (inputActions == null) return;
            inputActions.Player.Disable();
            inputActions.UI.Disable();
            inputActions.TowerBuilder.Enable();
            Debug.Log("TowerBuilder Input Enabled");
        }

        public void EnableUIInput()
        {
            if (inputActions == null) return;
            inputActions.Player.Disable();
            inputActions.TowerBuilder.Disable();
            inputActions.UI.Enable();
            Debug.Log("UI Input Enabled");
        }

        public void DisableAllInputs()
        {
            if (inputActions == null) return;
            inputActions.Player.Disable();
            inputActions.TowerBuilder.Disable();
            inputActions.UI.Disable();
        }

        #region Player Action Callbacks
        public void OnMove(InputAction.CallbackContext ctx)
        {
            Vector2 input = ctx.ReadValue<Vector2>();
            MoveInput = input.x;
        }

        public void OnJump(InputAction.CallbackContext ctx)
        {
            if (ctx.performed)
            {
                JumpPressed?.Invoke();
                isJumpReleased = false;
            }
            if (ctx.canceled)
            {
                JumpReleased?.Invoke();
                isJumpReleased = true;
            }
        }

        public void OnDoubleJump(InputAction.CallbackContext ctx)
        {
            if (ctx.performed) DoubleJumpPressed?.Invoke();
        }

        public void OnDash(InputAction.CallbackContext ctx)
        {
            if (ctx.performed) DashPressed?.Invoke();
        }

        public void OnGlide(InputAction.CallbackContext ctx)
        {
            if (ctx.performed)
            {
                GlidePressed?.Invoke();
            }
            if (ctx.canceled)
            {
                GlideReleased?.Invoke();
            }
        }

        public void OnInteract(InputAction.CallbackContext ctx)
        {
            if (ctx.performed) InteractPressed?.Invoke();
        }
        #endregion

        #region Tower Builder Action Callbacks
        public void OnDropBlock(InputAction.CallbackContext ctx)
        {
            if (ctx.performed) DropBlockPressed?.Invoke();
        }
        #endregion

        #region UI Action Callbacks
        public void OnNextLine(InputAction.CallbackContext ctx)
        {
            if (ctx.performed) NextLinePressed?.Invoke();
        }

        public void OnEndDialogue(InputAction.CallbackContext ctx)
        {
            if (ctx.performed) EndDialoguePressed?.Invoke();
        }
        #endregion
    }

}