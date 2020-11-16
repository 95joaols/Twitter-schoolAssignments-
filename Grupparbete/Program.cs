using System;
using System.Collections.Generic;
using TwitterCore;

namespace Grupparbete
{
    class Program
    {
        static LoginSystem loginSystem = new LoginSystem();
        static TweetManager tweetManager = new TweetManager();
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
                    System.Console.WriteLine("Inloggad");
                    TweetMenu(user);
                }
                else
                {
                    System.Console.WriteLine("this user dosent even exist");
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
                Console.WriteLine("[4] Search User");
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
                        tweetManager.AddBioToUser(bio,user);
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
                        // söka post
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
            System.Console.Write("Write an password: ");
            string password = Console.ReadLine();
            loginSystem.CreateUser(username, password);
            System.Console.WriteLine("User created");
        }

        public static void PrintHeadMenu()
        {
            while (true)
            {
                System.Console.WriteLine("[1]Create an user");
                System.Console.WriteLine("[2]login");
                userKey = Console.ReadKey(true).Key;
                switch (userKey)
                {
                    case ConsoleKey.D1:
                        CreateUserMenu();
                        break;
                    case ConsoleKey.D2:
                        LogginLogic();
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
    }
}
