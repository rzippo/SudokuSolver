using System;
using System.Collections.Generic;
using System.Linq;

namespace SudokuSolverLibrary
{
    public partial class SudokuBoard
    {
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

                    SudokuCell cell = Matrix[row, column];
                    if ( cell.IsDetermined)
                        Console.Write(cell.Value);
                    else
                        Console.Write(' ');
                }

                Console.WriteLine(" |");
            }
            Console.WriteLine(sepLine);
            Console.WriteLine();
            if (!IsLegal)
                Console.WriteLine("This sudoku is not legal!");
            Console.WriteLine("This sudoku is " + (IsSolved ? "solved" : "not solved"));
        }

        public void PrintCell(int cellRow, int cellColumn)
        {
            SudokuCell cell = Matrix[cellRow, cellColumn];
            Console.WriteLine($"Cell [{cellRow},{cellColumn}]:");
            Console.WriteLine($"\tDetermined: {cell.IsDetermined}");
            Console.WriteLine($"\tValue: {cell.Value}");
            Console.WriteLine($"\tPossible values: {string.Join(",", cell.Candidates)}");
        }

        public void ClearBoard()
        {
            foreach (var cell in Matrix)
                cell.Clear();
            if (Puzzle != null)
                LoadPuzzle();
        }

        public void ClearCell(
            int cellRow,
            int cellColumn)
        {
            Matrix[cellRow, cellColumn].Clear();
            RecomputeCandidates();
        }

        public void SetCell(
            int cellRow,
            int cellColumn,
            int valueToSet)
        {
            SudokuCell cell = Matrix[cellRow, cellColumn];
            cell.Value = valueToSet;
            UpdateBoardCandidates(
                sourceRow: cellRow,
                sourceColumn: cellColumn,
                valueToRemove: valueToSet);
        }
    }
}
