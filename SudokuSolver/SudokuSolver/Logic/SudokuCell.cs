using System;
using System.Collections.Generic;
using System.Linq;

namespace SudokuSolver.Logic
{
    internal class SudokuCell
    {
        public bool IsDetermined { get; private set; }
        public int? Value;
        public IList<int> PossibleValues = Enumerable.Range(1, 9).ToList();

        public void Set(int valueToSet)
        {
            Value = valueToSet;
            IsDetermined = true;
            PossibleValues.Clear();
            PossibleValues.Add(valueToSet);
        }

        public void Clear()
        {
            Value = null;
            IsDetermined = false;
            PossibleValues = Enumerable.Range(1, 9).ToList();
        }

        public void RemovePossibleValue( int valueToRemove )
        {
            if (!IsDetermined)
                PossibleValues.Remove(valueToRemove);
        }

        public bool IsDeterminable()
        {
            return !IsDetermined && PossibleValues.Count == 1;
        }
    }
}
