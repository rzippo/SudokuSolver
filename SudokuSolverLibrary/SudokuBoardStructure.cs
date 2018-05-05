using System;
using System.Collections.Generic;
using System.Linq;

namespace SudokuSolverLibrary
{
    public partial class SudokuBoard
    {
        private static readonly List<int> sudokuValues = Enumerable.Range(1, 9).ToList();

        private readonly SudokuCell[,] cellMatrix = new SudokuCell[9, 9];
        private readonly List<SudokuCell>[] rows;
        private readonly List<SudokuCell>[] columns;
        private readonly List<SudokuCell>[,] tiles;
        private readonly List<List<SudokuCell>> combinedGroups;

        public SudokuBoard()
        {
            for (int row = 0; row < 9; row++)
                for (int column = 0; column < 9; column++)
                    cellMatrix[row, column] = new SudokuCell(row, column);

            rows = GatherRows();
            columns = GatherColumns();
            tiles = GatherTiles();
            combinedGroups = new List<List<SudokuCell>>();
            combinedGroups.AddRange(rows);
            combinedGroups.AddRange(columns);
            combinedGroups.AddRange(tiles.Cast<List<SudokuCell>>());
        }

        private void CopyBoard(SudokuBoard other)
        {
            ClearBoard();
            other.cellMatrix.Cast<SudokuCell>()
                .Where(cell => cell.IsDetermined)
                .ToList()
                .ForEach(cell => this.SetCell(
                           cellRow: cell.Row,
                           cellColumn: cell.Column,
                           valueToSet: (int)cell.Value));
        }

        private List<SudokuCell>[] GatherRows()
        {
            List<SudokuCell>[] rows = new List<SudokuCell>[9];
            for (int rowIndex = 0; rowIndex < 9; rowIndex++)
            {
                rows[rowIndex] = new List<SudokuCell>();
                for (int columnIndex = 0; columnIndex < 9; columnIndex++)
                    rows[rowIndex].Add(cellMatrix[rowIndex, columnIndex]);
            }
            return rows;
        }

        private List<SudokuCell>[] GatherColumns()
        {
            List<SudokuCell>[] columns = new List<SudokuCell>[9];
            for (int columnIndex = 0; columnIndex < 9; columnIndex++)
            {
                columns[columnIndex] = new List<SudokuCell>();
                for (int rowIndex = 0; rowIndex < 9; rowIndex++)
                    columns[columnIndex].Add(cellMatrix[rowIndex, columnIndex]);
            }
            return columns;
        }

        private List<SudokuCell>[,] GatherTiles()
        {
            List<SudokuCell>[,] tiles = new List<SudokuCell>[3, 3];
            for (int tileVIndex = 0; tileVIndex < 3; tileVIndex++)
            {
                for (int tileHIndex = 0; tileHIndex < 3; tileHIndex++)
                {
                    tiles[tileVIndex, tileHIndex] = new List<SudokuCell>();
                    for (int rowOffset = 0; rowOffset < 3; rowOffset++)
                    {
                        for (int columnOffset = 0; columnOffset < 3; columnOffset++)
                        {
                            int row = tileVIndex * 3 + rowOffset;
                            int column = tileHIndex * 3 + columnOffset;
                            tiles[tileVIndex, tileHIndex].Add(cellMatrix[row, column]);
                        }
                    }
                }
            }
            return tiles;
        }
    }
}
