using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;

using Core.Data;

namespace Core.DataHandling
{
    public class TSVTool : OdinEditorWindow
    {
        [Serializable]
        public struct DownloadInfo
        {
            [LabelText("URLs")]
            [ListDrawerSettings(ShowIndexLabels = true)]
            public List<string> URLs;

            [LabelText("TSV Path")]
            public string TSVPath;

            public DownloadInfo(string url,string tsvPath)
            {
                URLs = new List<string> { url };
                TSVPath = tsvPath;
            }

            public DownloadInfo(List<string> urls, string tsvPath)
            {
                URLs = urls ?? new List<string>();
                TSVPath = tsvPath;
            }
        }

        [MenuItem("Utilities/TSV Data Download Tool")]
        private static void OpenWindow()
        {
            var tool = GetWindow<TSVTool>("TSV Tool");
        }

        public TSVSet CurrentTSVSet = new TSVSet();

        public class TSVSet
        {
            public virtual Dictionary<Type, DownloadInfo> downloadMap => new Dictionary<Type, DownloadInfo>
            {
                {
                    typeof(SampleDialogueCollection),
                    new DownloadInfo
                    (
                        "https://docs.google.com/spreadsheets/d/e/2PACX-1vTDMipFSjapqW2kJAk6oTAyPJQDU8wkTci_i9FAjpej0A5SsSFDovsxi8ZJRVsHMoM2kouvMPyT9DLF/pub?gid=345238535&single=true&output=tsv",
                        "Editor/TSV Files/Dialogue Sheets/sample_dialogue.tsv"
                    )
                }
            };

            [Button("Download All")]
            private void DownloadAll()
            {
                var count = 0;
                var total = 0;

                // kvp = key value pair?
                foreach (var kvp in downloadMap)
                {
                    total++;
                    Debug.Log($"Downloading {kvp.Key.Name}..."); // Key in this context = DataCollection
                    if (DownloadForType(kvp.Key))
                    {
                        count++;
                        Debug.Log($"Downloaded {kvp.Key.Name} successully");
                    }
                    else
                    {
                        Debug.LogError($"Download ERROR: {kvp.Key.Name}");
                    }
                }
                Debug.Log($"Downloaded {count}/{total} TSV files.");

                Assign();
            }

            public bool DownloadForType(Type type)
            {
#if UNITY_EDITOR
                if (!downloadMap.ContainsKey(type))
                {
                    Debug.LogError($"No download mapping found for: {type.Name}");
                    return false;
                }

                try
                {
                    var downloadInfo = downloadMap[type];
                    var results = new StringBuilder();
                    using (var webClient = new WebClient())
                    {
                        foreach (string url in downloadInfo.URLs)
                        {
                            if (!string.IsNullOrEmpty(url))
                            {
                                var result = webClient.DownloadString(url);
                                results.AppendLine(result);
                            }
                        }

                        string fullPath = Path.Combine(Application.dataPath, downloadInfo.TSVPath);
                        File.WriteAllText(fullPath, results.ToString());

                        Debug.Log($"Successfully downloaded TSV for {type.Name} to {downloadInfo.TSVPath}");

                        return true;
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError($"Failed to download TSV for {type.Name}: {e.Message}");

                    return false;
                }
#endif
            }

            private IEnumerable<Type> GetAvailableTypes()
            {
                return downloadMap.Keys;
            }

            [Button("Download Specific Type")]
            private void DownloadSpecificType([ValueDropdown("GetAvailableTypes")] Type type)
            {
                if (type != null)
                {
                    DownloadForType(type);
                }
                Assign();
            }

            [Button("Assign in Dialogue")]
            private void Assign()
            {

            }
        }
    }
}

