using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Core.DataHandling
{
    public interface ITSVData 
    {
        void OnImport(List<TSVLine> lines); 
        void Parse(List<TSVLine> lines); 
    }

    public class TSVLine 
    {
        public List<string> Items; 
    }
}


