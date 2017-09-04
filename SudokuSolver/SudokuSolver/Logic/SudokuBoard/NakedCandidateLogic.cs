using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.Logic
{
    public partial class SudokuBoard
    {
        public int SetNakedCandidateCells(
            bool repeatUntilPossible)
        {
            int nCellsSet = 0;

            for (int row = 0; row < 9; row++)
            {
                for (int column = 0; column < 9; column++)
                {
                    SudokuCell cell = cellMatrix[row, column];
                    if (cell.HasNakedCandidate())
                    {
                        SetCell(
                            cellRow: row,
                            cellColumn: column,
                            valueToSet: cell.Candidates[0]);
                        nCellsSet++;
                    }
                }
            }

            if (repeatUntilPossible && HasNakedCandidate())
                nCellsSet += SetNakedCandidateCells(repeatUntilPossible: true);

            return nCellsSet;
        }

        public void RecomputeCandidates()
        {
            for (int row = 0; row < 9; row++)
            {
                for (int column = 0; column < 9; column++)
                {
                    SudokuCell cell = cellMatrix[row, column];
                    if (!cell.IsDetermined)
                        cell.Clear();
                }
            }

            for (int row = 0; row < 9; row++)
            {
                for (int column = 0; column < 9; column++)
                {
                    SudokuCell cell = cellMatrix[row, column];
                    if (cell.IsDetermined)
                        UpdateBoardCandidates(
                            sourceRow: row,
                            sourceColumn: column,
                            valueToRemove: (int)cell.Value);
                }
            }
        }

        private void UpdateBoardCandidates(
            int sourceRow,
            int sourceColumn,
            int valueToRemove)
        {
            UpdateRowCandidates(
                sourceRow: sourceRow,
                valueToRemove: valueToRemove);

            UpdateColumnCandidates(
                sourceColumn: sourceColumn,
                valueToRemove: valueToRemove);

            UpdateTileCandidates(
                sourceRow: sourceRow,
                sourceColumn: sourceColumn,
                valueToRemove: valueToRemove);
        }

        private void UpdateRowCandidates(
            int sourceRow,
            int valueToRemove)
        {
            for (int column = 0; column < 9; column++)
            {
                SudokuCell cell = cellMatrix[sourceRow, column];
                cell.RemovePossibleValue(valueToRemove);
            }
        }

        private void UpdateColumnCandidates(
            int sourceColumn,
            int valueToRemove)
        {
            for (int row = 0; row < 9; row++)
            {
                SudokuCell cell = cellMatrix[row, sourceColumn];
                cell.RemovePossibleValue(valueToRemove);
            }
        }

        private void UpdateTileCandidates(
            int sourceRow,
            int sourceColumn,
            int valueToRemove)
        {
            int tileBaseRow = (sourceRow / 3) * 3;
            int tileBaseColumn = (sourceColumn / 3) * 3;

            for (int rowOffset = 0; rowOffset < 3; rowOffset++)
            {
                for (int columnOffset = 0; columnOffset < 3; columnOffset++)
                {
                    int row = tileBaseRow + rowOffset;
                    int column = tileBaseColumn + columnOffset;
                    SudokuCell cell = cellMatrix[row, column];
                    cell.RemovePossibleValue(valueToRemove);
                }
            }
        }
    }
}
