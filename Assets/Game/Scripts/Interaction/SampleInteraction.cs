using UnityEngine;
using Core.InputStates;
using UnityEngine.SceneManagement;

namespace Core.Interactions
{
    public class SampleInteraction : MonoBehaviour, IInteractable
    {
        [Header("Info")]
        [SerializeField] private string objName;
        //[SerializeField] private GameObject DialogueBox;
        [SerializeField] GameStateManager stateManager;

        public string InteractionPrompt => $"Press [E] to interact with {objName}.";
        public Transform Transform => this.transform;

        public void Interact(GameObject player)
        {
            // Insert logic here

        }
    }
}
