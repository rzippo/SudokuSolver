﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.SolverLogic
{
    public enum CellState { determined, undetermined };

    class SudokuCell
    {
        public CellState state = CellState.undetermined;
        public int? value = null;
        public IList<int> possibleValues = Enumerable.Range(1, 9).ToList<int>();

        public void set(int value)
        {
            this.value = value;
            this.state = CellState.determined;
            this.possibleValues.Clear();
            this.possibleValues.Add(value);
        }
    }
}
