using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Threading;

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
        //stores the letters the user guesses incorrectly
        static string wrongGuess = " ";
        //list to store the correctly guessed letters
        static List<string> correctGuesses = new List<string>();
        //if the user wants to keep playing
        static string keepPlaying = null;
        //looping for user input
        static bool looping = true;
        //host catcalls
        static int CatCallsCounter = 0;

        static int bestOfThree = 1;


        static void Main(string[] args)
        {
            //sets window size for the image to appear correctly
            SetWindowSize();
            //image
            TextSizeThing();
            //hangman text image
            HangmanText();
            //pauses for the user to see images
            Console.WriteLine("\n              Credit: 50 Cents To Play: ");
            Console.ReadKey();

            //game instructions
            Message();

            Lives = 8;
            //set the user name
            UserNameSet();
            //give the user 8 lives

            //if the user was messing around, they don't get to play the game :D
            if (InvalidAttempts >= 7)
            {
                looping = false;
            }
            //this is the display function that calls hangman text, gun picture etc
            Display();
            //loop while playing
            while (looping == true)
            {
                //get a random word each new game
                ListOfWords();
                //empty the current list, for restarting the game
                correctGuesses.Clear();
                //make and display blank number of letters
                for (int i = 0; i < CurrentRandomWord.Length; i++)
                {
                    correctGuesses.Add("_ ");
                }
                foreach (object blanks in correctGuesses)
                {

                    Console.Write(blanks);
                }
                //if there are still lives left
                while (Lives > 0)
                {

                    //user has x lives left
                    Console.WriteLine();
                    Console.WriteLine("\nLives left: {0}", Lives);

                    //user input function
                    UserInput();

                    //does the current word contain the current user guess?
                    if (WordContainsInput(currentGuess))
                    {

                        //cycle through current word
                        for (int i = 0; i < CurrentRandomWord.Length; i++)
                        {
                           //if the current letter of the current word is the exact same as user input
                            if (CurrentRandomWord[i].ToString() == currentGuess)
                            {
                                //add a to list at correct index
                                correctGuesses[i] = currentGuess.ToUpper();
                            }
                            //if the user guesses the full word
                            else if (currentGuess == CurrentRandomWord)
                            {
                                //accept full word
                                correctGuesses.Clear();
                                correctGuesses.Add(currentGuess);
                            }
                        }
                        //figure out this part
                        revealedChars = null;
                        Display();
                        //displays hangman logo and clears console for easy viewing


                        //if the user has won, reduce lives to 0
                        
                        if (IsWordCorrect(revealedChars))
                        {
                            //breaks out of input loop
                            Lives = 0;
                        }

                    }
                    //if the  users input is not contained in the random word
                    else
                    {
                        //if the user has already tried that letter
                        if (wrongGuess.Contains(currentGuess))
                        {
                            Display();
                            Console.WriteLine("\nThe host: ");
                            Console.WriteLine("You've already tried {0}", currentGuess);
                            Console.WriteLine("We'll give you a freebie... This time.");

                            wrongGuess = wrongGuess + " " + currentGuess.ToUpper();
                        }
                            //if wrongguess has values in it to access
                        else if (wrongGuess != null && wrongGuess != string.Empty)
                        {
                            //reduce number of guesses left
                            Lives--;

                            //store new wrong guess
                            wrongGuess = wrongGuess + " " + currentGuess.ToUpper();
                            
                            //refresh screen
                            Display();

                            //host announcements
                            //for each wrong answer
                            CatCallsCounter++;
                            switch (CatCallsCounter)
                            {
                                case 1:

                                    Console.WriteLine();
                                    Console.WriteLine("\nThe host: ");
                                    Console.WriteLine("Your fate draws closer.");
                                    break;
                                case 2:

                                    Console.WriteLine();
                                    Console.WriteLine("\nThe host: ");
                                    Console.WriteLine("You're one step closer to your doom!");
                                    break;
                                case 3:

                                    Console.WriteLine();
                                    Console.WriteLine("\nThe host: ");
                                    Console.WriteLine("Take comfort in the fact that you won't die alone.");
                                    break;
                                case 4:

                                    Console.WriteLine();
                                    Console.WriteLine("\nThe host: ");
                                    Console.WriteLine("The audience will enjoy the spectacle of your demise.");
                                    break;
                                case 5:

                                    Console.WriteLine();
                                    Console.WriteLine("\nThe host: ");
                                    Console.WriteLine("Your downfall is fast approaching!");
                                    break;
                                case 6:

                                    Console.WriteLine();
                                    Console.WriteLine("\nThe host: ");
                                    Console.WriteLine("Make your next choice carefully! You don't have many lives left!");
                                    break;
                                case 7:

                                    Console.WriteLine();
                                    Console.WriteLine("\nThe host: ");
                                    Console.WriteLine("One life left to live, better make it count.");
                                    break;
                                case 8:
                                    Console.WriteLine();
                                    Console.WriteLine("\nThe host: ");
                                    Console.WriteLine("Your struggle was futile after all! And now for the best part of the show--");
                                    Thread.Sleep(1000);
                                    Console.WriteLine("\n\n Audience: ARE YOU READY!?!?!?");
                                    break;

                            }
                            
                        }


                    }


                }
                //win
                if (IsWordCorrect(revealedChars))
                {
                    if (bestOfThree != 3)
                    {
                        bestOfThree++;
                        Console.WriteLine("\n\nThe host: ");
                        Console.WriteLine("You haven't won your liberty yet!");
                        Console.WriteLine("You must get 3 words correct before we let you go!\n");
                        Console.WriteLine("Only {0} more left", 4 - bestOfThree);
                        Lives = 8;
                        revealedChars = null;
                        CurrentRandomWord = null;
                        wrongGuess = " ";
                        correctGuesses.Clear();
                        Thread.Sleep(3500);
                        Display();
                    }
                    else if(bestOfThree == 3)
                    {
                        Console.WriteLine();
                        Console.WriteLine("\nThe host: ");
                        Console.WriteLine("You have earned your freedom. \nYou may go.");
                        Console.WriteLine("Unless you would like to play again. Double or nothing! Y/N");
                        keepPlaying = Console.ReadLine();
                        //does the user want to keep playing?
                        KeepPlayingPrompt();
                    }

                }
                //lose
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("\nYour life is forfeit!");
                    Console.WriteLine("Unless you would like to play again. Double or nothing! Y/N");
                    keepPlaying = Console.ReadLine();
                    //keep playing?
                    KeepPlayingPrompt();
                    //if not, show death animation


                }
            }
            //end
            Animation();
            Console.ReadKey();
        }
        /// <summary>
        /// does the user want to keep playing?
        /// </summary>
        public static void KeepPlayingPrompt()
        {
            switch (keepPlaying.ToUpper())
            {
                case "Y":
                    //reset counters
                    Lives = 8;
                    revealedChars = null;
                    CurrentRandomWord = null;
                    wrongGuess = " ";
                    correctGuesses.Clear();
                    bestOfThree = 1;
                    Display();
                    break;
                case "N":
                    looping = false;
                    Lives = 0;
                    Console.WriteLine();
                    break;
                default:
                    Display();
                    Console.WriteLine();
                    Console.WriteLine("That isn't a valid option. Press either Y or N: ");
                    keepPlaying = Console.ReadLine();
                    break;
            }
        }
        /// <summary>
        /// Set user name
        /// </summary>
        public static void UserNameSet()
        {

            //ask user for name
            Console.WriteLine("             Starring: ");
            UserName = Console.ReadLine();
            while (ValidInput(UserName) == false)
            {

                InvalidAttempts++;
                Console.Clear();
                TextSizeThing();
                HangmanText();
                //host voices for messing around with names
                switch (InvalidAttempts)
                {
                    case 1:
                        Console.WriteLine("\nThe host: ");
                        Console.WriteLine("     What a funny name! Try again!");
                        Console.WriteLine("             Starring: ");

                        Console.WriteLine();

                        UserName = Console.ReadLine();
                        break;
                    case 2:
                        Console.WriteLine("\nThe host: ");
                        Console.WriteLine("Are you an alien? That doesn't seem like a real name.");
                        Console.WriteLine("             Starring: ");

                        Console.WriteLine();

                        UserName = Console.ReadLine();

                        break;
                    case 3:
                        Console.WriteLine("\nThe host: ");
                        Console.WriteLine("You're not taking this seriously are you?");
                        Console.WriteLine("             Starring: ");
                        Console.WriteLine();

                        UserName = Console.ReadLine();
                        break;
                    case 4:
                        Console.WriteLine("\nThe host: ");
                        Console.WriteLine( "     This is a life or death game!");
                        Console.WriteLine("             Starring: ");
                        Console.WriteLine();

                        UserName = Console.ReadLine();
                        break;
                    case 5:
                        Console.WriteLine("\nThe host: ");
                        Console.WriteLine("   I don't have to let you play you know.");
                        Console.WriteLine("             Starring: ");
                        Console.WriteLine();

                        UserName = Console.ReadLine();
                        break;
                    case 6:
                        Console.WriteLine("\nThe host: ");

                        Console.WriteLine("\nI could just sentence you to death now!");
                        Console.WriteLine("             Starring: ");
                        Console.WriteLine();

                        UserName = Console.ReadLine();
                        break;
                    case 7:
                        Console.WriteLine("\nThe host: ");
                        Console.WriteLine("You lose the game!");
                        Lives = 0;
                        UserName = "Failure";
                        break;
                }


            }
        }
        /// <summary>
        /// get user input
        /// </summary>
        public static void UserInput()
        {
            //new string for current user input
            string currentInput = null;
            //asks for input
            Console.WriteLine("\n\n\nAt any time, you may guess the full word.");
            Console.WriteLine("Please enter a letter:");
            currentInput = Console.ReadLine();
            currentInput = currentInput.ToUpper();

            //if input is NOT valid
            if (currentInput != null && currentInput != string.Empty)
            {
                while (ValidInput(currentInput) == false)
                {

                    Display();
                    Console.WriteLine();
                    Console.WriteLine();
                    //ask for new input
                    Console.WriteLine("\nThe host: ");
                    Console.WriteLine("That is not a real letter!");
                    Console.WriteLine("Do you wish to die sooner rather than later? ");
                    Console.WriteLine("Please try again: ");
                    Console.WriteLine();
                    currentInput = Console.ReadLine();
                }

                currentGuess = currentInput;
            }

        }
        /// <summary>
        /// list of words to be selected by the program at random
        /// </summary>
        public static void ListOfWords()
        {
            List<string> words = new List<string>()
            {
                //store words for game here
                //UPPER CASE
                "AMBIDEXTROUS",
                "SENTENCES",
                "COOL",
                "DISCHARGE",
                "BLOCKBUSTER",
                "EMIT",
                "VENTILATE",
                "SIMILAR",
                "PADDLER",
                "TEASING",
                "SALTISH", 
                "WHISKEY", 
                "MOVIE", 
                "ACTION",
                "FUNNY",

	
            };
            string randomWord = null;
            Random rand = new Random();
            //select random word
            randomWord = words[rand.Next(0, words.Count)];
            CurrentRandomWord = randomWord;
        }

        /// <summary>
        /// is the user input valid?
        /// </summary>
        /// <param name="userInput"></param>
        /// <returns></returns>
        public static bool ValidInput(string userInput)
        {
            //check current input against alphabet only
            if (Regex.IsMatch(userInput, @"^[a-zA-Z]+$"))
            {
                return true;
            }
            //see if user input is valid or not
            return false;
        }

        /// <summary>
        /// is the current word correct 
        /// </summary>
        /// <param name="completeWord"></param>
        /// <returns></returns>
        public static bool IsWordCorrect(string completeWord)
        {
            if (completeWord == CurrentRandomWord)
            {
                return true;
            }
            //see if the entire word is revealed
            return false;
        }

        /// <summary>
        /// does the word contain the users input?
        /// </summary>
        /// <param name="userInput"></param>
        /// <returns></returns>
        public static bool WordContainsInput(string userInput)
        {
            //if the stuff is empty, the current guess is not in the word
            if (userInput == string.Empty || userInput == null)
            {
                return false;
            }
            if (CurrentRandomWord.Contains(userInput.ToUpper()))
            {
                return true;
            }
            //does the current word contain the user input anywhere?
            return false;
        }
        /// <summary>
        /// the hanging man logo
        /// </summary>
        public static void HangmanText()
        {


            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(@"
______  __                     _____               ______  ___              
___  / / /_____ ______________ ___(_)_____________ ___   |/  /_____ _______ 
__  /_/ /_  __ `/_  __ \_  __ `/_  /__  __ \_  __ `/_  /|_/ /_  __ `/_  __ \
_  __  / / /_/ /_  / / /  /_/ /_  / _  / / /  /_/ /_  /  / / / /_/ /_  / / /
/_/ /_/  \__,_/ /_/ /_/_\__, / /_/  /_/ /_/_\__, / /_/  /_/  \__,_/ /_/ /_/ 
                       /____/              /____/ ");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write(@"
             Based on an 80's Action Movie");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;
        }
        /// <summary>
        /// game rules and story
        /// </summary>
        public static void Message()
        {
            Console.WriteLine("\n             In the year 2019, \n      America's favorite television program is The Hanging Man.\n");
            Thread.Sleep(700);
            Console.WriteLine("A game show in which prisoners must play hangman to avoid a brutal death. \n");
            Thread.Sleep(800);
            Console.WriteLine("       By guessing the secret word correctly, \n      you have the opportunity to earn your freedom");
            Thread.Sleep(900);

            Console.WriteLine("\n          --but \n\n        the twisted host has no intention of letting you escape.\n");
            Thread.Sleep(3000);
        }
        /// <summary>
        /// refresh screen and display all messages, logos, images, etc
        /// </summary>
        public static void Display()
        {
            Console.Clear();
            TextSizeThing();
            HangmanText();
            Console.WriteLine("             Starring: {0}\n", UserName);

            Console.WriteLine("\nWrong Letters: {0}", wrongGuess);

            foreach (object letters in correctGuesses)
            {
                revealedChars = revealedChars + letters;
                Console.Write(letters);
            }
            
        }
        /// <summary>
        /// picture for top of game screen
        /// </summary>
        public static void TextSizeThing()
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            //http://ascii.co.uk/art/shoot
            //not my original
            Console.WriteLine(@"

          _.ggpp.                          
       .g$$$$SSSSSp.                       
      d$$$$SSSS$SSSSb                      
     :$$$SSS$$$SSP:SSb                     
     $$$SS$$$SSP'  $SS;                    
     $$S$$SSS^    :SS$                    
    :$$SSS^(   .-- :$S$                    
    $$S;   '       :$S$                    
    $$$$           $$$;                    
    :$$$;    -    :$$$                     
    :$$$$   .-.   $$$$                     
    T$$$$b. ` ' .d$$$;                     
      ^T$PT`---':^ ^T$+'                   
          ;     ;                         
   _..__.-. -. (:                          
 ,dP .:(o);      -._                      
 :$   _: 0 ;        \`.                    
 $; .dP.\-/-.        `:                    
:$ :$$  -U--:-.  \    ;                   
$$ $$$  ----;    -.L.-  \                  
'T;'$$b ---(      ;O:    ;                 
  T 'T$bp;p'      :-:    :                 
   `.   d$b       ; :    ;                 
      $$$$b._.= :  :`. /                  
       :$$$$$P   :   ; Y                   
        $$$P     ;   : :                   
        :$$     :     ; ;                  
         $$     :   ; : :                  
         :$;    ;   :  ; \                 
          $;   :    ;  :  \_               
          :;   :        \  \-.            
          $;   ;         \  `. -.         
      
");
        }
        /// <summary>
        /// sets size of window larger
        /// </summary>
        public static void SetWindowSize()
        {
            Console.WindowHeight = 69;
            Console.WindowWidth = 76;
        }

        /// <summary>
        /// death animation
        /// </summary>
        public static void Animation()
        {
            int loopCounter = 0;
            while (loopCounter < 3)
            {
                Console.Clear();

                HangmanText();
                Console.WriteLine(@"GAME OVER!!!

   |_______________``\
    [/]           [  ]
    [\]           | ||
    [/]           |  |
    [\]           |  |
    [/]           || |
   [---]          |  |
   [---]          |@ |
   [---]          |  |
  oOOOOOo         |  |
 oOO___OOo        | @|
oO/|||||\Oo       |  |
OO/|||||\OOo      |  |
*O\ x x /OO*      |  |
/*|  c  |O*\      |  |
   \_O_/    \     |  |
    \#/     |     |  |
 |       |  |     | ||
 |       |  |_____| ||__
_/_______\__|  \  ||||  \
/         \_|\  _ | ||\  \
|    V    |\  \//\  \  \  \
|    |    | __//  \  \  \  \
|    |    |\|//|\  \  \  \  \
------------\--- \  \  \  \  \
\  \  \  \  \  \  \  \  \  \  \
_\__\__\__\__\__\__\__\__\__\__\
__|__|__|__|__|__|__|__|__|__|__|
|___| |___|
|###/ \###|
\##/   \##/
 ``     `` ");

                Thread.Sleep(300);
                Console.Clear();
                HangmanText();
                Console.WriteLine(@"

   |_______________``\
    [/]           [  ]
     [\]          | ||
     [/]          |  |
     [\]          |  |
     [/]          || |
    [---]         |  |
    [---]         |@ |
    [---]         |  |
   oOOOOOo        |  |
   oOO___OOo      | @|
  oO/|||||\Oo     |  |
  OO/|||||\OOo    |  |
  *O\ x x /OO*    |  |
  /*|  c  |O*\    |  |
     \_O_/    \   |  |
      \#/     |   |  |
   |       |  |   | ||
   |       |  |___| ||_
  _/_______\__|   |||| \
  /         \_|\  | ||\\
  |    V    |\  \\ ||\  \
  |    V    | \//\  \  \  \
  |    |    |_//  \  \  \  \
  |    |    |//|\  \  \  \  \
------------\--- \  \  \  \  \
\  \  \  \  \  \  \  \  \  \  \
_\__\__\__\__\__\__\__\__\__\__\
__|__|__|__|__|__|__|__|__|__|__|
  |___| |___|
  |###/ \###|
  \##/   \##/
 ``     `` ");

                Thread.Sleep(300);
                Console.Clear();
                HangmanText();
                Console.WriteLine(@"GAME OVER!!!

   |_______________``\
    [/]           [  ]
     [\]          | ||
     [/]          |  |
     [\]          |  |
      [/]         || |
     [---]        |  |
      [---]       |@ |
      [---]       |  |
     oOOOOOo      |  |
     oOO___OOo    | @|
    oO/|||||\Oo   |  |
    OO/|||||\OOo  |  |
    *O\ x x /OO*  |  |
    /*|  c  |O*\  |  |
       \_O_/    \ |  |
        \#/     | |  |
     |       |  | | ||
     |       |  |_| ||_
    _/_______\__||||   \
    /         \_| ||\   \
    |    V    |\   ||\   \
    |    V    |//\  \  \  \
    |    |    |/  \  \  \  \
    |    |    ||\  \  \  \  \
------------\--- \  \  \  \  \
\  \  \  \  \  \  \  \  \  \  \
_\__\__\__\__\__\__\__\__\__\__\
__|__|__|__|__|__|__|__|__|__|__|
    |___| |___|
    |###/ \###|
    \##/   \##/
   ``     `` ");

                Thread.Sleep(300);
                Console.Clear();
                HangmanText();
                Console.WriteLine(@"

   |_______________``\
    [/]           [  ]
     [\]          | ||
      [/]         |  |
       [\]        |  |
       [/]        || |
      [---]       |  |
      [---]       |@ |
       [---]      |  |
      oOOOOOo     |  |
      oOO___OO    | @|
     oO/|||||\Oo  |  |
     OO/|||||\OOo |  |
     *O\ x x /OO* |  |
     /*|  c  |O*\ |  |
   /    \_O_/    \|  |
   |     \#/     ||  |
   |  |       |  || ||
   |  |       |  || ||__
   | _/_______\__| ||| \
     /         \_| | |  \
     |    V    |    ||\  \
     |    V    |/\  \  \  \
     |    |    |  \  \  \  \
     |    |    |\  \  \  \  \
------------\--- \  \  \  \  \
\  \  \  \  \  \  \  \  \  \  \
_\__\__\__\__\__\__\__\__\__\__\
__|__|__|__|__|__|__|__|__|__|__|
     |___| |___|
     |###/ \###|
     \##/   \##/
   ``     `` ");

                Thread.Sleep(300);
                Console.Clear();
                HangmanText();
                Console.WriteLine(@"GAME OVER!!!

   |_______________``\
    [/]           [  ]
     [\]          | ||
     [/]          |  |
     [\]          |  |
      [/]         || |
     [---]        |  |
      [---]       |@ |
      [---]       |  |
     oOOOOOo      |  |
     oOO___OOo    | @|
    oO/|||||\Oo   |  |
    OO/|||||\OOo  |  |
    *O\ x x /OO*  |  |
    /*|  c  |O*\  |  |
       \_O_/    \ |  |
        \#/     | |  |
     |       |  | | ||
     |       |  |_| ||__
    _/_______\__|   || \
    /         \_|\  |   \
    |    V    |     ||\  \
    |    V    |//\  \  \  \
    |    |    |/  \  \  \  \
    |    |    ||\  \  \  \  \
------------\--- \  \  \  \  \
\  \  \  \  \  \  \  \  \  \  \
_\__\__\__\__\__\__\__\__\__\__\
__|__|__|__|__|__|__|__|__|__|__|
    |___| |___|
    |###/ \###|
    \##/   \##/
   ``     `` ");

                Thread.Sleep(300);
                Console.Clear();
                HangmanText();
                Console.WriteLine(@"

   |_______________``\
    [/]           [  ]
     [\]          | ||
     [/]          |  |
     [\]          |  |
     [/]          || |
    [---]         |  |
    [---]         |@ |
    [---]         |  |
   oOOOOOo        |  |
   oOO___OOo      | @|
  oO/|||||\Oo     |  |
  OO/|||||\OOo    |  |
  *O\ x x /OO*    |  |
  /*|  c  |O*\    |  |
     \_O_/    \   |  |
      \#/     |   |  |
   |       |  |   | ||
   |       |  |___| ||__
  _/_______\__|   ||||  \
  /         \_|\  | ||\  \
  |    V    | \//\  \  \  \
  |    |    |_//  \  \  \  \
  |    |    |//|\  \  \  \  \
------------\--- \  \  \  \  \
\  \  \  \  \  \  \  \  \  \  \
_\__\__\__\__\__\__\__\__\__\__\
__|__|__|__|__|__|__|__|__|__|__|
  |___| |___|
  |###/ \###|
  \##/   \##/
 ``     `` ");
                Thread.Sleep(300);
                Console.Clear();
                HangmanText();
                Console.WriteLine(@"GAME OVER!!!

   |_______________``\
    [/]           [  ]
   [\]            | ||
   [/]            |  |
   [\]            |  |
   [/]            || |
  [---]           |  |
  [---]           |@ |
  [---]           |  |
 oOOOOOo          |  |
oOO___OOo         | @|
O/|||||\Oo        |  |
O/|||||\OOo       |  |
O\ x x /OO*       |  |
*|  c  |O*\       |  |
  \_O_/    \      |  |
   \#/     |      |  |
|       |  |      | ||
|       |  |_____ | ||__
/_______\__|  \   ||||  \
         \_|\  _  | ||\  \
    V    |   \//\   \  \  \
    |    |   //  \   \  \  \
    |    | |//|\  \   \  \  \
-----------\--- \  \   \  \  \
  \  \  \  \  \  \  \   \  \  \
\__\__\__\__\__\__\__\_ _\__\__\
_|__|__|__|__|__|__|__|__|__|__|
___| |___|
###/ \###|
\#/   \##/
 ``     `` ");

                Thread.Sleep(300);
                Console.Clear();
                HangmanText();
                Console.WriteLine(@"

   |_______________``\
    [/]           [  ]
   [\]            | ||
  [/]             |  |
  [\]             |  |
  [/]             || |
 [---]            |  |
 [---]            |@ |
[---]             |  |
OOOOOo            |  |
OO___OOo          | @|
|||||\Oo          |  |
|||||\OOo         |  |
 x x /OO*         |  |
  c  |O*\         |  |
\_O_/    \        |  |
 \#/     |        |  |
      |  |        | ||
      |  |_____   | ||__
______\__|   \    ||||  \
        |      \  \\ ||\  \
   V    |     \//\  \  \  \
   |    |     //  \  \  \  \
   |    |    //|\  \  \  \  \
------------\--- \  \  \  \  \
\  \  \  \  \  \  \  \  \  \  \
_\__\__\__\__\__\__\__\__\__\__\
__|__|__|__|__|__|__|__|__|__|__|
___| |___|
###/ \###|
##/   \##/
 ``     `` ");

                Thread.Sleep(300);
                Console.Clear();
                HangmanText();
                Console.WriteLine(@"GAME OVER!!!

   |_______________``\
    [/]           [  ]
   [\]            | ||
   [/]            |  |
   [\]            |  |
   [/]            || |
  [---]           |  |
  [---]           |@ |
  [---]           |  |
 oOOOOOo          |  |
oOO___OOo         | @|
O/|||||\Oo        |  |
O/|||||\OOo       |  |
O\ x x /OO*       |  |
*|  c  |O*\       |  |
  \_O_/    \      |  |
   \#/     |      |  |
|       |  |      | ||
|       |  |_____ | ||__
/_______\__|  \   ||||  \
         \_|   _  | ||\  \
              \  \\ ||\  \
    V    |    \//\  \  \  \
    |    |   _//  \  \  \  \
    |    |   //|\  \  \  \  \
------------\--- \  \  \  \  \
\  \  \  \  \  \  \  \  \  \  \
_\__\__\__\__\__\__\__\__\__\__\
__|__|__|__|__|__|__|__|__|__|__|
___| |___|
###/ \###|
\#/   \##/
 ``     `` ");
                loopCounter++;
            }
        }


    }
}