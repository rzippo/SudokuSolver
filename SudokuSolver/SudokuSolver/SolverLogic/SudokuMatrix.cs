using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.SolverLogic
{
    class SudokuMatrix
    {
        SudokuCell[,] matrix = new SudokuCell[9, 9];
        
        public SudokuMatrix()
        {
            for (int row = 0; row < 9; row++)
                for (int column = 0; column < 9; column++)
                    matrix[row, column] = new SudokuCell();
        }

        public void print()
        {
            string sepLine = "  ——————— ——————— ——————— ";
            for (int row = 0; row < 9; row++)
            {
                if (row % 3 == 0)
                    Console.WriteLine(sepLine);
                for (int column = 0; column < 9; column++)
                {
                    if (column % 3 == 0)
                        Console.Write(" |");
                    Console.Write(' ');
                    if (matrix[row, column].state == CellState.determined)
                        Console.Write(matrix[row, column].value);
                    else
                        Console.Write(' ');
                }
                Console.WriteLine("|");
            }
            //Closing line
            Console.WriteLine(sepLine);
        }

        public void checkForSet()
        {
            for (int row = 0; row < 9; row++)
            {
                for (int column = 0; column < 9; column++)
                {
                    var cell = matrix[row, column];
                    if(cell.state == CellState.undetermined && cell.possibleValues.Count == 1)
                        setCell(row, column, cell.possibleValues[0]);
                }
            }
        }

        public void setCell(int row, int column, int value )
        {
            matrix[row, column].set(value);
            updateAfterSet(row, column, value);
        }

        private void updateAfterSet(int row, int column, int value)
        {
            updateRow(row, value);
            updateColumn(column, value);
            updateTile(row, column, value);
        }

        private void updateRow(int row, int value)
        {
            for (int column = 0; column < 9; column++)
            {
                var cell = matrix[row, column];
                if(cell.state == CellState.undetermined)
                    cell.possibleValues.Remove(value);
            }
        }

        private void updateColumn(int column, int value)
        {
            for (int row = 0; row < 9; row++)
            {
                var cell = matrix[row, column];
                if (cell.state == CellState.undetermined)
                    cell.possibleValues.Remove(value);
            }
        }

        private void updateTile(int row, int column, int value)
        {
            int tileBaseRow = ( row / 3 ) * 3;
            int tileBaseColumn = ( column / 3 ) * 3;

            for (int rowOffset = 0; rowOffset < 3; rowOffset++)
                for (int columnOffset = 0; columnOffset < 3; columnOffset++)
                {
                    var cell = matrix[tileBaseRow + rowOffset, tileBaseColumn + columnOffset];
                    if(cell.state == CellState.undetermined)
                        cell.possibleValues.Remove(value);
                }
        }
    }
}
