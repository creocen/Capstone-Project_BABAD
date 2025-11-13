using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.InputSystem;
using System.Collections.Generic;

using Core.Data;
using Core.PlayerInput;
using Core.InputStates;

namespace Core.Dialogue_System
{
    public class DialogueManager : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Image speakerIcon;
        [SerializeField] private TextMeshProUGUI speakerName;
        [SerializeField] private TextMeshProUGUI dialogueText;
        [SerializeField] private GameObject dialogueBox;
        [SerializeField] private GameObject optionsPanel;
        [SerializeField] private GameObject optionBTNPrefab;

        // ----------T E M P O R A R Y---------------
        [Header("Emotion Sprites")]
        [SerializeField] private Sprite spritePandecoco;
        [SerializeField] private Sprite spritePandesal;
        // ----------T E M P O R A R Y---------------

        [Header("Data References")]
        [SerializeField] private SampleDialogueCollection dialogueCollection;
        [SerializeField] private InputReader input;
        [SerializeField] private GameStateManager stateManager;
        [SerializeField] private string startingDialogueID;

        private int currentIndex = 0;
        private bool hasChosen = false;
        private List<GameObject> activeOptionBTNS = new List<GameObject>();

        #region Enable-Disable
        void OnEnable() 
        { 
            input.NextLinePressed += OnInteract;
            input.EndDialoguePressed += CloseDialogue;
        }
        void OnDisable() 
        {
            input.NextLinePressed -= OnInteract;
            input.EndDialoguePressed -= CloseDialogue;
        }
        #endregion

        private void Start()
        {
            if (dialogueBox != null)
            {
                dialogueBox.SetActive(false);
            }

            if (!string.IsNullOrEmpty(startingDialogueID))
            {
                int startIndex = dialogueCollection.DialogueData.FindIndex(dialogue => dialogue.DialogueID == startingDialogueID);

                if (startIndex != -1)
                {
                    currentIndex = startIndex;
                }
                else
                {
                    Debug.LogWarning($"Starting dialogue ID '{startingDialogueID}' not found! Starting from index 0.");
                    currentIndex = 0;
                }
            }
        }

        private void OnInteract()
        {
            if (hasChosen) return;

            if (speakerName == null || dialogueText == null)
            {
                Debug.LogWarning("TMP references not assigned in inspector! >:(");
                return;
            }

            if (dialogueCollection == null)
            {
                Debug.LogWarning("Dialogue Collection not assigned in inspector! >:(");
                return;
            }

            if (dialogueCollection == null || dialogueCollection.DialogueData == null || dialogueCollection.DialogueData.Count == 0)
            {
                Debug.LogWarning("No dialogue lines found in the collection!");
                return;
            }

            if (currentIndex >= dialogueCollection.DialogueData.Count)
            {
                currentIndex = 0;
                return;
            }

            DisplayCurrentDialogue();
        }

        private void DisplayCurrentDialogue()
        {
            DialogueData currentDialogue = dialogueCollection.DialogueData[currentIndex];

            SetSprite(currentDialogue.DisplayName);

            speakerName.text = currentDialogue.DisplayName;
            dialogueText.text = currentDialogue.DialogueLine;


            if (currentDialogue.IsEnd)
            {
                CloseDialogue();
                return;
            }

            if (currentDialogue.options != null && currentDialogue.options.Count > 0)
            {
                ShowDialogueOptions(currentDialogue.options);
            }
            else
            {
                HideOptions();
                NextDialogue(currentDialogue);
            }
        }

        private void SetSprite(string sprite)
        {
            if (speakerIcon == null) return;

            Sprite spriteToUse = sprite.ToLower() switch
            {
                // T E M P O R A R Y
                "jubertpandecoco" => spritePandecoco,
                "hubertpandesal" => spritePandesal,
                _ => null
            };

            if (spriteToUse != null)
            {
                speakerIcon.sprite = spriteToUse;
            }
        }

        private void ShowDialogueOptions(List<DialogueOptions> options)
        {
            hasChosen = true;
            optionsPanel.SetActive(true);

            ClearBTNS();

            foreach (DialogueOptions option in options)
            {
                GameObject optionBTN = Instantiate(optionBTNPrefab, optionsPanel.transform);
                activeOptionBTNS.Add(optionBTN);

                TextMeshProUGUI optionBTNText = optionBTN.GetComponentInChildren<TextMeshProUGUI>();
                if (optionBTNText != null)
                {
                    optionBTNText.text = option.OptionTexts;
                }

                Button btn = optionBTN.GetComponent<Button>();
                if (btn != null)
                {
                    string connectingID = option.ConnectingLineIDs;
                    btn.onClick.AddListener(() => OnOptionSelected(connectingID));
                }
            }
        }

        private void OnOptionSelected(string connectingDialogueID)
        {
            hasChosen = false;
            HideOptions();

            int nextIndex = dialogueCollection.DialogueData.FindIndex(dialogue => dialogue.DialogueID == connectingDialogueID);

            if (nextIndex != -1)
            {
                currentIndex = nextIndex;
                DisplayCurrentDialogue();
            }
            else
            {
                Debug.LogWarning($"Dialogue with ID '{connectingDialogueID}' not found in container!");
            }
        }

        private void HideOptions()
        {
            if (optionsPanel != null)
            {
                optionsPanel.SetActive(false);
            }

            ClearBTNS();
        }

        private void ClearBTNS()
        {
            foreach (GameObject btn in activeOptionBTNS)
            {
                Destroy(btn);
            }
            activeOptionBTNS.Clear();
        }

        private void NextDialogue(DialogueData currentDialogue)
        {
            if (!string.IsNullOrEmpty(currentDialogue.NextLineID))
            {
                int nextIndex = dialogueCollection.DialogueData.FindIndex(dialogue => dialogue.DialogueID == currentDialogue.NextLineID);

                if (nextIndex != -1)
                {
                    currentIndex = nextIndex;
                }
                else
                {
                    currentIndex++;
                }
            }
            else
            {
                currentIndex++;
            }
        }

        private void CloseDialogue()
        {
            if (dialogueBox != null)
            {
                dialogueBox.SetActive(false);
            }

            HideOptions();
            currentIndex = 0; // Reset for next time

            if (stateManager != null)
            {
                stateManager.EndDialogue();
            }
        }

        public void OpenDialogue()
        {
            if (dialogueBox != null)
            {
                dialogueBox.SetActive(true);
            }

            DisplayCurrentDialogue();
        }

        public void StartDialogueFromID(string dialogueID)
        {
            int startIndex = dialogueCollection.DialogueData.FindIndex(dialogue => dialogue.DialogueID == dialogueID);

            if (startIndex != -1)
            {
                currentIndex = startIndex;
                DisplayCurrentDialogue();
            }
            else
            {
                Debug.LogWarning($"Dialogue ID '{dialogueID}' not found in container!");
            }
        }
    }
}
