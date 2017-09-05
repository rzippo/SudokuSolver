using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.Logic
{
    public partial class SudokuBoard
    {
        public int SetNakedCandidateCells()
        {
            int nCellsSet = 0;

            cellMatrix.Cast<SudokuCell>()
                .Where(cell => cell.HasNakedCandidate())
                .ToList()
                .ForEach(
                    cell =>
                    {
                        SetCell(
                            cellRow: cell.Row,
                            cellColumn: cell.Column,
                            valueToSet: cell.Candidates[0]);
                        nCellsSet++;
                    });
            
            return nCellsSet;
        }

        public void RecomputeCandidates()
        {
            cellMatrix.Cast<SudokuCell>()
                .Where(cell => !cell.IsDetermined)
                .ToList()
                .ForEach(cell => cell.Clear());
            
            cellMatrix.Cast<SudokuCell>()
                .Where(cell => cell.IsDetermined)
                .ToList()
                .ForEach(cell => UpdateBoardCandidates(
                    sourceRow: cell.Row,
                    sourceColumn: cell.Column,
                    valueToRemove: (int) cell.Value));
        }

        private void UpdateBoardCandidates(
            int sourceRow,
            int sourceColumn,
            int valueToRemove)
        {
            rows[sourceRow]
                .ForEach(cell => cell.RemoveCandidate(valueToRemove));

            columns[sourceColumn]
                .ForEach(cell => cell.RemoveCandidate(valueToRemove));

            tiles[sourceRow / 3, sourceColumn / 3]
                .ForEach(cell => cell.RemoveCandidate(valueToRemove));
        }
    }
}
