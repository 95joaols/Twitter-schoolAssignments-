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
            while (true)
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
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("[1] Add Twitter Post");
                Console.WriteLine("[2] Lägg till Bio info");
                Console.WriteLine("[3] Logga ut");
                //Console.WriteLine("[4] Remove tweet");
                //                Console.WriteLine("[5] Search Post");
                Console.WriteLine("[4] Show all tweets");
                Console.WriteLine("[5] Search Post");
                Console.WriteLine("[6] My profile");
                Console.WriteLine("[7] Search X");

                userKey = Console.ReadKey(true).Key;
                switch (userKey)
                {
                    case ConsoleKey.D1:
                        Console.Write("Write Tweet: ");
                        string tweet = Console.ReadLine();
                        if (String.IsNullOrEmpty(tweet))
                        {
                            Console.WriteLine("You can't post an empty tweet");
                        }
                        else
                        {
                            tweetManager.CreateTweet(tweet, user.Id);
                        }
                        break;
                    case ConsoleKey.D2:
                        Console.Write("Write your bio: ");
                        string bio = Console.ReadLine();
                        userManager.AddBioToUser(bio, user);
                        break;
                    case ConsoleKey.D3:
                        loginSystem.LogOutUser(user);
                        PrintHeadMenu();
                        System.Console.WriteLine("Logged out");
                        break;
                    case ConsoleKey.D4:
                        //Visa alla tweets
                        PrintOthersTweets(user);
                        break;
                    /*                    case ConsoleKey.D5:
                                            //userManager.AddFollwingOfUser(user användaren, en till user från sökning);
                                            SearchTweets(user);
                                            break; */
                    case ConsoleKey.D6:
                        PrintYourBioAndTweets(user);
                        break;
                    case ConsoleKey.D7:
                        SearchTweetsVersion2(user);
                        break;
                    default:
                        System.Console.WriteLine("Invalid menu input");
                        break;
                }
            }
        }

        private static void PrintOthersTweets(User user)
        {
            List<Tweet> tweets = tweetManager.GetOthersTweets(user);
            foreach (Tweet tweet in tweets)
            {
                Console.WriteLine("{0}: {1}, {2}, {3}", tweet.ID, tweet.Username, tweet.Message, tweet.CreateDate);
            }

            // Console.WriteLine("Välj tweet att retweeta: ");
            // int idChoice = int.Parse(Console.ReadLine());
            // bool skip = false;

            // foreach (Tweet tweet in tweets)
            // {
            //     if (tweet.ID == idChoice)
            //     {
            //         tweetManager.Retweet(user.Id, idChoice);
            //         skip = true;
            //     }
            // }
            // if (!skip)
            // {
            //     Console.WriteLine("Du valde felaktigt");
            // }

            System.Console.Write("Tryck på Enter för att fortsätta. Eller välj ett TweetId för att retweeta: ");
            string foo = Console.ReadLine();
            int idChoiche;
            bool success = Int32.TryParse(foo, out idChoiche);
            if (foo == string.Empty)
            {
                System.Console.WriteLine("tillbaka till meny");
            }
            else if (success)
            {
                bool print = false;
                foreach (Tweet tweet in tweets)
                {
                    if (tweet.ID == idChoiche)
                    {
                        print = true;
                        tweetManager.Retweet(user.Id, idChoiche);
                        System.Console.WriteLine("Tweeten finns nu även på din profil");
                        break;
                    }
                }
                if (print == false)
                {
                    System.Console.WriteLine("Detta TweetId finns inte tillgängligt att retweeta");
                }
            }
            else
            {
                Console.WriteLine("Du skrev inte in en siffra!");
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
            List<Tweet> tweets = tweetManager.GetTweets();

            foreach (Tweet tweet in tweets)
            {
                Console.WriteLine("{0}: {1}, {2}", tweet.Username, tweet.Message, tweet.CreateDate);
            }
        }

        private static void PrintYourBioAndTweets(User user)
        {
            System.Console.WriteLine("Bio: ");
            System.Console.WriteLine(user.Biography);
            Console.WriteLine();

            IEnumerable<Tweet> userTweets = tweetManager.GetUserTweets(user);
            Console.WriteLine("Tweets:");
            foreach (Tweet tweet in userTweets)
            {
                Console.WriteLine("{0}: {1}, {2}, {3}", tweet.ID, tweet.Username, tweet.Message, tweet.CreateDate);
            }

            System.Console.Write("Tryck på Enter för att fortsätta. Eller skriv in ett id på Tweet att ta bort: ");
            string foo = Console.ReadLine();
            int idChoiche;
            bool success = Int32.TryParse(foo, out idChoiche);
            if (foo == string.Empty)
            {
                System.Console.WriteLine("tillbaka till meny");
            }
            else if (success)
            {
                bool skip = false;
                foreach (Tweet tweet in userTweets)
                {
                    if (tweet.ID != idChoiche)
                    {
                        continue;
                    }
                    else if (tweet.ID == idChoiche)
                    {
                        skip = true;
                        tweetManager.Delete(idChoiche, user);
                        System.Console.WriteLine("Tweet raderad!");
                        break;
                    }

                }
                if (skip == false)
                {
                    System.Console.WriteLine("Detta TweetId finns inte eller är inte din att ta bort!");
                }
            }
            else
            {
                Console.WriteLine("Du skrev inte in en siffra!");
            }
        }

        public static void SearchTweetsVersion2(User loggedInUser)
        {
            bool isSearching = true;

            while (isSearching)
            {
                Console.WriteLine(Environment.NewLine + "[1] Search users");
                Console.WriteLine("[2] Search tweets");
                Console.WriteLine("[Esc] Return to main menu.");
                Console.Write("Option: ");
                userKey = Console.ReadKey(false).Key;

                if (userKey == ConsoleKey.D1)
                {
                    Console.Write(Environment.NewLine + "Search: ");
                    string searchString = Console.ReadLine();               // What user to search for.

                    IEnumerable<User> fetchedUsers = tweetManager.SearchUsers(searchString);

                    Console.WriteLine("Count: " + fetchedUsers.Count());
                    foreach (var x in fetchedUsers)
                        Console.WriteLine("User Id: {0}, Username: {1}, Firstname: {2}, Lastname: {3}, Biography: {4}", x.Id, x.Username, x.Firstname, x.Lastname, x.Biography);

                    Console.WriteLine(Environment.NewLine + "[1] Follow/unfollow");
                    Console.WriteLine("[Anything else] Return to search.");
                    Console.Write("Option: ");
                    userKey = Console.ReadKey(false).Key;

                    if (userKey == ConsoleKey.D1)
                    {
                        Console.Write(Environment.NewLine + "Choose an \"User Id\" to follow: ");
                        int userKeyInt = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine("userKeyInt: " + userKeyInt);
                        userManager.AddFollwingOfUser(loggedInUser, userKeyInt);
                    }

                    /*                    else if (userKey == ConsoleKey.Escape)
                    {
                    } */


                }

                else if (userKey == ConsoleKey.D2)              // Tweets
                {
                    Console.Write(Environment.NewLine + "Search: ");
                    string searchString = Console.ReadLine();               // What tweet to search for.

                    IEnumerable<Tweet> fetchedTweets = tweetManager.SearchTweets(searchString);

                    //Console.WriteLine("Count: " + fetchedUsers.Count());
                    foreach (var x in fetchedTweets)
                        Console.WriteLine("Tweet Id: {0}, Message: {1}, CreateDate: {2}, UserId: {3}", x.ID, x.Message, x.CreateDate, x.UserID);

                    Console.WriteLine(Environment.NewLine + "[1] Retweet");
                    Console.WriteLine("[Anything else] Return to search.");
                    Console.Write("Option: ");
                    userKey = Console.ReadKey(false).Key;

                    if (userKey == ConsoleKey.D1)
                    {
                        Console.Write(Environment.NewLine + "Choose an \"Tweet Id\" to retweet: ");
                        int userKeyInt = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine("userKeyInt: " + userKeyInt);
                        // TODO: RETWEET METHOD HERE!
                    }
                }

                else if (userKey == ConsoleKey.Escape)
                {
                    isSearching = false;
                }
            }
        }









        /*        public static void SearchTweets(User loggedInUser)
                {
                    Console.Write("Search: ");
                    string searchString = Console.ReadLine();               // What to search for.

                    Console.WriteLine();
                    List<Search> fetchedSearchProc = tweetManager.SearchUsersAndTweets(searchString) as List<Search>;           // TODO: Overkill to use a List?
                    foreach (Search x in fetchedSearchProc)
                    {
                        Console.WriteLine("User Id: {0}, Username: {1}, Biography: {2}, Message: {3}, CreateDate: {4}, IdTweet: {5}", x.Id, x.Username, x.Biography, x.Message, x.CreateDate, x.IdTweet);
                    }

                    bool searchMenuRunning = true;

                    while (searchMenuRunning)
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
                                    var uniqueUsers = fetchedSearchProc.GroupBy(x => new { x.Id, x.Username, x.Biography });              // The type for uniqueUsers is an IEnumerable that holds an IGrouping (seems to be similar to a Dictionary). Not fun to work with!
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
                                        Console.WriteLine("User Id: {0}, Username: {1}, Bioagraphy: {2}, Message: {3}, CreateDate: {4}, IdTweet: {5}", x.Id, x.Username, x.Biography, x.Message, x.CreateDate, x.IdTweet);
                                    }
                                    break;
                                }

                            // --------------------------------------------------- ESCAPE
                            case ConsoleKey.Escape:
                                {
                                    // TweetMenu(new User { Id = 777, Username = "Ghost" });                 // TODO: Sneaky solution! :)
                                    // break;
                                    searchMenuRunning = false;
                                    break;
                                }

                            default:
                                Console.WriteLine("Not a valid option.");
                                break;
                        }
                    }
                } */
    }
}
