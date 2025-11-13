using UnityEngine;
using UnityEditor.AssetImporters;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using Core.Data;

namespace Core.DataHandling
{
    [ScriptedImporter(1, "tsv")]
    public class TSVImporter : ScriptedImporter
    {
        public override void OnImportAsset(AssetImportContext ctx)
        {
            Debug.Log("Importing TSV: " + ctx.assetPath);
            ITSVData asset = null;
            if (ctx.assetPath.ToLower().Contains("sample_dialogue"))
            {
                asset = ScriptableObject.CreateInstance<SampleDialogueCollection>();
            }

            if (asset == null)
            {
                Debug.LogError($"Unknown TSV file content: '{ctx.assetPath}'");
                return;
            }

            var content = File.ReadAllText(ctx.assetPath);
            content = content.Replace("    ", "\t");
            var lines = new List<TSVLine>();
            var raw = content.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            raw.RemoveAll(p => p.StartsWith("#"));
            var allLines = raw.ToArray();

            foreach (var line in allLines)
            {
                try
                {
                    var tsvLine = new TSVLine();
                    tsvLine.Items = new List<string>();

                    var items = line.Split('\t');
                    tsvLine.Items.AddRange(items);
                    lines.Add(tsvLine);
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Error processing line in '{ctx.assetPath}': {ex.Message}");
                }
            }

            //bool parseSuccess = false;
            try
            {
                asset.OnImport(lines);
                asset.Parse(lines);
                //parseSuccess = true;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Initial parse failed for '{ctx.assetPath}': {ex.Message}");
            }

            ctx.AddObjectToAsset("main obj", asset as ScriptableObject);
            ctx.SetMainObject(asset as ScriptableObject);
        }
    }
}
