using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;

// Imports the struct including the media player information
using mpStruct;

namespace soundSharp

{
    class Program
    {
        // This makes a list containing 
        public static List<MediaPlayer> MediaPlayerList = new List<MediaPlayer>();

        // Sets the maximum amount of password tries.
        public static int LoginTries;
        public static int LoginMaxTries = 3;
        public static Boolean ClearScreenFalse = false;

        public static void Main(string[] args)
        {
            if (LoginAdmin(args))
            {
                MediaPlayerListInfo();
                ShowAssortment();
                Console.WriteLine("\n");
                ShowStock();
                Console.WriteLine("\n");
                ShowStatistics();
                ReturnMenu();
            }
            else if (Login(args))
            {
                MediaPlayerListInfo();
                ShowMenu();
                MenuOptions();
            }
        }

        // Checks if the admin parameters match the admins username and password
        static Boolean LoginAdmin(string[] args)
        {
            // Set password of the admin to soundsharp
            string adminPass = "soundsharp";

            // If the program is started with the arguments admin and the correct password the program will login. 
            if (args.Length == 2 && args[0] == "admin" && args[1] == adminPass)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // Checks the right password. Returns true or false.
        static Boolean Login(string[] args)
        {
            // Set password of the user to SOUNDSHARP
            string userPass = "";

            // This is the normal login procedure 
            Console.Write("Username: ");
            string userName = Console.ReadLine();

            Console.Write("Password: ");
            do
            {
                LoginTries++;
                if (LoginTries == (LoginMaxTries))
                {
                    Console.Write("Last try! ");
                }
                else if (LoginTries > 1)
                {
                    Console.Write("Try {0} of {1}: ", LoginTries, LoginMaxTries);
                }
                string userPassInput = Console.ReadLine();
                if (userPassInput == userPass)
                {
                    ClearScreen();
                    Console.WriteLine("Welcome to SoundSharp {0}!\n", userName);
                    ClearScreenFalse = true;
                    return true;
                }
            } while (LoginTries < LoginMaxTries);
            return false;
        }

        // Returns a int from a ReadKey used in chosing the menu options
        static int MenuInput()
        {
            while (true)
            {
                int userInput;
                if (int.TryParse(Console.ReadKey().KeyChar.ToString(), out userInput))
                {
                    ClearScreen();
                    return userInput;
                }
                else
                {
                    Console.WriteLine("Please enter a number.\n");
                    return -1;
                }
            }
        }

        // Contains a loop that will run untill the program closes. 
        static void MenuOptions()
        {
            while (true)
            {
                switch (MenuInput())
                {
                    case 1:
                        ShowAssortment();
                        break;
                    case 2:
                        ShowStock();
                        break;
                    case 3:
                        ShowMutation();
                        break;
                    case 4:
                        ShowStatistics();
                        break;
                    case 5:
                        ShowAddPlayer();
                        break;
                    case 6:
                    case 7:
                    case 8:
                        break;
                    case 9:
                        return;
                    default:
                        ClearScreen();
                        Console.WriteLine("Please enter a number (1 up to 9).\n");
                        break;
                }
                ReturnMenu();
                ShowMenu();
            }
        }

        // This method shows the assortment of media players.
        static void ShowAssortment()
        {
            Console.WriteLine("Assortment:\n");
            foreach (MediaPlayer mpData in MediaPlayerList)
            {
                Console.WriteLine("ID: {0} make: {1, -13} model: {2, -10} capacity: {3, -4} price: {4, -6} stock: {5}", mpData.Id, mpData.Make, mpData.Model, mpData.Mb, mpData.Price, mpData.Stock);
            }
        }

        // This method shows the amount of media players in stock.
        static void ShowStock()
        {
            Console.WriteLine("Overview stock:\n");
            foreach (MediaPlayer mpData in MediaPlayerList)
            {
                Console.WriteLine("ID {0}, stock {1}", mpData.Id, mpData.Stock);
            }
        }

        // This method mutates the amount of stock a media player has.
        static void ShowMutation()
        {
            Console.WriteLine("Mutate stock:\n");

            // Prints the amount of stock of media players
            ShowStock();

            // getUserInput gets converts the keyboard input to a usable int.
            Console.WriteLine("\nPlease enter the ID of the media player you'd like to change:");
            int mutateID = (returnNumber() - 1);
            if (mutateID > MediaPlayerList.Count || mutateID < 0)
            {
                Console.WriteLine("Not a valid ID.");
                Console.ReadKey();
                return;
            }

            // Gets the amount of stock that should be changed
            Boolean mutateAttempt = true;
            while (mutateAttempt == true)
            {


                MediaPlayer mpMutateTemp = MediaPlayerList[mutateID];
                Console.WriteLine("\nPlease enter a number not lower than -{0}:", mpMutateTemp.Stock);
                // mutateAmount gets set to the amount the user wants to 
                int mutateAmount = returnNumber();
                if ((mpMutateTemp.Stock = (mpMutateTemp.Stock + mutateAmount)) < 0)
                {
                    Console.WriteLine("Not enough items in stock.");
                }
                else
                {
                    Console.WriteLine("New stock: {0}", mpMutateTemp.Stock);
                    // Changes made to the stock will be saved
                    MediaPlayerList[mutateID] = mpMutateTemp;
                    mutateAttempt = false;
                }
            }
        }

        // This method shows the total amount and price of the media players and the average price of all mp in stock.
        static void ShowStatistics()
        {
            Console.WriteLine("Statistics:\n");

            float totalPrice = 0;
            int totalPlayers = 0;
            List<float> mediaPlayerPriceMb = new List<float>();

            foreach (MediaPlayer mpData in MediaPlayerList)
            {
                totalPrice = ((mpData.Price * mpData.Stock) + totalPrice);
                totalPlayers = (mpData.Stock + totalPlayers);
                mediaPlayerPriceMb.Add(mpData.Mb / mpData.Price);
            }

            // 1 The total amount of mps in stock.
            Console.WriteLine("Total amount of media players in stock: \n{0} media players\n", totalPlayers);

            // 2 The total value of mps in stock.
            Console.WriteLine("Total value of all media players in stock: \n{0} euro\n", totalPrice);

            // 3 The average price of a mp in stock.
            float averagePrice = (totalPrice / totalPlayers);
            Console.WriteLine("Average price of a media player in stock: \n{0} euro", averagePrice);

            // 4 The mp with the best price per MB.
            int mediaPlayerPriceMbId = mediaPlayerPriceMb.IndexOf(mediaPlayerPriceMb.Min());
            Console.WriteLine("\nPlayer with the best price per MB:\n");
            Console.WriteLine("ID:       {0}\nmake:     {1}\nmodel:    {2}\ncapacity: {3}\nprice:    {4}\nstock:    {5}", MediaPlayerList[mediaPlayerPriceMbId].Id, MediaPlayerList[mediaPlayerPriceMbId].Make, MediaPlayerList[mediaPlayerPriceMbId].Model, MediaPlayerList[mediaPlayerPriceMbId].Mb, MediaPlayerList[mediaPlayerPriceMbId].Price, MediaPlayerList[mediaPlayerPriceMbId].Stock);
        }

        // This method adds a media player to the list.
        static void ShowAddPlayer()
        {
            Console.WriteLine("Please enter the information about the media player that you'd like to add.\n\nID: {0}", (MediaPlayerList.Count + 1));
            Console.Write("Make: ");
            string newMake = Console.ReadLine();
            Console.Write("Model: ");
            string newModel = Console.ReadLine();
            Console.Write("MB: ");
            float newMB = returnFloat();
            Console.Write("Price: ");
            float newPrice = returnFloat();

            // Adds a MediaPlayer with the entered information
            MediaPlayerList.Add(new MediaPlayer((MediaPlayerList.Count + 1), 0, newMake, newModel, newMB, newPrice));
        }

        // This returns a integer that was entered by the user.
        static int returnNumber()
        {
            while (true)
            {
                try
                {
                    int userInput = Convert.ToInt32(Console.ReadLine());
                    return userInput;
                }
                catch (System.OverflowException)
                {
                    Console.WriteLine("This number is to big.");
                }
                catch
                {
                    Console.WriteLine("Please enter a number.");
                }
            }
        }

        // This returns a float that was entered by the user.
        static float returnFloat()
        {
            while (true)
            {
                try
                {
                    float userInput = Convert.ToSingle(Console.ReadLine());
                    return userInput;
                }
                catch (System.OverflowException)
                {
                    Console.WriteLine("This number is to big.");
                }
                catch
                {
                    Console.WriteLine("Please enter a number.");
                }
            }
        }

        // This method clears the console.
        static void ClearScreen()
        {
            if (ClearScreenFalse == false)
            {
                Console.Clear();
            }
            else
            {
                ClearScreenFalse = false;
            }
        }

        // Shows the menu after the user presses a button
        static void ReturnMenu()
        {
            Console.WriteLine("\nTo return to the menu press any button.");
            Console.ReadKey();
        }

        // Checks if the console should be cleared and shows the menu options.
        static void ShowMenu()
        {
            ClearScreen();
            Console.WriteLine("1. Assortment");
            Console.WriteLine("2. Overview stock");
            Console.WriteLine("3. Mutate stock");
            Console.WriteLine("4. Statistics");
            Console.WriteLine("5. Add a media player");
            Console.WriteLine("6.");
            Console.WriteLine("7.");
            Console.WriteLine("8.");
            Console.WriteLine("9. Exit the application");
        }

        // Loads 5 different media players.
        public static void MediaPlayerListInfo()
        {
            MediaPlayerList.Add(new MediaPlayer(1, 500, "GetTech inc.", "HF 410", 4096f, 129.99f));
            MediaPlayerList.Add(new MediaPlayer(2, 500, "Far & Loud", "XM 600", 8192f, 224.95f));
            MediaPlayerList.Add(new MediaPlayer(3, 500, "Innotivative", "Z3", 512f, 79.95f));
            MediaPlayerList.Add(new MediaPlayer(4, 500, "Resistance", "3001", 4096f, 124.95f));
            MediaPlayerList.Add(new MediaPlayer(5, 500, "CBA", "NXT Volume", 2048f, 159.05f));
        }
    }
}
