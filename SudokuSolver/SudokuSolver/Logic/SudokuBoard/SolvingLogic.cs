
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.Logic
{
    public partial class SudokuBoard
    {
        public void Solve()
        {
            int lastCycleSetEvents;
            do
            {
                while (HasNakedCandidate())
                    SetNakedCandidateCells();
                lastCycleSetEvents = SetHiddenCandidateCells();
            } while (!IsSolved() && lastCycleSetEvents > 0);
        }

        public bool IsSolved()
        {
            return
                TestGroupsSolved(rows) &&
                TestGroupsSolved(columns) &&
                TestGroupsSolved(tiles.Cast<IEnumerable<SudokuCell>>());

            bool TestGroupsSolved(IEnumerable<IEnumerable<SudokuCell>> cellGroups)
            {
                return cellGroups.All(
                    cellGroup => sudokuValues.All(
                        value => cellGroup.Any(
                            cell => cell.Value == value)));
            }
        }

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

        public int SetHiddenCandidateCells()
        {
            List<List<SudokuCell>> sudokuGroups = new List<List<SudokuCell>>();
            sudokuGroups.AddRange(rows);
            sudokuGroups.AddRange(columns);
            sudokuGroups.AddRange(tiles.Cast<List<SudokuCell>>());

            return sudokuGroups.Sum(SetGroupHiddenCandidates);

            int SetGroupHiddenCandidates(List<SudokuCell> sudokuGroup)
            {
                int nCellsSet = 0;
                IEnumerable<int> hiddenCandidates = sudokuValues.Where(
                    v => sudokuGroup.Count(cell => cell.Candidates.Contains(v)) == 1
                );
                foreach (int hiddenCandidate in hiddenCandidates)
                {
                    SudokuCell cell = sudokuGroup.First(c => c.Candidates.Contains(hiddenCandidate));
                    if (!cell.IsDetermined)
                    {
                        cell.Set(hiddenCandidate);
                        UpdateBoardCandidates(
                            sourceRow: cell.Row,
                            sourceColumn: cell.Column,
                            valueToRemove: (int)cell.Value);
                        nCellsSet++;
                    }
                }
                return nCellsSet;
            }
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
