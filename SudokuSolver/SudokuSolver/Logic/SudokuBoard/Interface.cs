using System;
using System.Collections.Generic;
using System.Linq;

namespace SudokuSolver.Logic
{
    public partial class SudokuBoard
    {
        private static readonly List<int> sudokuValues = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        private readonly SudokuCell[,] cellMatrix = new SudokuCell[9, 9];

        public bool HasNakedCandidate()
        {
            return cellMatrix.Cast<SudokuCell>()
                .Any(cell => cell.HasNakedCandidate());
        }

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
            Console.WriteLine($"\tPossible values: {string.Join(",", cell.Candidates)}");
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
            RecomputeCandidates();
        }

        public void SetCell(
            int cellRow,
            int cellColumn,
            int valueToSet)
        {
            SudokuCell cell = cellMatrix[cellRow, cellColumn];
            cell.Set(valueToSet);
            UpdateBoardCandidates(
                sourceRow: cellRow,
                sourceColumn: cellColumn,
                valueToRemove: valueToSet);
        }

        public void Solve()
        {
            //Incomplete strategy
            SetNakedCandidateCells(true);
        }

        public bool IsSolved()
        {
            return TestRowSolved() && TestColumnsSolved() && TestTilesSolved();
        }

        private bool TestRowSolved()
        {
            for (int row = 0; row < 9; row++)
            {
                List<int> rowValues = new List<int>();
                for (int column = 0; column < 9; column++)
                {
                    SudokuCell cell = cellMatrix[row, column];
                    if (!cell.IsDetermined)
                        return false;
                    else
                        rowValues.Add((int) cell.Value);
                }

                if (sudokuValues.Any(v => !rowValues.Contains(v)))
                    return false;
                else
                    continue;
            }
            return true;
        }
        
        private bool TestColumnsSolved()
        {
            for (int column = 0; column < 9; column++)
            {
                List<int> columnValues = new List<int>();
                for (int row = 0; row < 9; row++)
                {
                    SudokuCell cell = cellMatrix[row, column];
                    if (!cell.IsDetermined)
                        return false;
                    else
                        columnValues.Add((int)cell.Value);
                }

                if (sudokuValues.Any(v => !columnValues.Contains(v)))
                    return false;
                else
                    continue;
            }
            return true;
        }

        private bool TestTilesSolved()
        {
            for (int tileVIndex = 0; tileVIndex < 3; tileVIndex++)
            {
                for (int tileHIndex = 0; tileHIndex < 3; tileHIndex++)
                {
                    List<int> tileValues = new List<int>();

                    for (int rowOffset = 0; rowOffset < 3; rowOffset++)
                    {
                        for (int columnOffset = 0; columnOffset < 3; columnOffset++)
                        {
                            int row = tileVIndex * 3 + rowOffset;
                            int column = tileHIndex * 3 + columnOffset;
                            SudokuCell cell = cellMatrix[row, column];
                            if (!cell.IsDetermined)
                                return false;
                            else
                                tileValues.Add((int)cell.Value);
                        }
                    }

                    if (sudokuValues.Any(v => !tileValues.Contains(v)))
                        return false;
                    else
                        continue;
                }
            }

            return true;
        }
    }
}
