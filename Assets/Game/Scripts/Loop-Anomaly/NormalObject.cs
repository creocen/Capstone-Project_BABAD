using UnityEngine;
using Core.InputStates;
using Core.Interactions;

namespace Minigame.Loop_Anomaly
{
    public class NormalObject : MonoBehaviour, IInteractable
    {
        [Header("Object Info")]
        [SerializeField] private string objName = "A Totally Normal Object";
        [SerializeField] private string feedbackMessage = "Told ya it was normal!";
        [SerializeField] private string currentPrompt;

        public string InteractionPrompt
        {
            get => currentPrompt;
            set => currentPrompt = value;
        }
        public Transform Transform => this.transform;

        void Start()
        {
            currentPrompt = $"Press [E] to interact with {objName}.";
        }

        public void Interact(GameObject player)
        {
            InteractionPrompt = feedbackMessage;
        }
    }
}
