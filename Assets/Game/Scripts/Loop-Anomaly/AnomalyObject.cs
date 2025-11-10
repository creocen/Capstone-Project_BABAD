using UnityEngine;
using Core.InputStates;
using Core.Interactions;

namespace Minigame.Loop_Anomaly
{
    public class AnomalyObject : MonoBehaviour, IInteractable
    {
        [Header("Object Info")]
        [SerializeField] private string objName = "Super Suspicious Object";
        [SerializeField] private string feedbackMessage = "You found an anomaly!";
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
            if (ConditionChecker.Instance != null)
            {
                ConditionChecker.Instance.SetFoundAnomaly(true);
                InteractionPrompt = feedbackMessage;
            }
        }
    }
}
