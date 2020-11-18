using System;
using System.Collections.Generic;
using TwitterCore;
using System.Linq;

namespace Grupparbete
{
    class Program
    {
        static LoginSystem loginSystem = new LoginSystem();
        static TweetManager tweetManager = new TweetManager();
        static UserManager userManager = new UserManager();
        static ConsoleKey userKey;

        static void Main(string[] args)
        {
            PrintTweets();
            PrintHeadMenu();
        }

        private static void LogginLogic()
        {
            while(true)
            {
                Console.Write("Write Username: ");
                string username = Console.ReadLine();
                Console.Write("Write Password: ");
                string password = Console.ReadLine();

                var value = loginSystem.LogInUser(username, password);
                bool auth = value.Item1;
                User user = value.Item2;
                if (auth)
                {
                    System.Console.WriteLine("Login successful");
                    TweetMenu(user);
                }
                else
                {
                    System.Console.WriteLine("Access denied");
                }
            }
        }

        private static void TweetMenu(User user)
        {
            while(true)
            {
                Console.WriteLine("[1] Add Twitter Post");
                Console.WriteLine("[2] Lägg till Bio info");
                Console.WriteLine("[3] Logga ut");
                Console.WriteLine("[4] Remove tweet");
                Console.WriteLine("[5] Search Post");
                Console.WriteLine("[6] ?");

                userKey = Console.ReadKey(true).Key;
                switch (userKey)
                {
                    case ConsoleKey.D1:
                        Console.Write("Write Tweet: ");
                        string tweet = Console.ReadLine();
                        tweetManager.CreateTweet(tweet, user.Id);
                        break;
                    case ConsoleKey.D2:
                        Console.Write("Write your bio: ");
                        string bio = Console.ReadLine();
                        userManager.AddBioToUser(bio,user);
                        break;
                    case ConsoleKey.D3:
                        loginSystem.LogOutUser(user);
                        PrintHeadMenu();
                        System.Console.WriteLine("Logged out");
                        break;
                    case ConsoleKey.D4:
                        tweetManager.Delete(2, user);
                        // söka användare
                        break;
                    case ConsoleKey.D5:
                        //userManager.AddFollwingOfUser(user användaren, en till user från sökning);
                        SearchTweets(user);
                        break;
                    default:
                        System.Console.WriteLine("error");
                        break;
                }
            }
        }

        private static void CreateUserMenu()
        {
            System.Console.WriteLine("Create an user");
            System.Console.Write("Write an username: ");
            string username = Console.ReadLine();
            System.Console.Write("Write a password: ");
            string password = Console.ReadLine();
            loginSystem.CreateUser(username, password);
            System.Console.WriteLine("User created");
        }

        public static void PrintHeadMenu()
        {
            while (true)
            {
                System.Console.WriteLine("[1] Create an user");
                System.Console.WriteLine("[2] Login");
                Console.WriteLine("[Esc] Exit program");
                userKey = Console.ReadKey(true).Key;
                switch (userKey)
                {
                    case ConsoleKey.D1:
                        CreateUserMenu();
                        break;
                    case ConsoleKey.D2:
                        LogginLogic();
                        break;
                    case ConsoleKey.Escape:
                        Environment.Exit(0);
                        break;
                    default:
                        System.Console.WriteLine("Error");
                        break;
                }
            }
        }

        public static void PrintTweets()
        {
            List<Tweet> tweets = tweetManager.GetTweets(10);

            foreach (Tweet tweet in tweets)
            {
                Console.WriteLine("{0}: {1}, {2}", tweet.Username, tweet.Message, tweet.CreateDate);
            }
    }

        public static void foo()
        {
        }

        public static void SearchTweets(User loggedInUser)
        {
            Console.Write("Search: ");
            string searchString = Console.ReadLine();               // What to search for.

            Console.WriteLine();
            List<Search> fetchedSearchProc = tweetManager.SearchUsersAndTweets(searchString) as List<Search>;           // TODO: Overkill to use a List?
            foreach (Search x in fetchedSearchProc)
            {
                Console.WriteLine("User Id: {0}, Username: {1}, Bioagraphy: {2}, Message: {3}, CreateDate: {4}, IdTweet: {5}", x.Id ,x.Username, x.Biography, x.Message, x.CreateDate, x.IdTweet);
            }

            while (true)
            {
                Console.WriteLine(Environment.NewLine + "[1] Sort by users");
                Console.WriteLine("[2] Sort by tweets");
                Console.WriteLine("[3] Default view");
                Console.WriteLine("[Esc] Return to main menu.");
                Console.Write("Option: ");
                userKey = Console.ReadKey(false).Key;

                switch (userKey)
                {
// --------------------------------------------------- SORT BY USERS
                    case ConsoleKey.D1:
                    {
                        var uniqueUsers = fetchedSearchProc.GroupBy(x => new {x.Id, x.Username, x.Biography});              // The type for uniqueUsers is an IEnumerable that holds an IGrouping (seems to be similar to a Dictionary). Not fun to work with!
                        Console.WriteLine(Environment.NewLine);
                        foreach (var x in uniqueUsers)
                        {
                            Console.WriteLine("User Id: {0}, Username: {1}, Biography: {2}", x.Key.Id, x.Key.Username, x.Key.Biography);
                        }
                        Console.WriteLine(Environment.NewLine + "[1] Follow/unfollow");
                        Console.WriteLine("[Esc] Return to search");
                        Console.Write("Option: ");
                        userKey = Console.ReadKey(false).Key;
                        if (userKey == ConsoleKey.D1)
                        {
                            Console.Write(Environment.NewLine + "Choose an \"User Id\" to follow: ");
                            int userKeyInt = Convert.ToInt32(Console.ReadLine());

                            if (uniqueUsers.Any(x => x.Key.Id == userKeyInt))               // TODO: Fult. Det man får ut är en PK, hade varit snyggare ifall användaren såg en lista med "1 > x" istället. Problemet är att jag har väldigt dålig koll på hur man arbetar med typen som uniqueUsers har.
                            {
                                Console.WriteLine("You followed/unfollowed: " + userKeyInt);
                                
                                userManager.AddFollwingOfUser(loggedInUser, userKeyInt);
                                // TODO: TOGGLE FOLLOW METHOD HERE.
                            }
                            else
                                Console.WriteLine("Not a valid User Id (" + userKeyInt + ")");
                        }
                        else if (userKey == ConsoleKey.Escape)
                            break;
                        break;
                    }
// --------------------------------------------------- SORT BY TWEETS
                    case ConsoleKey.D2:
                    {
                        Console.WriteLine(Environment.NewLine);
                        foreach (Search x in fetchedSearchProc)
                        {
                            if (x.IdTweet != 0)
                                Console.WriteLine("IdTweet: {0}, Username: {1}, Message: {2}, CreateDate: {3}", x.IdTweet, x.Username, x.Message, x.CreateDate);
                        }

                        Console.WriteLine(Environment.NewLine + "[1] Retweet");
                        Console.WriteLine("[Esc] Return to search");
                        Console.Write("Option: ");
                        userKey = Console.ReadKey(false).Key;
                        if (userKey == ConsoleKey.D1)
                        {
                            Console.Write(Environment.NewLine + "Choose an \"IdTweet\" to retweet: ");
                            int userKeyInt = Convert.ToInt32(Console.ReadLine());

                            foreach (Search x in fetchedSearchProc)
                            {
                                if (x.IdTweet == userKeyInt)
                                {
                                    Console.WriteLine("You retweeted (" + userKeyInt + ")");
                                    // TODO: RETWEET METHOD HERE.
                                }
                            }
                        }
                        else if (userKey == ConsoleKey.Escape)
                            break;
                        break;
                    }

// --------------------------------------------------- DEFAULT SORT
                    case ConsoleKey.D3:
                    {
                        Console.WriteLine(Environment.NewLine);
                        foreach (Search x in fetchedSearchProc)
                        {
                            Console.WriteLine("User Id: {0}, Username: {1}, Bioagraphy: {2}, Message: {3}, CreateDate: {4}, IdTweet: {5}", x.Id ,x.Username, x.Biography, x.Message, x.CreateDate, x.IdTweet);
                        }
                        break;
                    }

// --------------------------------------------------- ESCAPE
                    case ConsoleKey.Escape:
                    {
                        TweetMenu(new User {Id = 777, Username = "Ghost"});                 // TODO: Sneaky solution! :)
                        break;
                    }

                    default:
                        Console.WriteLine("Not a valid option.");
                    break;
                }
            }
        }
    }
}
