# Introduction

The idea of this project is to build a command line application to solve sudoku, as a personal challenge to test the algorithms I thought about and the C# language. 
The program thus evolved as I learned new quirks of the problem and language used in functionality, usability and code readability/efficiency.

The program is structured with a command based interface, consisting in commands and parameters.
This includes commands for both automatic and manual usage, that is
	- automatic: load a scheme from file, process it until solved or unable to continue
	- manual: set single numbers, clear them, solve it step by step
Check the [user guide](User_guide.md) for more details about the interface.

# About the algorithm(s)

At first, the program was built on finding *naked candidates*, which are cells where only one number can be legally written to.

As I found it to be not enough, I then introduced *hidden candidates*, which are cells with more then one candidate but one of them is unique in one of the groups the cell belongs, e.g. a cell being the only one in its row who can hold the number 1.

Turns out, there is [much more than that](https://www.sudokuoftheday.com/techniques/) required to be able to solve the hardest puzzles.

So I introduced an implementation of the [backtracking algorithm](https://en.wikipedia.org/wiki/Backtracking), which builds upon the solving techniques already implemented.
Each time a scheme is found to be unsolvable with the available deduction techniques, yet still legal, one of the undetermined cells with the lowest number of candidates is selected for the speculation.
All possible solutions for that cell are then tested independently, applying the same algorithm (deduction + speculation) recursively. 

When exploring the tree of possible solutions, randomization is used to avoid worst case scenarios where the correct speculation path is the last one to be evaluated. Being each speculation an independent scheme to solve, I introduced a version of the algorithm which explores the tree concurrently. However, being the computations to be made quite simple for modern hardware, the parallelization overhead makes the algorithm less efficient.
