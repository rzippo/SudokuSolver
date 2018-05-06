
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SudokuSolverLibrary
{
    public partial class SudokuBoard
    {
        public bool HasNakedCandidate => MatrixIterator.Any(cell => cell.HasNakedCandidate());
               
        [JsonProperty("solved")]
        public bool IsSolved => combinedGroups.AsParallel()
                .All( cellGroup => sudokuValues
                    .All( value => cellGroup
                        .Any( cell => cell.Value == value)));

        [JsonProperty("legal")]
        public bool IsLegal =>
                MatrixIterator
                .Where(cell => !cell.IsDetermined)
                .All(cell => cell.Candidates.Count > 0)
                &&
                combinedGroups
                .All(cellGroup => sudokuValues
                                    .All(value =>
                                           cellGroup.Count(cell => cell.IsDetermined && cell.Value == value) <= 1));

        public void Solve()
        {
            BasicSolve();
            if (IsLegal && !IsSolved)
            {
                var undeterminedCells =
                    MatrixIterator
                        .Where(cell => !cell.IsDetermined);

                SudokuCell speculationTarget = undeterminedCells
                        .First(cell => cell.Candidates.Count == 
                            undeterminedCells.Min(c => c.Candidates.Count));

                Random rng = new Random();
                var shuffledCandidates = speculationTarget.Candidates
                    .OrderBy(c => rng.Next());
                foreach (int candidate in shuffledCandidates)
                {
                    SudokuBoard speculativeBoard = new SudokuBoard(this);
                    speculativeBoard.SetCell(
                        cellRow: speculationTarget.Row,
                        cellColumn: speculationTarget.Column,
                        valueToSet: candidate);

                    speculativeBoard.Solve();
                    if (speculativeBoard.IsLegal && speculativeBoard.IsSolved)
                    {
                        this.Copy(speculativeBoard);
                        return;
                    }
                }
            }
        }

        public async Task ParallelSolve(int branchingDepth = -1)
        {
            if (branchingDepth == 0)
            {
                Solve();
                return;
            }

            BasicSolve();
            if(IsLegal && !IsSolved)
            {
                var undeterminedCells =
                    MatrixIterator
                    .Where(cell => !cell.IsDetermined);

                SudokuCell speculationTarget = undeterminedCells
                    .First(cell => cell.Candidates.Count == 
                        undeterminedCells.Min(c => c.Candidates.Count));

                var tokenSource = new CancellationTokenSource();
                List<Task<SudokuBoard>> speculationTasks = new List<Task<SudokuBoard>>();
                foreach (int candidate in speculationTarget.Candidates)
                {
                    SudokuBoard speculativeBoard = new SudokuBoard(this);
                    speculativeBoard.SetCell(
                        cellRow: speculationTarget.Row,
                        cellColumn: speculationTarget.Column,
                        valueToSet: candidate);


                    var token = tokenSource.Token;
                    speculationTasks.Add(
                        Task.Factory.StartNew(() =>
                            {
                                speculativeBoard
                                    .ParallelSolve(branchingDepth > 0 ? branchingDepth - 1 : branchingDepth)
                                    .Wait(token);
                                return speculativeBoard;
                            },
                            token)
                    );
                }

                while (speculationTasks.Count > 0)
                {
                    Task<SudokuBoard> completedTask = await Task.WhenAny(speculationTasks);
                    SudokuBoard speculativeBoard = completedTask.Result;
                    if (speculativeBoard.IsLegal && speculativeBoard.IsSolved)
                    {
                        tokenSource.Cancel();
                        this.Copy(speculativeBoard);
                        return;
                    }
                    speculationTasks.Remove(completedTask);
                }
            }
        }

        private void BasicSolve()
        {
            int lastCycleSetEvents;
            do
            {
                while (HasNakedCandidate)
                    SetNakedCandidateCells();
                lastCycleSetEvents = SetHiddenCandidateCells();
            } while ( IsLegal && !IsSolved && lastCycleSetEvents > 0);
        }

        public int SetNakedCandidateCells()
        {
            int nCellsSet = 0;

            MatrixIterator
                .Where(cell => cell.HasNakedCandidate())
                .ToList()
                .ForEach(
                    cell =>
                    {
                        if (cell.Candidates.Count == 1)
                        {
                            SetCell(
                                cellRow: cell.Row,
                                cellColumn: cell.Column,
                                valueToSet: cell.Candidates.First());
                            nCellsSet++;
                        }
                    });
            
            return nCellsSet;
        }

        public int SetHiddenCandidateCells()
        {
            return combinedGroups.Sum(SetGroupHiddenCandidates);

            int SetGroupHiddenCandidates(List<SudokuCell> sudokuGroup)
            {
                int nCellsSet = 0;
                IEnumerable<int> hiddenCandidates = sudokuValues.Where(
                    v => sudokuGroup.Count(cell => cell.Candidates.Contains(v)) == 1
                );
                foreach (int hiddenCandidate in hiddenCandidates)
                {
                    SudokuCell cell = sudokuGroup.First(c => c.Candidates.Contains(hiddenCandidate));
                    if (!cell.IsDetermined)
                    {
                        cell.Value = hiddenCandidate;
                        UpdateBoardCandidates(
                            sourceRow: cell.Row,
                            sourceColumn: cell.Column,
                            valueToRemove: (int)cell.Value);
                        nCellsSet++;
                    }
                }
                return nCellsSet;
            }
        }

        public void RecomputeCandidates()
        {
            MatrixIterator
                .Where(cell => !cell.IsDetermined)
                .ToList()
                .ForEach(cell => cell.Clear());
            
            MatrixIterator
                .Where(cell => cell.IsDetermined)
                .ToList()
                .ForEach(cell => UpdateBoardCandidates(
                    sourceRow: cell.Row,
                    sourceColumn: cell.Column,
                    valueToRemove: (int) cell.Value));
        }

        private void UpdateBoardCandidates(
            int sourceRow,
            int sourceColumn,
            int valueToRemove)
        {
            rows[sourceRow]
                .ForEach(cell => cell.RemoveCandidate(valueToRemove));

            columns[sourceColumn]
                .ForEach(cell => cell.RemoveCandidate(valueToRemove));

            tiles[sourceRow / 3, sourceColumn / 3]
                .ForEach(cell => cell.RemoveCandidate(valueToRemove));
        }
    }
}
