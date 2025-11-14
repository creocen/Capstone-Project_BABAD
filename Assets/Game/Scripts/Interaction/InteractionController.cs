using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Core.PlayerInput;
using System.Collections.Generic;

namespace Core.Interactions
{
    public class InteractionController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private InputReader inputReader;
        [SerializeField] private CircleCollider2D triggerCollider;

        [Header("Interaction Prompt")]
        [SerializeField] private GameObject interactionPromptUI;
        [SerializeField] private TextMeshProUGUI promptText;

        private List<IInteractable> interactablesInRange = new List<IInteractable>();
        private IInteractable currentInteractable;

        void Start()
        {
            if (interactionPromptUI != null) interactionPromptUI.SetActive(false);
        }

        void OnEnable() { inputReader.InteractPressed += HandleInteraction; }
        void OnDisable() { inputReader.InteractPressed -= HandleInteraction; }

        void Update()
        {
            UpdateCurrentInteractable();
            UpdateUI();
        }

        private void UpdateCurrentInteractable()
        {
            if (interactablesInRange.Count == 0)
            {
                currentInteractable = null;
                return;
            }

            if (interactablesInRange.Count == 1)
            {
                currentInteractable = interactablesInRange[0];
                return;
            }

            currentInteractable = GetClosestInteractable();
        }

        private IInteractable GetClosestInteractable()
        {
            IInteractable interactable = null;
            float closestsInteractable = float.MaxValue;

            foreach (var interactableObject in interactablesInRange)
            {
                if (interactableObject?.Transform == null) continue;

                float distance = Vector2.Distance(transform.position, interactableObject.Transform.position);

                if (distance < closestsInteractable)
                {
                    closestsInteractable = distance;
                    interactable = interactableObject;
                }
            }

            return interactable;
        }

        private void UpdateUI()
        {
            if (interactionPromptUI == null) return;

            bool shouldShow = currentInteractable != null;
            interactionPromptUI.SetActive(shouldShow);

            if (shouldShow && promptText != null)
            {
                promptText.text = currentInteractable.InteractionPrompt;
            }
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            IInteractable interactableObject = other.GetComponent<IInteractable>();

            if (interactableObject != null && !interactablesInRange.Contains(interactableObject))
            {
                interactablesInRange.Add(interactableObject);
                interactionPromptUI.SetActive(true);
                interactionPromptUI.transform.position = new Vector2(interactableObject.Transform.position.x - 1, interactableObject.Transform.position.y + 2);
                Debug.Log($"{other.name}");
            }
        }

        void OnTriggerExit2D(Collider2D other)
        {
            IInteractable interactableObject = other.GetComponent<IInteractable>();

            if (interactableObject != null)
            {
                interactablesInRange.Remove(interactableObject);
            }
        }

        private void HandleInteraction()
        {
            if (currentInteractable != null)
            {
                currentInteractable.Interact(gameObject);
            }
        }
    }
}
