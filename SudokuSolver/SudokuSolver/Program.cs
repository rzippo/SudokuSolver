using SudokuSolver.SolverLogic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    class Program
    {
        static SudokuMatrix sudoku = new SudokuMatrix();

        static void Main(string[] args)
        {
            commandLoop();
        }

        private static void commandLoop()
        {
            displayHelp();
            while(true)
            {
                Console.WriteLine("");
                Console.Write("Please write your command: ");

                string command = Console.ReadLine();
                Console.WriteLine("");

                switch(command[0])
                {
                    case 'h':
                        displayHelp();
                        break;

                    case 'p':
                        sudoku.print();
                        break;

                    case 'l':
                        Console.WriteLine("Load command, reading...");
                        string[] lwords = command.Split(null);
                        string filepath = lwords[1];
                        Console.WriteLine("The file path is: " + filepath);
                        loadFromFile(filepath);
                        break;

                    case 'i':
                        string[] iwords = command.Split(null);
                        int irow = int.Parse(iwords[1]);
                        int icolumn = int.Parse(iwords[2]);
                        int value = int.Parse(iwords[3]);
                        sudoku.setCell(irow, icolumn, value);
                        Console.WriteLine("Number " + value + " set in cell [" + irow + ',' + icolumn + "].");
                        break;

                    case 'c':
                        Console.WriteLine("Clear cell command, clearing and recomputing...");
                        string[] cwords = command.Split(null);
                        int crow = int.Parse(cwords[1]);
                        int ccolumn = int.Parse(cwords[2]);
                        sudoku.clearCell(crow, ccolumn);
                        Console.WriteLine("Clearing complete.");
                        break;

                    case 'u':
                        int updated = sudoku.setReadyCells(false);
                        Console.WriteLine("Update command executed. " + updated + " cells were set.");
                        break;

                    case 'r':
                        Console.WriteLine("Recompute command, recomputing possible values...");
                        sudoku.recomputePossibleValues();
                        Console.WriteLine("Recomputation completed");
                        break;

                    case 's':
                        Console.WriteLine("Solve command, processing...");
                        sudoku.setReadyCells(true);
                        Console.WriteLine("Solving completed, the resulting sudoku is: ");
                        sudoku.print();
                        break;

                    case 'q':
                        Console.WriteLine("Closing the program.");
                        return;

                    default:
                        Console.WriteLine("Command not recognized.");
                        break;
                }
            }
        }

        private static void displayHelp()
        {
            Console.Write("Welcome to SudokuSolver, small project by Raffaele Zippo.\n" +
                "Here are the available commands:\n" +
                "\t h \t\t\t\t Displays this message.\n" +
                "\t p \t\t\t\t Prints the current state of the sudoku.\n" +

                "\t l {filepath} \t\t\t [No spaces in the path!] Loads inputs from the specified file.\n" +
                "\t i {row} {column} {value} \t Sets a number in the sudoku.\n" +
                "\t c {row} {column} \t\t Clears the cell and recomputes possible values for all cells.\n" +

                "\t u \t\t\t\t Checks each cell, left to right and top to bottom, and sets it if there is only one possible value it can take\n" +
                "\t r \t\t\t\t Recomputes possible values for all cells.\n" +
                "\t s \t\t\t\t Tries to solve the sudoku, basically issuing u commands until necessary.\n" +

                "\t q \t\t\t\t Quits the program.\n\n" +
                "This program may crash in case of incorrect input :P");
            Console.WriteLine();
        }

        private static void loadFromFile(string filepath)
        {
            try
            {
                using (StreamReader sr = new StreamReader(filepath))
                {
                    int row = 0;
                    while (sr.Peek() != -1)
                    {
                        String line = sr.ReadLine();
                        if ( line.ToCharArray().Where(c => !char.IsWhiteSpace(c)).Count() > 0 )
                        {
                            var lineFields = line.Split(null).Where(n => n.Length > 0);
                            for (int column = 0; column < Math.Min(9, lineFields.Count()); column++)
                            {
                                int value;
                                int.TryParse(lineFields.ElementAt(column), out value);
                                if(value >= 1 && value <=9)
                                    sudoku.setCell(row, column, value);
                            }
                            row++;
                            if (row >= 9)
                                break;
                        }
                    }
                    Console.WriteLine("Loading complete, here is the resulting sudoku:");
                    sudoku.print();
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
        }
    }
}
