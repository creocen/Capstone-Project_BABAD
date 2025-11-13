using UnityEngine;
using Core.InputStates;

namespace Core.Interactions
{
    public class TriggerDialogue : MonoBehaviour, IInteractable
    {
        [Header("Info")]
        [SerializeField] private string npcName;
        [SerializeField] private GameStateManager stateManager;

        public string InteractionPrompt => $"Press [E] to interact with {npcName}.";
        public Transform Transform => this.transform;

        public void Interact(GameObject player)
        {
            stateManager.StartDialogue();
        }
    }
}