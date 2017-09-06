# Overview

The program interface relies on command line interaction, with commands and parameters.
This is mainly to allow easy implementation of the interface and expose the features directly, as the core value is the functionality of the program.

The commands allow for both direct manipulation and automatic loading/solving of the scheme, this allows for easy usage, testing and debugging.
Out of these groups are some standard utility commands:
	**print** Prints the current board, whether is solved or illegal
	**reset** Clears the board
	**help** Displays a command guide
	**quit** Quits the program

Some commands also have abbreviation. These, togheter with parameter syntax, are detailed in the **help** command.

# Auto mode commands

Using the **load** command the sudoku to solve can be loaded from a file which retains its visual structure. An example of input text file is the following:

	  - - 7  8 - 2  - - -
	  - - -  - 3 1  7 6 9
	  3 - -  5 - -  - 8 -
	 
	  7 - 2  - 1 -  - - -
	  - 8 -  2 7 -  - - -
	  9 4 -  - - -  2 - 1

	  - 7 -  - - 6  8 - 4
 	  - 9 5  - - -  - - -
 	  - 2 -  - - 8  1 - -

The sudoku can the be solved, at the best of the program's capabilities, using the **solve** command. This command uses first deduction, with hidden and naked candidates search, the speculation when those fail. Speculations are explored synchronously. **parallelsolve**, instead, uses a parallel exploration of the possible speculations. Due to the parallelization overhead, it is less efficient.

# Manual mode commands

Many commands are available to allow step-by-step manipulation and solving. 
*Beware: solving isn't really (yet?) step-by-step, as more than one number is set at each pass.*

* **set** Sets a single number in the scheme
* **clear** Removes a single number from the scheme
* **detail** Details a cell, in particular its candidate values
* **nakedStep** Performs a passing of the naked candidate search: checks each cell and sets it if there is only one candidate 
value for it
* **hiddenSteps** Performs a passing of the hidden candidate search: checks each row, column and tile and sets those cells which are the only ones who can take a specific value, i.e. a cell being the only one in its row who can take the number 1
