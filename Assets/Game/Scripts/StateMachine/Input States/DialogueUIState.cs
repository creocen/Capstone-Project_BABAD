using UnityEngine;
using Core.State_Machine;
using Core.PlayerInput;
using Core.Dialogue_System;

namespace Core.InputStates
{
    public class DialogueUIState : IState
    {
        readonly InputReader inputReader;
        readonly DialogueManager dialogueManager;

        public DialogueUIState(InputReader inputReader, DialogueManager dialogueManager)
        {
            this.inputReader = inputReader;
            this.dialogueManager = dialogueManager;
        }

        public void OnEnter()
        {
            dialogueManager.OpenDialogue();
        }

        public void Update() { }
        public void FixedUpdate() { }
        public void OnExit() { }
    }
}
