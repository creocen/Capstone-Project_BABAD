using UnityEngine;
using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace Core.Data
{
    [CreateAssetMenu(fileName = "Dialogue Container", menuName = "Data Container/Dialogue Data")]
    public class DialogueSO : SerializedScriptableObject
    {
        // sample
        [TabGroup("TSV")] public SampleDialogueCollection SampleDialogueCollection;

        public List<DialogueData> DialogueDataList => SampleDialogueCollection.DialogueData;
    }

    // [ B A C K L O G ] Add actual sprite data to this model

    [Serializable]
    public class DialogueData
    {
        public string DialogueID;
        public string DisplayName;
        public string DialogueLine;
        public string NextLineID;
        public List<DialogueOptions> options = new List<DialogueOptions>();
        public CharacterEmotion characterEmotion;
        public DialogueType dialogueType;
        public bool HidePortrait;
        public bool HideDisplayName;
        public bool IsEnd;

    }

    [Serializable]
    public class DialogueOptions
    {
        public string OptionTexts;
        public string ConnectingLineIDs;
    }

    public enum CharacterEmotion
    {
        //insert emotions here idk wat to put lol
        joy,
        sadness,
        anger
    }

    public enum DialogueType
    {
        Default,
        World_Space
    }


}
