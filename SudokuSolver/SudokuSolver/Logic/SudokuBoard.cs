﻿using System;

namespace SudokuSolver.Logic
{
    public class SudokuBoard
    {
        private readonly SudokuCell[,] cellMatrix = new SudokuCell[9, 9];
        
        public SudokuBoard()
        {
            for (int row = 0; row < 9; row++)
                for (int column = 0; column < 9; column++)
                    cellMatrix[row, column] = new SudokuCell();
        }

        public void PrintBoard()
        {
            const string sepLine = "  ——————— ——————— ——————— ";

            for (int row = 0; row < 9; row++)
            {
                if (row % 3 == 0)
                    Console.WriteLine(sepLine);

                for (int column = 0; column < 9; column++)
                {
                    if (column % 3 == 0)
                        Console.Write(" |");

                    Console.Write(' ');

                    SudokuCell cell = cellMatrix[row, column];
                    if ( cell.IsDetermined)
                        Console.Write(cell.Value);
                    else
                        Console.Write(' ');
                }

                Console.WriteLine(" |");
            }
            Console.WriteLine(sepLine);
        }

        public void PrintCell(int cellRow, int cellColumn)
        {
            SudokuCell cell = cellMatrix[cellRow, cellColumn];
            Console.WriteLine($"Cell [{cellRow},{cellColumn}]:");
            Console.WriteLine($"\tDetermined: {cell.IsDetermined}");
            Console.WriteLine($"\tValue: {cell.Value}");
            Console.WriteLine($"\tPossible values: {string.Join(",", cell.PossibleValues)}");
        }

        public void ClearBoard()
        {
            for (int row = 0; row < 9; row++)
            for (int column = 0; column < 9; column++)
                cellMatrix[row, column].Clear();
        }

        public void ClearCell(
            int cellRow,
            int cellColumn)
        {
            cellMatrix[cellRow, cellColumn].Clear();
            RecomputePossibleValues();
        }

        public void SetCell(
            int cellRow,
            int cellColumn,
            int valueToSet,
            out bool isThereDeterminableCell)
        {
            SudokuCell cell = cellMatrix[cellRow, cellColumn];
            cell.Set(valueToSet);
            UpdateBoardPossibleValues(
                sourceRow: cellRow,
                sourceColumn: cellColumn,
                valueToRemove: valueToSet,
                isThereDeterminableCell: out isThereDeterminableCell);
        }

        public void SetDeterminableCells(
            bool repeatUntilPossible,
            out int nCellsSet)
        {
            bool isThereDeterminableCell = false;
            nCellsSet = 0;

            for (int row = 0; row < 9; row++)
            {
                for (int column = 0; column < 9; column++)
                {
                    SudokuCell cell = cellMatrix[row, column];
                    if (cell.IsDeterminable())
                    {
                        SetCell(
                            cellRow: row,
                            cellColumn: column,
                            valueToSet: cell.PossibleValues[0],
                            isThereDeterminableCell: out bool localIsThereDeterminableCell);
                        isThereDeterminableCell |= localIsThereDeterminableCell;
                        nCellsSet++;
                    }
                }
            }

            if (repeatUntilPossible && isThereDeterminableCell)
            {
                SetDeterminableCells(
                    repeatUntilPossible: true,
                    nCellsSet: out int nestedNCellsSet);
                nCellsSet += nestedNCellsSet;
            }
        }

        public void RecomputePossibleValues()
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
                        UpdateBoardPossibleValues(
                            sourceRow: row,
                            sourceColumn: column,
                            valueToRemove: (int) cell.Value,
                            isThereDeterminableCell: out bool unused);
                }
            }
        }

        private void UpdateBoardPossibleValues(
            int sourceRow,
            int sourceColumn,
            int valueToRemove,
            out bool isThereDeterminableCell)
        {
            isThereDeterminableCell = false;

            UpdateRowPossibleValues(
                sourceRow: sourceRow,
                valueToRemove: valueToRemove,
                isThereDeterminableCell: ref isThereDeterminableCell);

            UpdateColumnPossibleValues(
                sourceColumn: sourceColumn,
                valueToRemove: valueToRemove,
                isThereDeterminableCell: ref isThereDeterminableCell);

            UpdateTilePossibleValues(
                sourceRow: sourceRow,
                sourceColumn: sourceColumn,
                valueToRemove: valueToRemove,
                isThereDeterminableCell: ref isThereDeterminableCell);
        }

        private void UpdateRowPossibleValues(
            int sourceRow,
            int valueToRemove,
            ref bool isThereDeterminableCell)
        {
            for (int column = 0; column < 9; column++)
            {
                SudokuCell cell = cellMatrix[sourceRow, column];
                cell.RemovePossibleValue(valueToRemove);
                isThereDeterminableCell |= cell.IsDeterminable();
            }
        }

        private void UpdateColumnPossibleValues(
            int sourceColumn,
            int valueToRemove,
            ref bool isThereDeterminableCell)
        {
            for (int row = 0; row < 9; row++)
            {
                SudokuCell cell = cellMatrix[row, sourceColumn];
                cell.RemovePossibleValue(valueToRemove);
                isThereDeterminableCell |= cell.IsDeterminable();
            }
        }

        private void UpdateTilePossibleValues(
            int sourceRow,
            int sourceColumn,
            int valueToRemove,
            ref bool isThereDeterminableCell)
        {
            int tileBaseRow = ( sourceRow / 3 ) * 3;
            int tileBaseColumn = ( sourceColumn / 3 ) * 3;

            for (int rowOffset = 0; rowOffset < 3; rowOffset++)
            {
                for (int columnOffset = 0; columnOffset < 3; columnOffset++)
                {
                    int row = tileBaseRow + rowOffset;
                    int column = tileBaseColumn + columnOffset;
                    SudokuCell cell = cellMatrix[ row, column];
                    cell.RemovePossibleValue(valueToRemove);
                    isThereDeterminableCell |= cell.IsDeterminable();
                }
            }
        }
    }
}