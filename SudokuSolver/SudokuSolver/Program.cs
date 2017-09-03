using SudokuSolver.Logic;
using System;
using System.IO;
using System.Linq;

namespace SudokuSolver
{
    internal class Program
    {
        private static readonly SudokuBoard sudokuBoard = new SudokuBoard();

        private static void Main()
        {
            CommandLoop();
        }

        private static void CommandLoop()
        {
            DisplayHelp();
            while(true)
            {
                Console.WriteLine("");
                Console.Write("Please write your command: ");

                string command = Console.ReadLine();
                Console.WriteLine("");

                switch(command[0])
                {
                    case 'h':
                    {
                        DisplayHelp();
                        break;
                    }

                    case 'p':
                    {
                        sudokuBoard.Print();
                        break;
                    }

                    case 'l':
                    {
                        Console.WriteLine("Load command, reading...");
                        string[] lwords = command.Split(null);
                        string filepath = lwords[1];
                        Console.WriteLine("The file path is: " + filepath);
                        LoadFromFile(filepath);
                        break;
                    }

                    case 'i':
                    {
                        string[] iwords = command.Split(null);
                        int row = int.Parse(iwords[1]);
                        int column = int.Parse(iwords[2]);
                        int value = int.Parse(iwords[3]);

                        sudokuBoard.SetCell(
                            cellRow: row,
                            cellColumn: column,
                            valueToSet: value,
                            isThereDeterminableCell: out bool unused);
                        Console.WriteLine("Number " + value + " set in cell [" + row + ',' + column + "].");
                        break;
                    }

                    case 'c':
                    {
                        Console.WriteLine("Clear cell command, clearing and recomputing...");
                        string[] words = command.Split(null);
                        int row = int.Parse(words[1]);
                        int column = int.Parse(words[2]);
                        sudokuBoard.ClearCell(row, column);
                        Console.WriteLine("Clearing complete.");
                        break;
                    }

                    case 'u':
                    {
                        sudokuBoard.SetDeterminableCells(false, out int updated);
                        Console.WriteLine("Update command executed. " + updated + " cells were set.");
                        break;
                    }

                    case 'r':
                    {
                        Console.WriteLine("Recompute command, recomputing possible values...");
                        sudokuBoard.RecomputePossibleValues();
                        Console.WriteLine("Recomputation completed");
                        break;
                    }

                    case 's':
                    {
                        Console.WriteLine("Solve command, processing...");
                        sudokuBoard.SetDeterminableCells(
                            repeatUntilPossible: true,
                            nCellsSet: out int updated);
                        Console.WriteLine(
                            "Solving completed, " + updated + " cells were set. \n" +
                            "The resulting sudoku is: ");
                        sudokuBoard.Print();
                        break;
                    }

                    case 'q':
                    {
                        Console.WriteLine("Closing the program.");
                        return;
                    }

                    default:
                    {
                        Console.WriteLine("Command not recognized.");
                        break;
                    }
                }
            }
        }

        private static void DisplayHelp()
        {
            Console.Write(
                "Welcome to SudokuSolver, small project by Raffaele Zippo.\n" +
                "Here are the available commands:\n" +
                "\t h \t\t\t\t Displays this message.\n" +
                "\t p \t\t\t\t Prints the current State of the sudokuBoard.\n" +

                "\t l {filepath} \t\t\t [No spaces in the path!] Loads inputs from the specified file.\n" +
                "\t i {row} {column} {Value} \t Sets a number in the sudokuBoard.\n" +
                "\t c {row} {column} \t\t Clears the cell and recomputes possible values for all cells.\n" +

                "\t u \t\t\t\t Checks each cell, left to right and top to bottom, and sets it if there is only one possible value it can take\n" +
                "\t r \t\t\t\t Recomputes possible values for all cells.\n" +
                "\t s \t\t\t\t Tries to solve the sudoku. Equivalent to issuing u commands until necessary.\n" +

                "\t q \t\t\t\t Quits the program.\n\n" +
                "This program may crash in case of incorrect input :P");
            Console.WriteLine();
        }

        private static void LoadFromFile(string filepath)
        {
            try
            {
                using (StreamReader sr = new StreamReader(filepath))
                {
                    int row = 0;
                    while (sr.Peek() != -1)
                    {
                        string line = sr.ReadLine();
                        if ( line.ToCharArray().Any(c => !char.IsWhiteSpace(c)) )
                        {
                            var lineFields = line.Split(null).Where(n => n.Length > 0);
                            for (
                                int column = 0;
                                column < Math.Min(9, lineFields.Count());
                                column++)
                            {
                                int.TryParse(
                                    s: lineFields.ElementAt(column),
                                    result: out int value);
                                if (value >= 1 && value <=9)
                                    sudokuBoard.SetCell(
                                        cellRow: row,
                                        cellColumn: column,
                                        valueToSet: value,
                                        isThereDeterminableCell: out bool unused);
                            }
                            row++;
                            if (row >= 9)
                                break;
                        }
                    }
                    Console.WriteLine("Loading complete, here is the resulting sudokuBoard:");
                    sudokuBoard.Print();
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
