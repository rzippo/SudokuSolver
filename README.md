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

The current roadmap is to introduce a *speculative technique*, where, given a scheme that cannot be solved with the implemented techinques, cells are assigned candidates to try to unlock the scheme.
This of course will require more resources than the current setup, but it should guarantee to eventually solve the scheme, and it can be later optimized by introducing more non-speculative techniques.
