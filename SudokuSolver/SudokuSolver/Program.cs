﻿using SudokuSolver.Logic;
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

                string[] commandWords = command.Split(null);
                switch(commandWords[0])
                {
                    case "help":
                    case "h":
                    {
                        DisplayHelp();
                        break;
                    }

                    case "print":
                    case "p":
                    {
                        sudokuBoard.PrintBoard();
                        break;
                    }

                    case "load":
                    case "l":
                    {
                        Console.WriteLine("Load command, reading...");
                        string filepath = commandWords[1];
                        Console.WriteLine($"The file path is: {filepath}");
                        LoadFromFile(filepath);
                        break;
                    }

                    case "set":
                    {
                        int row = int.Parse(commandWords[1]);
                        int column = int.Parse(commandWords[2]);
                        int value = int.Parse(commandWords[3]);

                        sudokuBoard.SetCell(
                            cellRow: row,
                            cellColumn: column,
                            valueToSet: value);
                        Console.WriteLine($"Number {value} set in cell [{row},{column}].");
                        break;
                    }

                    case "clear":
                    {
                        Console.WriteLine("Clear cell command, clearing and recomputing...");
                        int row = int.Parse(commandWords[1]) - 1;
                        int column = int.Parse(commandWords[2]) - 1;
                        sudokuBoard.ClearCell(row, column);
                        Console.WriteLine("Clearing complete.");
                        break;
                    }

                    case "reset":
                    case "r":
                    {
                        Console.WriteLine("Board clear command...");
                        sudokuBoard.ClearBoard();
                        Console.WriteLine("Clearing complete");
                        sudokuBoard.PrintBoard();
                        break;
                    }

                    case "nakedStep":
                    {
                        int updated = sudokuBoard.SetNakedCandidateCells();
                        Console.WriteLine($"Naked step command executed. {updated} cells were set.");
                        break;
                    }

                    case "hiddenStep":
                    {
                        int updated = sudokuBoard.SetHiddenCandidateCells();
                        Console.WriteLine($"Hidden step command executed. {updated} cells were set.");
                        break;
                    }

                    case "detail":
                    {
                        Console.WriteLine("Cell details command...");
                        int row = int.Parse(commandWords[1]) - 1;
                        int column = int.Parse(commandWords[2]) - 1;
                        sudokuBoard.PrintCell(
                            cellRow: row,
                            cellColumn: column);
                        break;
                    }

                    case "recompute":
                    {
                        Console.WriteLine("Recompute command, recomputing possible values...");
                        sudokuBoard.RecomputeCandidates();
                        Console.WriteLine("Recomputation completed");
                        break;
                    }

                    case "solve":
                    case "s":
                    {
                        Console.WriteLine("Solve command, processing...");
                        sudokuBoard.Solve();
                        Console.WriteLine(sudokuBoard.IsSolved() ? "Solving successful!" : "Solving failed!");

                        Console.WriteLine("The resulting sudoku is: ");
                        sudokuBoard.PrintBoard();
                        break;
                    }

                    case "quit":
                    case "q":
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
                "Here are the available commands. Abbreviation are shown in round brackets, parameters in curly brackets:\n" +
                "\t (h) help \t\t\t\t Displays this message.\n" +
                "\t (p) print \t\t\t\t Prints the current state of the sudoku.\n" +
                "\n" +
                "\t (l) load {filepath} \t\t\t Clears the board and loads inputs from the specified file. No spaces in the path!\n" +
                "\t set {row} {column} {Value} \t Sets a number in the sudoku.\n" +
                "\t clear {row} {column} \t\t Clears the cell and recomputes possible values for all cells.\n" +
                "\t (r) reset \t\t\t\t Clears the whole board.\n" + 
                "\n" +
                "\t nakedStep \t\t\t\t Checks each cell, left to right and top to bottom, and sets it if there is only one possible value it can take (naked candidate)\n" +
                "\t hiddenStep \t\t\t\t Checks each row, column and tile and sets cells which are the only one who can take a certain value in a group (hidden candidate)\t" +
                "\t detail {row} {column} \t\t Details the specified cell, including its possible values\n"+
                "\t recompute \t\t\t\t Recomputes possible values for all cells.\n" +
                "\t (s) solve \t\t\t\t Tries to solve the sudoku. Equivalent to issuing u commands until necessary.\n" +
                "\n" +
                "\t (q) quit \t\t\t\t Quits the program.\n\n" +
                "This program may crash in case of incorrect input :P");
            Console.WriteLine();
        }

        private static void LoadFromFile(string filepath)
        {
            sudokuBoard.ClearBoard();
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
                                        valueToSet: value);
                            }
                            row++;
                            if (row >= 9)
                                break;
                        }
                    }
                    Console.WriteLine("Loading complete, here is the resulting sudoku:");
                    sudokuBoard.PrintBoard();
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
