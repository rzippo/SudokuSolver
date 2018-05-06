using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SudokuSolverLibrary
{
    public partial class SudokuBoard
    {
        private static readonly List<int> sudokuValues = Enumerable.Range(1, 9).ToList();

        [JsonProperty("matrix")]
        public readonly SudokuCell[,] Matrix = new SudokuCell[9, 9];

        public readonly IEnumerable<SudokuCell> MatrixIterator;
        
        private readonly SudokuGroup[] rows;
        private readonly SudokuGroup[] columns;
        private readonly SudokuGroup[,] tiles;
        private readonly List<SudokuGroup> combinedGroups;

        [JsonProperty("puzzle")]
        public SudokuPuzzle Puzzle { get; set; }

        public SudokuBoard()
        {
            for (int row = 0; row < 9; row++)
                for (int column = 0; column < 9; column++)
                    Matrix[row, column] = new SudokuCell()
                    {
                        Row = row,
                        Column = column
                    };

            MatrixIterator = Matrix.Cast<SudokuCell>();
            rows = GatherRows();
            columns = GatherColumns();
            tiles = GatherTiles();
            combinedGroups = new List<SudokuGroup>();
            combinedGroups.AddRange(rows);
            combinedGroups.AddRange(columns);
            combinedGroups.AddRange(tiles.Cast<SudokuGroup>());
        }

        public SudokuBoard(SudokuPuzzle puzzle) : this()
        {
            Puzzle = puzzle;
            LoadPuzzle();
        }

        public SudokuBoard(SudokuBoard copySource) : this()
        {
            Copy(copySource);
        }

        public void Copy(SudokuBoard source)
        {
            ClearBoard();
            source.MatrixIterator
                .Where(cell => cell.IsDetermined)
                .ToList()
                .ForEach(cell => this.SetCell(
                           cellRow: cell.Row,
                           cellColumn: cell.Column,
                           valueToSet: (int)cell.Value));
        }
        
        private SudokuGroup[] GatherRows()
        {
            SudokuGroup[] rows = new SudokuGroup[9];
            for (int rowIndex = 0; rowIndex < 9; rowIndex++)
            {
                rows[rowIndex] = new SudokuGroup(
                    MatrixIterator.AsParallel().Where(cell => cell.Row == rowIndex));                
            }
            return rows;
        }

        private SudokuGroup[] GatherColumns()
        {
            SudokuGroup[] columns = new SudokuGroup[9];
            for (int columnIndex = 0; columnIndex < 9; columnIndex++)
            {
                columns[columnIndex] = new SudokuGroup(
                    MatrixIterator.AsParallel().Where(cell => cell.Column == columnIndex));
            }
            return columns;
        }

        private SudokuGroup[,] GatherTiles()
        {
            SudokuGroup[,] tiles = new SudokuGroup[3, 3];
            for (int tileVIndex = 0; tileVIndex < 3; tileVIndex++)
            {
                for (int tileHIndex = 0; tileHIndex < 3; tileHIndex++)
                {
                    tiles[tileVIndex, tileHIndex] = new SudokuGroup(
                            MatrixIterator.AsParallel().Where(cell =>
                                ( cell.Row >= tileVIndex*3 && cell.Row < (tileVIndex + 1) * 3) &&
                                ( cell.Column >= tileHIndex * 3 && cell.Column < (tileHIndex + 1) * 3)
                            )
                        );
                }
            }
            return tiles;
        }

        private void LoadPuzzle()
        {
            if (Puzzle == null || Puzzle.Cells == null)
                return;

            foreach(var cell in Puzzle.Cells)
            {
                Matrix[cell.Row, cell.Column].Value = cell.Value;
            }
        }
    }
}
