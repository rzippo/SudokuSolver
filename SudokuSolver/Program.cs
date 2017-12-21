using SudokuSolver.Logic;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.Extensions.CommandLineUtils;

namespace SudokuSolver
{
    internal partial class Program
    {
        private static readonly SudokuBoard sudokuBoard = new SudokuBoard();

        private static void Main(params string[] args)
        {
            CommandLineApplication commandLineApplication =
                new CommandLineApplication(throwOnUnexpectedArg: false);
            
            CommandOption interactiveMode = commandLineApplication.Option(
                "-i | --interactive",
                "Starts the app in interactive mode",
                CommandOptionType.NoValue);

            CommandOption filename = commandLineApplication.Option(
                "-f | --filename",
                "Path to the file containing the board to solve",   //todo: add file syntax
                CommandOptionType.SingleValue);

            CommandOption parallelSolve = commandLineApplication.Option(
                "-ps | --paralleSolve",
                "Enable use of tasks while branching with speculation",
                CommandOptionType.NoValue);

            CommandOption branchingDepth = commandLineApplication.Option(
                "-d <depth> | --branchingDepth <depth>",
                "If parallel solving is enabled, specifies how deep the branching can be parallelized",
                CommandOptionType.SingleValue);

            CommandOption verbosity = commandLineApplication.Option(
                "-v | --verbose",
                "If enabled, prints the board both before and after the solving",
                CommandOptionType.NoValue);

            commandLineApplication.HelpOption("-? | -h | --help");
            commandLineApplication.OnExecute(() =>
            {
                if (interactiveMode.HasValue())
                    InteractiveLoop();
                else
                {
                    if(!filename.HasValue())
                        commandLineApplication.ShowHelp();
                    else
                    {
                        LoadFromFile(filename.Value());

                        if (verbosity.HasValue())
                        {
                            Console.WriteLine("Loading complete, here is the resulting sudoku:");
                            sudokuBoard.PrintBoard();
                            Console.WriteLine("Solve command, processing...");
                        }
                        
                        var watch = Stopwatch.StartNew();
                        
                        if (parallelSolve.HasValue())
                        {
                            //todo: use branching depth
                            sudokuBoard.ParallelSolve().Wait();
                        }
                        else
                        {
                            sudokuBoard.Solve();
                        }

                        var elapsedMs = watch.ElapsedMilliseconds;
                        Console.WriteLine(sudokuBoard.IsSolved() ? "Solving successful!" : "Solving failed!");
                        Console.WriteLine($"Execution took {elapsedMs} milliseconds.");

                        if (verbosity.HasValue())
                        {
                            Console.WriteLine("The resulting sudoku is: ");
                            sudokuBoard.PrintBoard();
                        }
                    }
                }
                return 0;
            });
            commandLineApplication.Execute(args);
        }

        private static bool LoadFromFile(string filepath)
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
                        if (line.ToCharArray().Any(c => !char.IsWhiteSpace(c)))
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
                                if (value >= 1 && value <= 9)
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
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
                return false;
            }
        }
    }
}
