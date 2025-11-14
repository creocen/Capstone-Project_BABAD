using UnityEngine;

namespace Core.Interactions
{
    public interface IInteractable 
    {
        string InteractionPrompt { get; } // public string InteractionPrompt => func
        void Interact(GameObject player); // public void Interact(GameObject player) { method }
        Transform Transform { get; } // public Transform Transform => func
    }
}