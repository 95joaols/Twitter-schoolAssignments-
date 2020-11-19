using System;
using System.Collections.Generic;
using TwitterCore;

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
                Console.WriteLine("[2] User settings");
                Console.WriteLine("[3] Logga ut");
                Console.WriteLine("[4] Show all tweets");
                Console.WriteLine("[5] Search tweets or users");
                Console.WriteLine("[6] My profile");

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
                        UserSettingsMenu(user);
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
                    case ConsoleKey.D5:
                        //userManager.AddFollwingOfUser(user användaren, en till user från sökning);
                        SearchTweetsVersion2(user);
                        break;
                    case ConsoleKey.D6:
                        PrintYourBioAndTweets(user);
                        break;
                    default:
                        System.Console.WriteLine("Invalid menu input");
                        break;
                }
            }
        }

        private static void PrintOthersTweets(User user)
        {
            List<Tuple<string, Tweet>> tweets = tweetManager.GetOthersTweets(user);
            foreach (var tweet in tweets)
            {
                Console.WriteLine("{0}: {1}, {2}, {3}", tweet.Item2.ID, tweet.Item1, tweet.Item2.Message, tweet.Item2.CreateDate);
            }

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
                foreach (var tweet in tweets)
                {
                    if (tweet.Item2.ID == idChoiche)
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
            List<Tuple<string, Tweet>> tweets = tweetManager.GetTweets();

            foreach (var tweet in tweets)
            {
                Console.WriteLine("{0}: {1}, {2}", tweet.Item1, tweet.Item2.Message, tweet.Item2.CreateDate);
            }
        }

        private static void PrintYourBioAndTweets(User user)
        {
            System.Console.WriteLine("Bio: ");
            System.Console.WriteLine(user.Biography);
            Console.WriteLine();

            List<Tuple<string, Tweet>> userTweets = tweetManager.GetUserTweets(user);
            Console.WriteLine("Tweets:");
            foreach (var tweet in userTweets)
            {
                Console.WriteLine("{0}: {1}, {2}, {3}", tweet.Item2.ID, tweet.Item1, tweet.Item2.Message, tweet.Item2.CreateDate);
            }
            System.Console.WriteLine("");


            System.Console.WriteLine("Tryck på [Enter] för att fortsätta ");
            System.Console.WriteLine("Skriv [R] för att se dina retweets");
            System.Console.Write("Eller skriv id på Tweet att ta bort tweet: ");
            string foo = Console.ReadLine();
            int idChoiche;
            bool success = Int32.TryParse(foo, out idChoiche);
            if (foo == string.Empty)
            {
                System.Console.WriteLine("tillbaka till meny");
            }
            else if (foo.ToLower() == "r")
            {
                RetweetMenue(user);
            }
            else if (success)
            {
                bool skip = false;
                foreach (var tweet in userTweets)
                {
                    if (tweet.Item2.ID != idChoiche)
                    {
                        continue;
                    }
                    else if (tweet.Item2.ID == idChoiche)
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

                    IEnumerable<User> fetchedUsers = userManager.SearchUsers(searchString);

                    Console.WriteLine();
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
                }

                else if (userKey == ConsoleKey.D2)
                {
                    Console.Write(Environment.NewLine + "Search: ");
                    string searchString = Console.ReadLine();               // What tweet to search for.

                    IEnumerable<Tweet> fetchedTweets = tweetManager.SearchTweets(searchString);

                    Console.WriteLine();
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
                        tweetManager.Retweet(loggedInUser.Id, userKeyInt);
                    }
                }

                else if (userKey == ConsoleKey.Escape)
                {
                    isSearching = false;
                }
            }
        }

        public static void UserSettingsMenu(User user)
        {
            Console.WriteLine("Ange menyval: ");
            Console.WriteLine("[1] Ange/ändra förnamn");
            Console.WriteLine("[2] Ange/ändra efternamn");
            Console.WriteLine("[3] Ange/ändra biografi");

            userKey = Console.ReadKey(true).Key;

            switch (userKey)
            {
                case ConsoleKey.D1:
                    Console.WriteLine("Ange förnamn: ");
                    string input = Console.ReadLine();
                    userManager.UpdateFirstnameUser(user, input);
                    break;
                case ConsoleKey.D2:
                    Console.WriteLine("Ange efternamn: ");
                    input = Console.ReadLine();
                    userManager.UpdateLastnameUser(user, input);
                    break;
                case ConsoleKey.D3:
                    Console.Write("Write your bio: ");
                    input = Console.ReadLine();
                    userManager.AddBioToUser(input, user);
                    break;
            }
        }

        public static void RetweetMenue(User user)
        {
            List<Tuple<string, Tweet, UserToRetweet>> reTweets = tweetManager.GetRetweets(user);
            System.Console.WriteLine("My ReTweets: ");
            foreach (var reTweet in reTweets)
            {
                Console.WriteLine("{0}: {1}, {2}, {3}", reTweet.Item3.Id, reTweet.Item1, reTweet.Item2.Message, reTweet.Item2.CreateDate);
            }
            System.Console.WriteLine("Tryck på [Enter] för att fortsätta ");
            System.Console.Write("Eller skriv id på Tweet att ta bort tweet: ");
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
                foreach (var tweet in reTweets)
                {
                    if (tweet.Item3.Id != idChoiche)
                    {
                        continue;
                    }
                    else if (tweet.Item3.Id == idChoiche)
                    {
                        skip = true;
                        tweetManager.DeleteReTweet(idChoiche);
                        System.Console.WriteLine("ReTweet raderad!");
                        System.Console.WriteLine("");
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
    }
}
