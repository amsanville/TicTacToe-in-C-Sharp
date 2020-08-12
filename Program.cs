using System;
using System.Security.Principal;
using System.Text.Json;

namespace TicTacToe
{
    class Program
    {
        // Create some constant globals
        // INITBOARD - a string of an empty tic-tac-toe board
        public static readonly string INITBOARD = "1  |2  |3  \n   |   |   \n___|___|___\n4  |5  |6  \n   |   |   \n___|___|___\n7  |8  |9  \n   |   |   \n   |   |   \n";
        // INDICES - the location of the different tic-tac-toe squares on the board
        public static readonly int[] INDICES = { 13, 17, 21, 49, 53, 57, 85, 89, 93 };
        // WINCONDITIONS - the possible 3-in-a-row combinations that mean that the game is won
        public static readonly int[][] WINCONDITIONS = {
            new[] { 0, 1, 2 },
            new[] { 3, 4, 5 },
            new[] { 6, 7, 8 },
            new[] { 0, 3, 6 },
            new[] { 1, 4, 7 },
            new[] { 2, 5, 8 },
            new[] { 0, 4, 8 },
            new[] { 2, 4, 6 }
        };

        /// <summary>
        /// <c>MainMenu</c> Prints the main menu and processes the input.
        /// </summary>
        /// <returns>An integer giving the next game state</returns>
        // Note, I would nominally create a constant enumeration for each game state. However, this is a small game and the numbers actually correspond to what the user inputs.
        static int MainMenu()
        {
            // Welcome Screen
            Console.Clear();
            Console.WriteLine("Welcome to Tic Tac Toe");
            Console.WriteLine("Choose an option:");
            Console.WriteLine("1.) 1 Player");
            Console.WriteLine("2.) 2 Player");
            Console.WriteLine("3.) Quit");

            // Repeatedly prompt the user for new input
            bool validInput = false;
            string input = "3";
            while (!validInput)
            {
                input = Console.ReadLine().Trim();
                switch (input)
                {
                    case "1":
                        // Write out slection and wait for player before clearing the board
                        Console.WriteLine("1 Player Selected. Press Enter to Continue.");
                        Console.ReadLine();
                        Console.Clear();
                        validInput = true;
                        break;
                    case "2":
                        // Write out slection and wait for player before clearing the board
                        Console.WriteLine("2 Player Selected. Press Enter to Continue.");
                        Console.ReadLine();
                        Console.Clear();
                        validInput = true;
                        break;
                    case "3":
                        // Write out slection and wait for player before clearing the board
                        Console.WriteLine("Goodbye!");
                        validInput = true;
                        break;
                    default:
                        Console.WriteLine("Please select a valid option.");
                        break;
                }
            }
            
            // Return the result
            return Convert.ToInt16(input);

        }
        
        static void Game1Player()
        {
            // Initialize RNG
            Random random = new Random();

            // Initialize game data
            int playerScore = 0;        //<-- number of games the player has won
            int computerScore = 0;      //<-- number of games the computer has won
            char playerToken = 'X';   //<-- the symbol the player is using
            char computerToken = 'O'; //<-- the symbol the computer is using
            bool quit = false;          //<-- quit 1 player mode flag
            int counter = 0;            //<-- counter used for various things

            // Input processing variables
            string input;
            bool invalidInput;

            // Initialize the board
            while (!quit)
            {
                // Flip coin for whether player is X or O
                if (random.Next(100) < 50)
                {
                    playerToken = 'X';
                    computerToken = 'O';
                }
                else
                {
                    playerToken = 'O';
                    computerToken = 'X';
                }

                // Create the output for the game
                string header = "Player: " + playerScore + "\t Computer: " + computerScore + "\nPlayer is " + playerToken;
                string currBoard = INITBOARD;

                Console.WriteLine(header);
                Console.WriteLine("X goes first. Press Enter to Continue.\n");
                Console.ReadLine();

                // Loop
                bool turn = playerToken == 'X';   // <-- Turn flag for player's turn
                bool gameOver = false;            // <-- Game Over flag
                while (!gameOver)
                {
                    Console.Clear();
                    Console.WriteLine(header);
                    Console.WriteLine(currBoard);
                    // Player's Turn
                    int index = -1;
                    if (turn)
                    {
                        Console.WriteLine("Player's turn. Select a number 1-9 corresponding to where you want to play.");
                        invalidInput = true;
                        while (invalidInput)
                        {
                            input = Console.ReadLine().Trim();
                            switch (input)
                            {
                                case "1":
                                case "2":
                                case "3":
                                case "4":
                                case "5":
                                case "6":
                                case "7":
                                case "8":
                                case "9":
                                    index = INDICES[Convert.ToInt16(input) - 1];
                                    if (currBoard[index] == ' ')
                                    {
                                        invalidInput = false;
                                    }
                                    else
                                    {
                                        Console.WriteLine("Please enter an unoccupied space.");
                                    }
                                    break;
                                default:
                                    Console.WriteLine("Please select a valid option.");
                                    break;
                            }
                        }

                        // Place the piece
                        currBoard = currBoard.Substring(0, index) + playerToken + currBoard.Substring(index + 1);
                    }
                    // Computer's Turn
                    else
                    {
                        // Is the middle unoccupied? If so play there
                        if (currBoard[INDICES[4]] == ' ')
                        {
                            index = INDICES[4];
                        }

                        // Can the computer win? If so play there
                        for(int i = 0; i < WINCONDITIONS.Length && index == -1; i++)
                        {
                            // Count the number of X's and O's (+1 for computer, -1 for player)
                            counter = 0;
                            for(int j = 0; j < WINCONDITIONS[i].Length; j++)
                            {
                                if (currBoard[INDICES[WINCONDITIONS[i][j]]] == computerToken)
                                {
                                    counter++;
                                } else if (currBoard[INDICES[WINCONDITIONS[i][j]]] == playerToken)
                                {
                                    counter--;
                                }
                            }

                            // If 2 computer spots and no player spots, play there for win:
                            if(counter == 2)
                            {
                                for (int j = 0; j < WINCONDITIONS[i].Length; j++)
                                {
                                    if(currBoard[INDICES[WINCONDITIONS[i][j]]] == ' ')
                                    {
                                        index = INDICES[WINCONDITIONS[i][j]];
                                    }
                                }
                            }
                        }

                        // Can the player win? If so play there
                        for (int i = 0; i < WINCONDITIONS.Length && index == -1; i++)
                        {
                            // Count the number of X's and O's (+1 for X, -1 for O)
                            counter = 0;
                            for (int j = 0; j < WINCONDITIONS[i].Length; j++)
                            {
                                if (currBoard[INDICES[WINCONDITIONS[i][j]]] == computerToken)
                                {
                                    counter--;
                                }
                                else if (currBoard[INDICES[WINCONDITIONS[i][j]]] == playerToken)
                                {
                                    counter++;
                                }
                            }

                            // If 2 player and no computer:
                            if (counter == 2)
                            {
                                for (int j = 0; j < WINCONDITIONS[i].Length; j++)
                                {
                                    if (currBoard[INDICES[WINCONDITIONS[i][j]]] == ' ')
                                    {
                                        index = INDICES[WINCONDITIONS[i][j]];
                                    }
                                }
                            }
                        }

                        // Finally, if none of the above is met randomly choose a spot
                        while (index == -1)
                        {
                            index = INDICES[random.Next(8)];
                            if(currBoard[index] != ' ')
                            {
                                index = -1;
                            }
                        }

                        // PLace the computer token
                        currBoard = currBoard.Substring(0, index) + computerToken + currBoard.Substring(index + 1);
                    }

                    // Check if someone has won
                    for(int i = 0; i < WINCONDITIONS.Length; i++)
                    {
                        counter = 0;
                        for(int j = 0; j < WINCONDITIONS[i].Length && !gameOver; j++)
                        {
                            if(currBoard[INDICES[WINCONDITIONS[i][j]]] == playerToken)
                            {
                                counter++;
                            }
                            if (currBoard[INDICES[WINCONDITIONS[i][j]]] == computerToken)
                            {
                                counter--;
                            }
                        }
                        if(counter == -3)
                        {
                            Console.Clear();
                            Console.WriteLine(header);
                            Console.WriteLine(currBoard);
                            Console.WriteLine("Computer wins!");
                            computerScore++;
                            gameOver = true;
                        } else if(counter == 3)
                        {
                            Console.Clear();
                            Console.WriteLine(header);
                            Console.WriteLine(currBoard);
                            Console.WriteLine("Player wins!");
                            playerScore++;
                            gameOver = true;
                        }
                    }

                    // Check if board is full
                    counter = 0;
                    for(int i = 0; i < INDICES.Length; i++)
                    {
                        if(currBoard[INDICES[i]] != ' ')
                        {
                            counter++;
                        }
                    }
                    if(counter == 9)
                    {
                        Console.Clear();
                        Console.WriteLine(header);
                        Console.WriteLine(currBoard);
                        Console.WriteLine("Draw. Game Over.");
                        gameOver = true;
                    }

                    // Flip the turn
                    turn = !turn;
                }
                Console.WriteLine("Play again? Y/N");

                // Read Input from the console to determine whether to play again
                invalidInput = true;
                while (invalidInput)
                {
                    input = Console.ReadLine().Trim().ToLower();
                    if (input == "y" || input == "yes")
                    {
                        invalidInput = false;
                    }
                    else if (input == "n" || input == "no")
                    {
                        invalidInput = false;
                        quit = true;

                    }
                    else
                    {
                        Console.WriteLine("Please select a valid option.");
                    }
                }
            }
        }

        static void Game2Player()
        {
            // Initialize RNG
            Random random = new Random();

            // Initialize game data
            int player1Score = 0;        //<-- number of games the player has won
            int player2Score = 0;      //<-- number of games the computer has won
            char player1Token = 'X';   //<-- the symbol the player is using
            char player2Token = 'O'; //<-- the symbol the computer is using
            bool quit = false;          //<-- quit 1 player mode flag
            int counter = 0;            //<-- counter used for various things

            // Input processing variables
            string input;
            bool invalidInput;

            // Initialize the board
            while (!quit)
            {
                // Flip coin for whether player is X or O
                if (random.Next(100) < 50)
                {
                    player1Token = 'X';
                    player2Token = 'O';
                }
                else
                {
                    player1Token = 'O';
                    player2Token = 'X';
                }

                // Create the output for the game
                string header = "Player 1: " + player1Score + "\t Player 2: " + player2Score + "\nPlayer 1 is " + player1Token + " Player 2 is " + player2Token;
                string currBoard = INITBOARD;

                Console.WriteLine(header);
                Console.WriteLine("X goes first. Press Enter to Continue.\n");
                Console.ReadLine();

                // Loop
                bool turn = player1Token == 'X';   // <-- Turn flag for player's turn
                bool gameOver = false;            // <-- Game Over flag
                while (!gameOver)
                {
                    Console.Clear();
                    Console.WriteLine(header);
                    Console.WriteLine(currBoard);
                    // Player's Turn
                    int index = -1;
                    if (turn)
                    {
                        Console.WriteLine("Player 1's turn. Select a number 1-9 corresponding to where you want to play.");
                    } else
                    {
                        Console.WriteLine("Player 2's turn. Select a number 1-9 corresponding to where you want to play.");
                    }

                    // Get input from the user
                    invalidInput = true;
                    while (invalidInput)
                    {
                        input = Console.ReadLine().Trim();
                        switch (input)
                        {
                            case "1":
                            case "2":
                            case "3":
                            case "4":
                            case "5":
                            case "6":
                            case "7":
                            case "8":
                            case "9":
                                index = INDICES[Convert.ToInt16(input) - 1];
                                if (currBoard[index] == ' ')
                                {
                                    invalidInput = false;
                                }
                                else
                                {
                                    Console.WriteLine("Please enter an unoccupied space.");
                                }
                                break;
                            default:
                                Console.WriteLine("Please select a valid option.");
                                break;
                        }
                    }

                    // Place the piece
                    if (turn) {
                        currBoard = currBoard.Substring(0, index) + player1Token + currBoard.Substring(index + 1);
                    } else
                    {
                        currBoard = currBoard.Substring(0, index) + player2Token + currBoard.Substring(index + 1);
                    }


                    // Check if someone has won
                    for (int i = 0; i < WINCONDITIONS.Length; i++)
                    {
                        counter = 0;
                        for (int j = 0; j < WINCONDITIONS[i].Length && !gameOver; j++)
                        {
                            if (currBoard[INDICES[WINCONDITIONS[i][j]]] == player1Token)
                            {
                                counter++;
                            }
                            if (currBoard[INDICES[WINCONDITIONS[i][j]]] == player2Token)
                            {
                                counter--;
                            }
                        }
                        if (counter == -3)
                        {
                            Console.Clear();
                            Console.WriteLine(header);
                            Console.WriteLine(currBoard);
                            Console.WriteLine("Player 2 wins!");
                            player2Score++;
                            gameOver = true;
                        }
                        else if (counter == 3)
                        {
                            Console.Clear();
                            Console.WriteLine(header);
                            Console.WriteLine(currBoard);
                            Console.WriteLine("Player 1 wins!");
                            player1Score++;
                            gameOver = true;
                        }
                    }

                    // Check if board is full
                    counter = 0;
                    for (int i = 0; i < INDICES.Length; i++)
                    {
                        if (currBoard[INDICES[i]] != ' ')
                        {
                            counter++;
                        }
                    }
                    if (counter == 9)
                    {
                        Console.Clear();
                        Console.WriteLine(header);
                        Console.WriteLine(currBoard);
                        Console.WriteLine("Draw. Game Over.");
                        gameOver = true;
                    }

                    // Flip the turn
                    turn = !turn;
                }
                Console.WriteLine("Play again? Y/N");

                // Read Input from the console to determine whether to play again
                invalidInput = true;
                while (invalidInput)
                {
                    input = Console.ReadLine().Trim().ToLower();
                    if (input == "y" || input == "yes")
                    {
                        invalidInput = false;
                    }
                    else if (input == "n" || input == "no")
                    {
                        invalidInput = false;
                        quit = true;

                    }
                    else
                    {
                        Console.WriteLine("Please select a valid option.");
                    }
                }
            }
        }

        static void Main(string[] args)
        {
            
            // Start the game loop
            bool quit = false;
            while (!quit)
            {
                // Choose what's next based on what happens on the menu
                switch (MainMenu())
                {
                    case 1:
                        // 1 Player mode
                        Game1Player();
                        break;
                    case 2:
                        // 2 Player Mode
                        Game2Player();
                        break;
                    case 3:
                        // Quit
                        quit = true;
                        break;
                    default:
                        // *Should* be impossible to reach
                        Console.WriteLine("Congratulations, there's a bug and you stumbled into it!");
                        break;

                }
            }
            
        }
    }
}
