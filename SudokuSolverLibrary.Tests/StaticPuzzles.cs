using System;
using System.Collections.Generic;
using System.Text;

namespace SudokuSolverLibrary.Tests
{
    static class StaticPuzzles
    {
        public static SudokuPuzzle[] Puzzles = new SudokuPuzzle[]
        {
            new SudokuPuzzle()
            {
                Name = "Hardest",
                Cells = new List<SudokuCell>()
                {
                    new SudokuCell()
                    {
                        Row = 0,
                        Column = 0,
                        Value = 8
                    },
                    new SudokuCell()
                    {
                        Row = 1,
                        Column = 2,
                        Value = 3
                    },
                    new SudokuCell()
                    {
                        Row = 1,
                        Column = 3,
                        Value = 6
                    },
                    new SudokuCell()
                    {
                        Row = 2,
                        Column = 1,
                        Value = 7
                    },
                    new SudokuCell()
                    {
                        Row = 2,
                        Column = 4,
                        Value = 9
                    },
                    new SudokuCell()
                    {
                        Row = 2,
                        Column = 6,
                        Value = 2
                    },
                    new SudokuCell()
                    {
                        Row = 3,
                        Column = 1,
                        Value = 5
                    },
                    new SudokuCell()
                    {
                        Row = 3,
                        Column = 5,
                        Value = 7
                    },
                    new SudokuCell()
                    {
                        Row = 4,
                        Column = 4,
                        Value = 4
                    },
                    new SudokuCell()
                    {
                        Row = 4,
                        Column = 5,
                        Value = 5
                    },
                    new SudokuCell()
                    {
                        Row = 4,
                        Column = 6,
                        Value = 7
                    },
                    new SudokuCell()
                    {
                        Row = 5,
                        Column = 3,
                        Value = 1
                    },
                    new SudokuCell()
                    {
                        Row = 5,
                        Column = 7,
                        Value = 3
                    },
                    new SudokuCell()
                    {
                        Row = 6,
                        Column = 2,
                        Value = 1
                    },
                    new SudokuCell()
                    {
                        Row = 6,
                        Column = 7,
                        Value = 6
                    },
                    new SudokuCell()
                    {
                        Row = 6,
                        Column = 8,
                        Value = 8
                    },
                    new SudokuCell()
                    {
                        Row = 7,
                        Column = 2,
                        Value = 8
                    },
                    new SudokuCell()
                    {
                        Row = 7,
                        Column = 3,
                        Value = 5
                    },
                    new SudokuCell()
                    {
                        Row = 7,
                        Column = 7,
                        Value = 1
                    },
                    new SudokuCell()
                    {
                        Row = 8,
                        Column = 1,
                        Value = 9
                    },
                    new SudokuCell()
                    {
                        Row = 8,
                        Column = 6,
                        Value = 4
                    },
                }
            }
        };
    }
}
