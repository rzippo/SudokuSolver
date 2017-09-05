
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.Logic
{
    public partial class SudokuBoard
    {
        public int SetHiddenCandidateCells()
        {
            List<List<SudokuCell>> sudokuGroups = new List<List<SudokuCell>>();
            sudokuGroups.AddRange(rows);
            sudokuGroups.AddRange(columns);
            sudokuGroups.AddRange(tiles.Cast <List<SudokuCell>>());

            return sudokuGroups.Sum(SetGroupHiddenCandidates);
        }

        private int SetGroupHiddenCandidates(List<SudokuCell> cellGroup)
        {
            int nCellsSet = 0;
            IEnumerable<int> hiddenCandidates = sudokuValues.Where(
                v => cellGroup.Count(cell => cell.Candidates.Contains(v)) == 1
            );
            foreach (int hiddenCandidate in hiddenCandidates)
            {
                SudokuCell cell = cellGroup.Where(c => c.Candidates.Contains(hiddenCandidate)).ElementAt(0);
                if (!cell.IsDetermined)
                {
                    cell.Set(hiddenCandidate);
                    UpdateBoardCandidates(
                        sourceRow: cell.Row,
                        sourceColumn: cell.Column,
                        valueToRemove: (int) cell.Value);
                    nCellsSet++;
                }
            }
            return nCellsSet;
        }
    }
}
