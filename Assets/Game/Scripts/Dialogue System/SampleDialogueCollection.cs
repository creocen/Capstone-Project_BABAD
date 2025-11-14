using UnityEngine;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Core.DataHandling;

namespace Core.Data
{
    public class SampleDialogueCollection : BaseInformationCollection
    {
        public List<DialogueData> DialogueData = new List<DialogueData>();

        public override void OnImport(List<TSVLine> lines)
        {
            base.OnImport(lines);
        }

        public override void Parse(List<TSVLine> lines)
        {
            var prev = DialogueData;

            DialogueData = new List<DialogueData>();

            foreach( var line in lines )
            {
                if (line.Items.Count < 3) continue; // Skip lines that don't have atleast 3 columns

                
                var id = line.Items[0];
                Enum.TryParse<CharacterEmotion>(line.Items[6], true, out CharacterEmotion emotion);
                Enum.TryParse<DialogueType>(line.Items[7], true, out DialogueType dialogueType);
                var hidePortrait = line.Items.Count >= 8 ? line.Items[8].ToLower().Trim() : "";
                var hideDisplayName = line.Items.Count >= 9 ? line.Items[9].ToLower().Trim() : "";
                var isEnd = line.Items.Count >= 10 ? line.Items[10].ToLower().Trim() : "";

                var importedData = prev.FirstOrDefault(p => p.DialogueID == id);

                if (importedData == null) { importedData = new DialogueData(); }

                string[] coptions = string.IsNullOrEmpty(line.Items[4]) ? new string[0] : line.Items[4].Split('|');
                string[] cconnection = string.IsNullOrEmpty(line.Items[5]) ? new string[0] : line.Items[5].Split('|');

                importedData.DialogueID = id;
                importedData.DisplayName = line.Items[1];
                importedData.DialogueLine = line.Items[2];
                importedData.NextLineID = line.Items[3];
                importedData.characterEmotion = emotion;
                importedData.dialogueType = dialogueType;
                importedData.HidePortrait = hidePortrait == "true" || hidePortrait == "yes" || hidePortrait == "1";
                importedData.HideDisplayName = hideDisplayName == "true" || hideDisplayName == "yes" || hideDisplayName == "1";
                importedData.IsEnd = isEnd == "true" || isEnd == "yes" || isEnd == "1";


                for (int i = 0; i < coptions.Length; i++)
                {
                    DialogueOptions option = new DialogueOptions
                    {
                        OptionTexts = coptions[i],
                        ConnectingLineIDs = i < cconnection.Length ? cconnection[i] : ""
                    };
                    importedData.options.Add(option);
                }

                DialogueData.Add(importedData);
            }

            if (prev.Count > 0)
            {
                DialogueData.RemoveAll(p => !prev.Contains(p));
            }
        }
    }
}
