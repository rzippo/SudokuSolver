using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SudokuSolverLibrary
{
    public class SudokuPuzzle
    {
        [JsonProperty("name")]
        public string Name { get; set; } = "";

        [JsonProperty("cells")]
        public IList<SudokuCell> Cells { get; set; }
    }
}
