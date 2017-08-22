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
                        int row = int.Parse(iwords[1]);
                        int column = int.Parse(iwords[2]);
                        int value = int.Parse(iwords[3]);
                        sudoku.setCell(row, column, value);
                        Console.WriteLine("Number " + value + " set in cell [" + row + ',' + column + "].");
                        break;

                    case 'c':
                        Console.WriteLine("NOT IMPLEMENTED");
                        break;

                    case 'u':
                        sudoku.checkForSet();
                        Console.WriteLine("Update command executed. [Should specify if 1+ cells were set or not]");
                        break;

                    case 'r':
                        Console.WriteLine("NOT IMPLEMENTED");
                        break;

                    case 's':
                        Console.WriteLine("NOT IMPLEMENTED");
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
                "\t h \t Displays this message.\n" +
                "\t p \t Prints the current state of the sudoku.\n" +

                "\t l {filepath} \t [No spaces in the path!] Loads inputs from the specified file. One line for each number, in the format {row} {column} {value}\n" +
                "\t i {row} {column} {value}\t Sets a number in the sudoku.\n" +
                "\t c {row} {column}\t [NOT IMPLEMENTED] Clears the cell and recomputes possible values for all cells.\n" +
                
                "\t u \t Checks each cell, left to right and top to bottom, and sets it if there is only one possible value it can take\n" +
                "\t r \t [NOT IMPLEMENTED] Recomputes possible values for all cells.\n" +
                "\t s \t [NOT IMPLEMENTED] Tries to solve the sudoku, basically issuing u commands until necessary.\n" +
                
                "\t q \t Quits the program.\n" +
                "This program may crash in case of incorrect input :P");
            Console.WriteLine();
        }

        private static void loadFromFile(string filepath)
        {
            try
            {
                using (StreamReader sr = new StreamReader(filepath))
                {
                    while (sr.Peek() != -1)
                    {
                        String[] lineFields = sr.ReadLine().Split(null);
                        int row = int.Parse(lineFields[0]);
                        int column = int.Parse(lineFields[1]);
                        int value = int.Parse(lineFields[2]);
                        sudoku.setCell(row, column, value);
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
