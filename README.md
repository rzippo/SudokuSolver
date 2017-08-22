# SudokuSolver

The idea of this project is to build a command line application to solve sudoku, as a personal challenge to test the algorithms I thought about and the C# language.

The program is structured with a command based interface, consisting in one character followed by parameters (when necessary). This includes commands to set numbers, clear them and, of course, try to solve the sudoku with the information available.

The sudoku to solve can be specified one number at a time, using the i command, or loaded in a single step from a file using l. Obviously, the latter is recommended. An example of input text file is the following:

	  - - 7  8 - 2  - - -
	  - - -  - 3 1  7 6 9
	  3 - -  5 - -  - 8 -
	 
	  7 - 2  - 1 -  - - -
	  - 8 -  2 7 -  - - -
	  9 4 -  - - -  2 - 1

	  - 7 -  - - 6  8 - 4
 	  - 9 5  - - -  - - -
 	  - 2 -  - - 8  1 - -

The parsing algorithm ignores whitespaces; any non-whitespace character or non-separated sequence can be used for a blank cell.

The s command attempts to solve the sudoku fully, stopping only when it's unable to fill more cells. For a gradual solution, the u command is present.

i and c can be used to manually control the scheme. This can be troublesome for the underlying algorithm, it is advisable to use the r command to recompute the state of the scheme before any solving step.

The algorithm does not distinguish illegal states, and will try to complete them regardless.