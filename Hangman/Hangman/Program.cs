using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Hangman
{
    class Program
    {
        //stores username
        static string UserName = null;

        //current guess
        static string currentGuess = null;

        //string to store the currently revealed chars
        static string revealedChars = null;

        //stores random word 
        static string CurrentRandomWord = null;

        //stores number of guesses left
        static int Lives = 0;

        //invalid attempts counter
        static int InvalidAttempts = 0;



        static void Main(string[] args)
        {
            bool looping = true;
            //allows for generating randome int
            string keepPlaying = null;
            //list of possible words
            string wrongGuess = null;
            List<string> correctGuesses = new List<string>();



            HangmanText();
            UserNameSet();
            ListOfWords();
            //set initial value of revealed characters

            //set inital number of guesses
            Lives = 8;

            //loop while playing
            Console.Clear();
            HangmanText();
            Console.WriteLine("             Starring: {0}\n", UserName);

            if (InvalidAttempts >= 7)
            {
                looping = false;
            }
            while (looping == true)
            {
                correctGuesses.Clear();
                for (int i = 0; i < CurrentRandomWord.Length; i++)
                {
                    correctGuesses.Add("_ ");
                }
                foreach (object blanks in correctGuesses)
                {

                    Console.Write(blanks);
                }
                while (Lives > 0)
                {
                    //TODO Display #of guesses left and blank spaces


                    Console.WriteLine("\nLives left: {0}", Lives);

                    UserInput();
                    //does the current word contain the current user guess?
                    if (WordContainsInput(currentGuess))
                    {

                        //reset reveal chars to create new string
                        //cycle through the current word
                        for (int i = 0; i < CurrentRandomWord.Length; i++)
                        {
                            //string to hold current letter for ease of typing
                            string a = CurrentRandomWord[i].ToString();
                            //is a the same as the current guess
                            if (a == currentGuess)
                            {
                                //add a to list at correct index and a space for clarity
                                correctGuesses[i] = a.ToUpper();
                            }
                            //if the user guesses the full word
                            else if (currentGuess.Contains(CurrentRandomWord))
                            {
                                //accept full word
                                if (currentGuess == CurrentRandomWord)
                                {
                                    correctGuesses.Clear();
                                    correctGuesses.Add(currentGuess);
                                }
                            }
                        }


                        Console.Clear();
                        HangmanText();
                        Console.WriteLine("             Starring: {0}\n", UserName);
                        Console.WriteLine("Already Guessed: {0}", wrongGuess);

                        foreach (object letters in correctGuesses)
                        {
                            revealedChars = null;
                            revealedChars = revealedChars + letters;
                            Console.Write(letters);
                        }
                    }

                    //if the guess is not in the word
                    else
                    {

                        //reduce number of guesses left
                        Lives--;
                        //store current guess if it is wrong
                        wrongGuess = wrongGuess + " " + currentGuess.ToUpper();


                        Console.Clear();
                        HangmanText();
                        Console.WriteLine("             Starring: {0}\n", UserName);
                        Console.WriteLine("Already Guessed: {0}", wrongGuess);

                        foreach (object letters in correctGuesses)
                        {
                            revealedChars = null;
                            revealedChars = revealedChars + letters;
                            Console.Write(letters);
                        }
                    }

                    if (IsWordCorrect(revealedChars))
                    {
                        //win the game
                        //would you like to keep playing?
                        //if no change playing to false
                        Console.WriteLine("You win!");
                        Console.WriteLine("Play again? Y/N");
                        keepPlaying = Console.ReadLine();

                        switch (keepPlaying.ToUpper())
                        {
                            case "Y":
                                //reset counters

                                break;
                            case "N":
                                looping = false;
                                Lives = 0;
                                Console.WriteLine("Thanks for playing!");
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            //end
            Console.ReadKey();
        }

        public static void UserNameSet()
        {

            //ask user for name
            Console.WriteLine("             Starring: ");
            UserName = Console.ReadLine();
            while (ValidInput(UserName) == false)
            {
                InvalidAttempts++;
                Console.Clear();
                HangmanText();
                switch (InvalidAttempts)
                {
                    case 1:
                        Console.WriteLine("What a funny name! Try again!");
                        Console.WriteLine("             Starring: ");
                        break;
                    case 2:
                        Console.WriteLine("Are you an alien? That doesn't seem like a real name.");
                        Console.WriteLine("             Starring: ");

                        break;
                    case 3:
                        Console.WriteLine("You're not taking this seriously are you?");
                        Console.WriteLine("             Starring: ");
                        Console.WriteLine();
                        break;
                    case 4:
                        Console.WriteLine("You're not taking this seriously are you?");
                        Console.WriteLine("             Starring: ");
                        Console.WriteLine();
                        break;
                    case 5:
                        Console.WriteLine("I don't have to let you play you know.");
                        Console.WriteLine("             Starring: ");
                        Console.WriteLine();
                        break;
                    case 6:

                        Console.WriteLine("I could just sentence you to death now!");
                        Console.WriteLine("             Starring: ");
                        Console.WriteLine();
                        break;
                    case 7:
                        Console.WriteLine("You lose!");
                        break;
                }


                UserName = Console.ReadLine();
            }
        }
        public static void UserInput()
        {
            //new string for current user input
            string currentInput = null;
            //asks for input
            Console.WriteLine("Please enter a letter: ");
            currentInput = Console.ReadLine();

            //if input is NOT valid
            if (currentInput != null && currentInput != string.Empty)
            {
                while (ValidInput(currentInput) == false)
                {


                    Console.Clear();
                    HangmanText();
                    //ask for new input
                    Console.WriteLine("That is not a valid input.");
                    Console.WriteLine("Please enter a new letter: ");
                    currentInput = Console.ReadLine();
                }

                currentGuess = currentInput;
            }

        }
        public static void ListOfWords()
        {
            List<string> words = new List<string>()
            {
                //store words for game here
                //UPPER CASE
                "AMBIDEXTROUS",
                "SENTENCES",
                "COOL",
            };
            string randomWord = null;
            Random rand = new Random();
            //select random word
            randomWord = words[rand.Next(0, words.Count)];
            CurrentRandomWord = randomWord;
        }


        public static bool ValidInput(string userInput)
        {

            if (Regex.IsMatch(userInput, @"^[a-zA-Z]+$"))
            {
                return true;
            }
            //see if user input is valid or not
            return false;
        }

        public static bool IsWordCorrect(string completeWord)
        {
            if (completeWord == CurrentRandomWord)
            {
                return true;
            }
            //see if the entire word is revealed
            return false;
        }

        public static bool WordContainsInput(string userInput)
        {
            if (CurrentRandomWord.Contains(userInput.ToUpper()))
            {
                return true;
            }
            //does the current word contain the user input anywhere?
            return false;
        }

        public static void HangmanText()
        {
            Console.WriteLine(@"
______  __                     _____               ______  ___              
___  / / /_____ ______________ ___(_)_____________ ___   |/  /_____ _______ 
__  /_/ /_  __ `/_  __ \_  __ `/_  /__  __ \_  __ `/_  /|_/ /_  __ `/_  __ \
_  __  / / /_/ /_  / / /  /_/ /_  / _  / / /  /_/ /_  /  / / / /_/ /_  / / /
/_/ /_/  \__,_/ /_/ /_/_\__, / /_/  /_/ /_/_\__, / /_/  /_/  \__,_/ /_/ /_/ 
                       /____/              /____/                              
             An 80's Action Movie
");
        }
    }

}
