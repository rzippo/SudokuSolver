using Newtonsoft.Json;
using System;
using System.IO;
using Xunit;

namespace SudokuSolverLibrary.Tests
{
    public class SudokuBoard_Solve
    {
        [Fact]
        public void LoadFromPuzzle_Solve()
        {
            foreach (SudokuPuzzle puzzle in StaticPuzzles.Puzzles)
            {
                SudokuBoard board = new SudokuBoard(puzzle);
                board.Solve();
                Assert.True(board.IsSolved);
            }
        }

        [Fact]
        public void LoadFromJson_Solve_Save()
        {
            using (StreamReader sr = new StreamReader("Hardest.json"))
            {
                SudokuPuzzle puzzle = JsonConvert.DeserializeObject<SudokuPuzzle>(sr.ReadToEnd());
                SudokuBoard board = new SudokuBoard(puzzle);
                board.Solve();
                Assert.True(board.IsSolved);

                using (StreamWriter sw = new StreamWriter("Hardest.solution.json"))
                {
                    sw.Write(JsonConvert.SerializeObject(board));
                }
            }
        }

        [Fact]
        public void SavePuzzleJson()
        {
            foreach (SudokuPuzzle puzzle in StaticPuzzles.Puzzles)
            {
                using (StreamWriter sw = new StreamWriter($"{puzzle.Name}.json"))
                {
                    sw.Write(JsonConvert.SerializeObject(puzzle));
                }
            }
        }        
    }
}
