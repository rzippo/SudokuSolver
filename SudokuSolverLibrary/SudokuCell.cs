using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SudokuSolverLibrary
{
    public class SudokuCell
    {
        [JsonIgnore]
        public bool IsDetermined { get; private set; }

        private int? _value;

        [JsonProperty("value")]
        public int? Value {
            get => _value;
            set {
                if (value == null)
                    return;

                _value = value;
                IsDetermined = true;
                Candidates.Clear();
                Candidates.Add((int)value);
            }
        }

        [JsonIgnore]
        public SortedSet<int> Candidates { get; private set; } = new SortedSet<int>(Enumerable.Range(1, 9));

        [JsonProperty("row")]
        public int Row { get; set; }

        [JsonProperty("column")]
        public int Column { get; set; }
        
        public void Clear()
        {
            Value = null;
            IsDetermined = false;
            Candidates = new SortedSet<int>(Enumerable.Range(1, 9));
        }

        public void RemoveCandidate( int valueToRemove )
        {
            if (!IsDetermined)
                Candidates.Remove(valueToRemove);
        }

        public bool HasNakedCandidate()
        {
            return !IsDetermined && Candidates.Count == 1;
        }
    }
}
