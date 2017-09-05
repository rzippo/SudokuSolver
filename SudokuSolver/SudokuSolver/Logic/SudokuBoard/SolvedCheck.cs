using System;
using System.Collections.Generic;
using System.Linq;

namespace SudokuSolver.Logic
{
    public partial class SudokuBoard
    {
        private static readonly List<int> sudokuValues = Enumerable.Range(1, 9).ToList();
        
        public bool IsSolved()
        {
            return 
                TestGroupsSolved(rows) && 
                TestGroupsSolved(columns) &&
                TestGroupsSolved(tiles.Cast<IEnumerable<SudokuCell>>());
        }

        private bool TestGroupsSolved(IEnumerable<IEnumerable<SudokuCell>> cellGroups)
        {
            return cellGroups.All(
                cellGroup => sudokuValues.All(
                    value => cellGroup.Any(
                        cell => cell.Value == value)));
        }
    }
}
