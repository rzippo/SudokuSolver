using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SudokuSolverLibrary
{
    public class SudokuCell
    {
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
        public IList<int> Candidates { get; private set; } = Enumerable.Range(1, 9).ToList();

        [JsonProperty("row")]
        public int Row { get; set; }

        [JsonProperty("column")]
        public int Column { get; set; }
        
        public void Clear()
        {
            Value = null;
            IsDetermined = false;
            Candidates = Enumerable.Range(1, 9).ToList();
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
