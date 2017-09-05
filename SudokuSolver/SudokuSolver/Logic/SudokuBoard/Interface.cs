using System;
using System.Collections.Generic;
using System.Linq;

namespace SudokuSolver.Logic
{
    public partial class SudokuBoard
    {
        public bool HasNakedCandidate()
        {
            return cellMatrix.Cast<SudokuCell>()
                .Any(cell => cell.HasNakedCandidate());
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
            int lastCycleSetEvents;
            do
            {
                while(HasNakedCandidate())
                    SetNakedCandidateCells();
                lastCycleSetEvents = SetHiddenCandidateCells();
            } while ( !IsSolved() &&  lastCycleSetEvents > 0);
        }
    }
}
