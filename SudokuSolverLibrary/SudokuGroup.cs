using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolverLibrary
{
    class SudokuGroup
    {
        public SudokuCell[] Cells;

        public SudokuGroup(IEnumerable<SudokuCell> cells)
        {
            Cells = cells.ToArray();
        }

        public static implicit operator SudokuCell[](SudokuGroup group)
        {
            return group.Cells;
        }
    }
}
