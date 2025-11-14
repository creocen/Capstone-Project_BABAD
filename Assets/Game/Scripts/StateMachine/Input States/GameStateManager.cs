using UnityEngine;
using Core.PlayerInput;
using Core.State_Machine;

using Core.Dialogue_System;
using Core.Movement;
using Minigame.TowerBuilder;

namespace Core.InputStates
{
    public class GameStateManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Animator animator;
        [SerializeField] private InputReader inputReader;
        [SerializeField] private DialogueManager dialogueManager;

        [Header("Player Controller References")]
        [SerializeField] private PlayerMovement playerBaseMovement;
        [SerializeField] private DropBlock playerBlockDropper;
        //[SerializeField] private (Player Controller) playerMSGCatcher;
        //[SerializeField] private (Player Controller) playerPaddle;

        private StateMachine stateMachine;
        private PlayerBaseState baseInputState;
        private DialogueUIState dialogueState;
        private TowerBuildingState towerBuildingState;

        void Awake()
        {
            stateMachine = new StateMachine();

            baseInputState = new PlayerBaseState(playerBaseMovement, animator, inputReader);
            towerBuildingState = new TowerBuildingState(playerBlockDropper, animator, inputReader);
            dialogueState = new DialogueUIState(inputReader,dialogueManager);

            BaseInput();
        }

        void Update()
        {
            stateMachine.Update();
        }

        void FixedUpdate()
        {
            stateMachine.FixedUpdate();
        }

        public void BaseInput()
        {
            inputReader.EnablePlayerInput();
            stateMachine.SetState(baseInputState);
        }

        public void StartDialogue()
        {
            inputReader.EnableUIInput();
            stateMachine.ChangeState(dialogueState);
            Debug.Log("Start Dialogue UI State");
        }
        
        public void EndDialogue()
        {
            inputReader.EnablePlayerInput();
            stateMachine.ChangeState(baseInputState);
            Debug.Log("Exiting Dialogue UI State");
        }

        public void StartTowerBuilding()
        {
            inputReader.EnableTowerBuilderInput();
            stateMachine.ChangeState(towerBuildingState);
            Debug.Log("Start Tower Builing State");
        }

        public void StopTowerBuilding()
        {
            inputReader.EnablePlayerInput();
            stateMachine.ChangeState(baseInputState);
            Debug.Log("Exiting Tower Builing State");
        }

    }
}
