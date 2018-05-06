using System;
using System.Collections.Generic;
using System.ComponentModel;
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
            foreach (var cell in Cells)
                cell.PropertyChanged += UpdateCandidates;
        }

        private void UpdateCandidates(object sender, PropertyChangedEventArgs e)
        {
            SudokuCell updatedCell = sender as SudokuCell;
            int newValue = (int) updatedCell.Value;
            foreach (var cell in Cells)
                if (cell != updatedCell)
                    cell.RemoveCandidate(newValue);
        }

        public static implicit operator SudokuCell[](SudokuGroup group)
        {
            return group.Cells;
        }
    }
}
