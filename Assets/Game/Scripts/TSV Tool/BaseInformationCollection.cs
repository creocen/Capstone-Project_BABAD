using UnityEngine;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Core.DataHandling;

namespace Core.Data
{
    public class BaseInformationCollection : SerializedScriptableObject, ITSVData
    {
        protected bool ShouldLoop;

        public List<TSVLine> rawLines = new List<TSVLine>();

        public virtual void Parse(List<TSVLine> lines)
        {
            throw new System.NotImplementedException();
        }

        public virtual void OnImport(List<TSVLine> lines)
        {
            rawLines = lines.ToList();
        }
    }
}


