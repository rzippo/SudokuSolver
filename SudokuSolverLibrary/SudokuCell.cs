using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace SudokuSolverLibrary
{
    public class SudokuCell : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        [JsonIgnore]
        public bool IsDetermined { get; private set; }

        private int? _value;

        [JsonProperty("value")]
        public int? Value {
            get => _value;
            set {
                _value = value;

                if (value != null)
                {
                    IsDetermined = true;
                    Candidates.Clear();
                    Candidates.Add((int)value);
                    NotifyPropertyChanged();
                }
                else
                {
                    IsDetermined = false;
                    Candidates = new SortedSet<int>(Enumerable.Range(1, 9));
                }
            }
        }

        [JsonIgnore]
        public SortedSet<int> Candidates { get; private set; } = new SortedSet<int>(Enumerable.Range(1, 9));

        [JsonProperty("row")]
        public int Row { get; set; }

        [JsonProperty("column")]
        public int Column { get; set; }
        
        public void Clear() => Value = null;

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
