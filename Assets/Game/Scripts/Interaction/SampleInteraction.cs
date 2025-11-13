using UnityEngine;
using Core.InputStates;
using UnityEngine.SceneManagement;

namespace Core.Interactions
{
    public class SampleInteraction : MonoBehaviour, IInteractable
    {
        [Header("Info")]
        [SerializeField] private string npcName;
        //[SerializeField] private GameObject DialogueBox;
        [SerializeField] GameStateManager stateManager;

        public string InteractionPrompt => $"Press [E] to interact with {npcName}.";
        public Transform Transform => this.transform;

        public void Interact(GameObject player)
        {
            SceneManager.LoadScene("TowerStacking Testing");
            stateManager.StartTowerBuilding();
        }
    }
}
