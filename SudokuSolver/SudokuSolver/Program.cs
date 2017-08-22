using SudokuSolver.SolverLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    class Program
    {
        static void Main(string[] args)
        {
            SudokuMatrix matrix = new SudokuMatrix();

            matrix.setCell(0, 0, 3);
            matrix.setCell(2, 2, 5);
            matrix.setCell(6, 4, 8);

            matrix.print();
        }
    }
}
