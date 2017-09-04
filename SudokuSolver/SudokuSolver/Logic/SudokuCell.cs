using System;
using System.Collections.Generic;
using System.Linq;

namespace SudokuSolver.Logic
{
    internal class SudokuCell
    {
        public bool IsDetermined { get; private set; }
        public int? Value;
        public IList<int> Candidates = Enumerable.Range(1, 9).ToList();

        public void Set(int valueToSet)
        {
            Value = valueToSet;
            IsDetermined = true;
            Candidates.Clear();
            Candidates.Add(valueToSet);
        }

        public void Clear()
        {
            Value = null;
            IsDetermined = false;
            Candidates = Enumerable.Range(1, 9).ToList();
        }

        public void RemovePossibleValue( int valueToRemove )
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
