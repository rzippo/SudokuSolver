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

                    case 'i':
                        string[] words = command.Split(' ');
                        int row = int.Parse(words[1]);
                        int column = int.Parse(words[2]);
                        int value = int.Parse(words[3]);
                        sudoku.setCell(row, column, value);
                        break;

                    case 'u':
                        sudoku.checkForSet();
                        break;

                    case 's':
                        Console.WriteLine("NOT IMPLEMENTED");
                        break;

                    case 'q':
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
                "\t h \t Displays this message.\n" +
                "\t p \t Prints the current state of the sudoku.\n" +
                
                "\t l {filepath} [NOT IMPLEMENTED] Loads inputs from the specified file.\n" +
                "\t i {row} {column} {value}\t Sets a number in the sudoku.\n" +
                "\t c {row} {column}\t [NOT IMPLEMENTED] Clears the cell and recomputes possible values for all cells" +
                
                "\t u \t Checks each cell, left to right and top to bottom, and sets it if there is only one possible value it can take\n" +
                "\t r \t [NOT IMPLEMENTED] Recomputes possible values for all cells.\t" +
                "\t s \t [NOT IMPLEMENTED] Tries to solve the sudoku, basically issuing u commands until necessary.\n" +


                "\t q \t Quits the program." +
                "This program may crash in case of incorrect input :P");
            Console.WriteLine();
        }
    }
}
