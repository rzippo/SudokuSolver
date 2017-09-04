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
