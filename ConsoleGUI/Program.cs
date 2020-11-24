using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using TwitterCore;

namespace ConsoleGUI
{
    internal class Program
    {
        private static readonly LoginSystem loginSystem = new LoginSystem();
        private static readonly TweetManager tweetManager = new TweetManager();
        private static readonly UserManager userManager = new UserManager();
        private static ConsoleKey userKey;

        private static void Main(string[] args)
        {
            InitializeEventListener();
            PrintTweets();
            PrintHeadMenu();
        }

        private static void InitializeEventListener()
        {
            Console.CancelKeyPress += (object sender, ConsoleCancelEventArgs e) => handleLogOut();
            AppDomain.CurrentDomain.UnhandledException += (s, e) => handleLogOut();

            static void handleLogOut()
            {
                if (loginSystem.LoginUser != null && loginSystem.LoginUser.IsLoggedIn)
                {
                    loginSystem.LogOutUser(loginSystem.LoginUser);
                }
            }
        }

        private static void LogginLogic()
        {
            Console.Clear();
            Console.Write("Username: ");
            string username = Console.ReadLine();
            Console.Write("Password: ");
            string password = "";
            string passwordDots = "";
            while (true)
            {
                ConsoleKeyInfo inputKey = Console.ReadKey(true);

                if (inputKey.Key == ConsoleKey.Enter)
                {
                    break;
                }
                else if (inputKey.Key == ConsoleKey.Backspace && password.Length == 0)
                {
                    Console.Clear();
                    Console.WriteLine("Username: " + username);
                    Console.Write("Password: " + passwordDots);
                }

                else if (inputKey.Key == ConsoleKey.Backspace && password.Length > 0)
                {
                    Console.Clear();
                    Console.WriteLine("Username: " + username);
                    password = password.Remove(password.Length - 1);
                    passwordDots = passwordDots.Remove(passwordDots.Length - 1);
                    Console.Write("Password: " + passwordDots);
                }

                else
                {
                    passwordDots += '*';
                    Console.Write("*");
                    password += inputKey.KeyChar;
                }
            }

            Tuple<bool, User> value = new Tuple<bool, User>(false, new User());
            try
            {
                value = loginSystem.LogInUser(username, password);
            }
            catch (Exception e)
            {
                Console.WriteLine(Environment.NewLine + "Error: " + e.Message);
            }
            bool auth = value.Item1;
            User user = value.Item2;
            if (auth)
            {
                Console.WriteLine(Environment.NewLine + "Login successful!");
                TweetMenu(user);
            }
            else
            {
                Console.WriteLine(Environment.NewLine + "Access denied.");
            }
        }

        private static void TweetMenu(User user)
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("[1] Add Twitter Post");
                Console.WriteLine("[2] User settings");
                Console.WriteLine("[3] Show all tweets");
                Console.WriteLine("[4] Search tweets or users");
                Console.WriteLine("[5] My profile");
                Console.WriteLine("[6] My inbox");
                Console.WriteLine("[7] See online users");
                Console.WriteLine("[8] See Friends bio");
                Console.WriteLine("[Esc] Logout");
                Console.WriteLine();

                userKey = Console.ReadKey(true).Key;
                switch (userKey)
                {
                    case ConsoleKey.D1:
                        Console.Clear();
                        Console.Write("Write Tweet: ");
                        string tweet = Console.ReadLine();
                        Console.Clear();
                        if (string.IsNullOrEmpty(tweet))
                        {
                            Console.WriteLine("You can't post an empty tweet");
                        }
                        else
                        {
                            try
                            {
                                tweetManager.CreateTweet(tweet, user.Id);
                                Console.WriteLine("Tweet posted");
                            }
                            catch (System.Exception e)
                            {
                                Console.WriteLine(e.Message);
                            }
                        }
                        break;
                    case ConsoleKey.D2:
                        UserSettingsMenu(user);
                        break;
                    case ConsoleKey.D3:
                        //Visa alla tweets
                        PrintOthersTweets(user);
                        break;
                    case ConsoleKey.D4:
                        SearchTweetsOrUsers(user);
                        break;
                    case ConsoleKey.D5:
                        PrintYourBioAndTweets(user);
                        break;
                    case ConsoleKey.D6:
                        PrintUserInBox(user);
                        break;
                    case ConsoleKey.D7:
                        PrintAllLoggedinNow();
                        break;
                    case ConsoleKey.D8:
                        PrintBiosOfFriends(user);
                        break;
                    case ConsoleKey.Escape:
                        loginSystem.LogOutUser(user);
                        PrintHeadMenu();
                        Console.WriteLine("Logged out");
                        break;
                    default:
                        Console.WriteLine("Invalid menu input");
                        break;
                }
            }
        }

        private static void PrintAllLoggedinNow()
        {
            Console.Clear();
            ReadOnlyCollection<User> onlineUsers = userManager.GetOnlineUser();
            if (onlineUsers.Count == 1)
            {
                Console.WriteLine("No one else is online, press any key to countinue..");
                _ = Console.ReadKey(true);
            }
            else
            {
                Console.WriteLine("Awsome people online below!\n");
                foreach (User onlineUser in onlineUsers)
                {
                    Console.WriteLine("Id:{0} Name: {1}", onlineUser.Id, onlineUser.Username);
                }
                Console.WriteLine("press any key to continue..");
                _ = Console.ReadKey(true);
                Console.Clear();
            }

        }

        private static void PrintUserInBox(User user)
        {
            Console.Clear();
            bool running = true;
            while (running)
            {
                Console.WriteLine("[1] Send Mail to a friend");
                Console.WriteLine("[2] My Mail Conversation");
                Console.WriteLine("[Esc] Back to menu");
                Console.WriteLine();
                userKey = Console.ReadKey(true).Key;
                switch (userKey)
                {
                    case ConsoleKey.D1:
                        PrintSendMailMenu(user);
                        break;
                    case ConsoleKey.D2:
                        ChooseMailConversationOfUser(user);
                        break;
                    case ConsoleKey.Escape:
                        running = false;
                        Console.Clear();
                        Console.WriteLine("Back to menu");
                        break;
                    default:
                        Console.WriteLine("Invalid choice");
                        break;
                }
            }
        }

        private static void PrintSendMailMenu(User user)
        {
            Console.Clear();
            ReadOnlyCollection<Tuple<string, int>> following = userManager.GetFollowing(user); // int = UserToUser.FollowingId, samma som User.Id
            Console.WriteLine("These are the users that you follow:");
            foreach (Tuple<string, int> idAndName in following)
            {
                Console.WriteLine("{0}: {1}", idAndName.Item2, idAndName.Item1);
            }
            Console.Write("Press enter to continue or type an ID of the person you want to send a mail to: ");
            string userInput = Console.ReadLine();
            Console.Clear();
            bool success = int.TryParse(userInput, out int idChoice);
            if (userInput == string.Empty)
            {
                Console.WriteLine("Back to main menu");
            }
            else if (success)
            {
                bool print = true;
                foreach (Tuple<string, int> idAndName in following)
                {
                    if (idAndName.Item2 == idChoice)
                    {
                        print = false;
                        Console.Write("Send your message to " + idAndName.Item1 + ": ");
                        string message = Console.ReadLine();
                        try
                        {
                            userManager.SendMessage(message, user, idAndName.Item2);
                        }
                        catch (System.Exception e)
                        {
                            Console.WriteLine(e.Message);
                            break;
                        }

                        Console.WriteLine("Message has been sent!");
                        break;
                    }
                }
                if (print == true)
                {
                    Console.WriteLine("You do not follow this user, yet!");
                }
            }
            else
            {
                Console.WriteLine("You did not enter a number!");
            }

        }

        private static void ChooseMailConversationOfUser(User user)
        {
            Console.Clear();
            ReadOnlyCollection<Tuple<string, int>> myMail = userManager.GetUserMail(user);
            foreach (Tuple<string, int> mail in myMail)
            {
                Console.WriteLine("Id:{0} Name: {1}", mail.Item2, mail.Item1);
            }
            Console.Write("Press enter to continue or type an Id to reply to that message: ");
            string userInput = Console.ReadLine();
            Console.Clear();
            bool success = int.TryParse(userInput, out int idChoice);
            if (userInput == string.Empty)
            {
                Console.WriteLine("Back to menu");
            }
            else if (success)
            {
                bool print = true;
                foreach (Tuple<string, int> mail in myMail)
                {
                    if (mail.Item2 == idChoice)
                    {
                        print = false;
                        PrintMailConversation(user, idChoice);

                    }
                }
                if (print == true)
                {
                    Console.WriteLine("This Id could not be found in your inbox.");
                }
            }
            else
            {
                Console.WriteLine("You did not write a number!");
            }

        }

        private static void PrintMailConversation(User user, int mailToId)
        {
            Console.Clear();
            ReadOnlyCollection<Tuple<string, PrivateMessage>> mailConvo = userManager.GetMailConven(user, mailToId);
            foreach (Tuple<string, PrivateMessage> m in mailConvo)
            {
                Console.WriteLine("Name: {0} : {1}", m.Item1, m.Item2.Message);
            }
            Console.Write("Press enter to continue, or start typing your message to reply to your friend: ");
            string answer = Console.ReadLine();
            Console.Clear();
            if (answer == string.Empty)
            {
                Console.WriteLine("Back to menu");
            }
            else
            {
                try
                {
                    userManager.SendMessage(answer, user, mailToId);
                }
                catch (System.Exception e)
                {
                    Console.WriteLine(e.Message);
                    return;
                }
                Console.WriteLine("Message has been sent!");
            }
        }

        private static void PrintBiosOfFriends(User user)
        {
            Console.Clear();
            IEnumerable<User> friends = userManager.GetFriendsBio(user);
            Console.WriteLine("These are your friends biographies!\n");
            foreach (User friend in friends)
            {
                Console.WriteLine("Username: " + friend.Username);
                Console.WriteLine("Fristname: " + friend.Firstname);
                Console.WriteLine("Lastname: " + friend.Lastname);
                Console.WriteLine("Bio: " + friend.Biography + "\n");
            }
            Console.WriteLine("Press any key to countinue..");
            userKey = Console.ReadKey(true).Key;
            Console.Clear();
        }

        private static void PrintOthersTweets(User user)
        {
            Console.Clear();
            ReadOnlyCollection<Tuple<string, Tweet>> tweets = tweetManager.GetOthersTweets(user);
            foreach (Tuple<string, Tweet> tweet in tweets)
            {
                Console.WriteLine("{0}: {1}, {2}, {3}", tweet.Item2.ID, tweet.Item1, tweet.Item2.Message, tweet.Item2.CreateDate);
            }

            Console.WriteLine();
            Console.Write("Press enter to continue, or choose a TweetId to retweet: ");
            string userInput = Console.ReadLine();
            Console.Clear();
            bool success = int.TryParse(userInput, out int idChoice);
            if (userInput == string.Empty)
            {
                Console.WriteLine("Back to menu");
            }
            else if (success)
            {
                bool print = false;
                foreach (Tuple<string, Tweet> tweet in tweets)
                {
                    if (tweet.Item2.ID == idChoice)
                    {
                        print = true;
                        tweetManager.Retweet(user.Id, idChoice);
                        Console.WriteLine("This tweet can now be seen on your profile as a retweet!");
                        break;
                    }
                }
                if (print == false)
                {
                    Console.WriteLine("This TweetId is not available for you to retweet.");
                }
            }
            else
            {
                Console.WriteLine("You did not write a number!");
            }
        }

        private static void CreateUserMenu()
        {
            Console.Clear();
            Console.WriteLine("Create a user");
            Console.Write("Write a username: ");
            string username = Console.ReadLine();
            Console.Write("Write a password: ");
            string password = Console.ReadLine();
            bool isSuccessfulUserCreate = true;

            try
            {
                loginSystem.CreateUser(username, password);
            }
            catch (System.Exception e)
            {
                if (e.Message.Contains("Violation of UNIQUE KEY"))
                {
                    Console.WriteLine("User already exists");
                    isSuccessfulUserCreate = false;
                }
                else
                {
                    Console.WriteLine(e.Message);
                    isSuccessfulUserCreate = false;
                }

            }

            if (isSuccessfulUserCreate)
            {
                Console.WriteLine("User created!");
            }
            else if (!isSuccessfulUserCreate)
            {
                Console.WriteLine("No new user was not created.");
            }
        }
        public static void PrintHeadMenu()
        {
            while (true)
            {
                Console.WriteLine("[1] Create a user");
                Console.WriteLine("[2] Login");
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
                        Console.WriteLine("Incorrect menu input");
                        break;
                }
            }
        }

        public static void PrintTweets()
        {
            ReadOnlyCollection<Tuple<string, Tweet>> tweets = tweetManager.GetTweets();

            foreach (Tuple<string, Tweet> tweet in tweets)
            {
                Console.WriteLine("{0}: {1}, {2}", tweet.Item1, tweet.Item2.Message, tweet.Item2.CreateDate);
            }
            Console.WriteLine();
        }

        private static void PrintYourBioAndTweets(User user)
        {
            Console.Clear();
            Console.WriteLine("Bio: ");
            Console.WriteLine(user.Biography);
            Console.WriteLine();

            ReadOnlyCollection<Tuple<string, Tweet>> userTweets = tweetManager.GetUserTweets(user);
            Console.WriteLine("Tweets:");
            foreach (Tuple<string, Tweet> tweet in userTweets)
            {
                Console.WriteLine("{0}: {1}, {2}, {3}", tweet.Item2.ID, tweet.Item1, tweet.Item2.Message, tweet.Item2.CreateDate);
            }
            Console.WriteLine("");


            Console.WriteLine("Press [Enter] to continue");
            Console.WriteLine("Type [R] to see your retweets");
            Console.Write("Or type and Id for a Tweet to delete it: ");
            string userInput = Console.ReadLine();
            Console.Clear();
            bool success = int.TryParse(userInput, out int idChoice);
            if (userInput == string.Empty)
            {
                Console.WriteLine("Back to menu");
            }
            else if (userInput.ToLower() == "r")
            {
                RetweetMenu(user);
            }
            else if (success)
            {
                bool skip = false;
                foreach (Tuple<string, Tweet> tweet in userTweets)
                {
                    if (tweet.Item2.ID != idChoice)
                    {
                        continue;
                    }
                    else if (tweet.Item2.ID == idChoice)
                    {
                        skip = true;
                        tweetManager.Delete(idChoice);
                        Console.WriteLine("Tweet deleted!");
                        break;
                    }

                }
                if (skip == false)
                {
                    Console.WriteLine("This TweetId does not exist or is not yours to delete!");
                }
            }
            else
            {
                Console.WriteLine("You did not write a number!");
            }
        }

        public static void SearchTweetsOrUsers(User loggedInUser)
        {
            Console.Clear();
            bool isSearching = true;
            while (isSearching)
            {
                Console.WriteLine("[1] Search users");
                Console.WriteLine("[2] Search tweets");
                Console.WriteLine("[Esc] Return to main menu.");
                Console.Write("Option: ");
                userKey = Console.ReadKey(false).Key;

                if (userKey == ConsoleKey.D1)                               // Search user.
                {
                    Console.Write(Environment.NewLine + "Search: ");
                    string searchString = Console.ReadLine();               // What user to search for.
                    IEnumerable<User> fetchedUsers = userManager.SearchUsers(searchString, loggedInUser);
                    Console.WriteLine();
                    if (fetchedUsers.ToList().Count == 0)
                    {
                        Console.WriteLine("No user found");
                    }
                    else
                    {
                        foreach (User x in fetchedUsers)
                        {
                            Console.WriteLine("{0} {1} ------- {2} {3}", x.Id, x.Username, x.Firstname, x.Lastname);
                            Console.WriteLine("  Biography: {0}", x.Biography);
                        }
                        Console.Write("Press enter to continue, or write an id to follow: ");
                        string userInput = Console.ReadLine();
                        bool success = int.TryParse(userInput, out int userInt);
                        if (userInput == string.Empty)
                        {
                            Console.Clear();
                            break;
                        }
                        else if (success)
                        {
                            bool skip = false;
                            foreach (User user in fetchedUsers)
                            {
                                if (user.Id != userInt)
                                {
                                    continue;
                                }
                                else if (user.Id == userInt)
                                {
                                    skip = true;
                                    Console.Clear();
                                    Console.WriteLine("You follow a new user");
                                    userManager.AddFollwingOfUser(loggedInUser, userInt);
                                    break;
                                }
                            }
                            if (skip == false)
                            {
                                Console.Clear();
                                Console.WriteLine("This Id does not exist in this context!");
                            }
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine("You did not write a number!");
                        }
                    }
                }
                else if (userKey == ConsoleKey.D2)
                {
                    Console.Write(Environment.NewLine + "Search: ");
                    string searchString = Console.ReadLine();               // What tweet to search for.
                    ReadOnlyCollection<Tuple<string, Tweet>> fetchedTweets = tweetManager.SearchTweets(searchString);
                    Console.WriteLine();
                    if (fetchedTweets.ToList().Count == 0)
                    {
                        Console.Clear();
                        Console.WriteLine("No tweet found");
                    }
                    else
                    {
                        Console.Clear();
                        foreach (Tuple<string, Tweet> x in fetchedTweets)
                        {
                            Console.WriteLine("{0}, {1}: {2} {3}", x.Item2.ID, x.Item1, x.Item2.Message, x.Item2.CreateDate);
                        }
                        Console.Write("Press enter to continue, or write an id to retweet: ");
                        string userInput = Console.ReadLine();
                        bool success = int.TryParse(userInput, out int userInt);
                        if (userInput == string.Empty)
                        {
                            break;
                        }
                        else if (success)
                        {
                            bool skip = false;
                            foreach (Tuple<string, Tweet> x in fetchedTweets)
                            {
                                if (x.Item2.ID != userInt)
                                {
                                    continue;
                                }
                                else if (x.Item2.ID == userInt)
                                {
                                    skip = true;
                                    tweetManager.Retweet(loggedInUser.Id, x.Item2.ID);
                                    Console.Clear();
                                    Console.WriteLine("You have retweeted the tweet, its now on your bio also");
                                    break;
                                }
                            }
                            if (skip == false)
                            {
                                Console.Clear();
                                Console.WriteLine("This Id does not exist in this context!");
                            }
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine("You did not write a number!");
                        }
                    }
                }
                else if (userKey == ConsoleKey.Escape)
                {
                    isSearching = false;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Invalid input");
                }
            }
        }

        public static void UserSettingsMenu(User user)
        {
            bool running = true;
            while (running)
            {
                Console.Clear();
                Console.WriteLine("Choose a menu option: ");
                Console.WriteLine("[1] Set/change your firstname");
                Console.WriteLine("[2] Set/change your lastname");
                Console.WriteLine("[3] Set/change your biography");
                Console.WriteLine("[Esc] Return to menu");

                userKey = Console.ReadKey(true).Key;

                switch (userKey)
                {
                    case ConsoleKey.D1:
                        Console.WriteLine("Firstname: ");
                        string input = Console.ReadLine();
                        try
                        {
                            userManager.UpdateFirstnameUser(user, input);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Error: " + e.Message);
                            Console.WriteLine("Press any key to continue..");
                            _ = Console.ReadLine();
                        }
                        break;
                    case ConsoleKey.D2:
                        Console.WriteLine("Lastname: ");
                        input = Console.ReadLine();

                        try
                        {
                            userManager.UpdateLastnameUser(user, input);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Error: " + e.Message);
                            Console.WriteLine("Press any key to continue..");
                            _ = Console.ReadLine();
                        }
                        break;
                    case ConsoleKey.D3:
                        Console.Write("Write your bio: ");
                        input = Console.ReadLine();
                        Console.Clear();
                        try
                        {
                            userManager.AddBioToUser(input, user);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Error: " + e.Message);
                            Console.WriteLine("Press any key to continue..");
                            _ = Console.ReadLine();
                        }
                        break;
                    case ConsoleKey.Escape:
                        running = false;
                        Console.WriteLine("Back to menu\n");
                        break;
                    default:
                        Console.WriteLine("\nInvalid input\n");
                        break;
                }
            }
        }

        public static void RetweetMenu(User user)
        {
            ReadOnlyCollection<Tuple<string, Tweet, UserToRetweet>> reTweets = tweetManager.GetRetweets(user);
            Console.WriteLine("My Retweets: ");
            foreach (Tuple<string, Tweet, UserToRetweet> reTweet in reTweets)
            {
                Console.WriteLine("{0}: {1}, {2}, {3}", reTweet.Item3.Id, reTweet.Item1, reTweet.Item2.Message, reTweet.Item2.CreateDate);
            }
            Console.WriteLine("Press enter to continue");
            Console.Write("Or type the Id of a Tweet to delete it: ");
            string userInput = Console.ReadLine();
            Console.Clear();
            bool success = int.TryParse(userInput, out int idChoice);
            if (userInput == string.Empty)
            {
                Console.WriteLine("Back to menu");
            }
            else if (success)
            {
                bool skip = false;
                foreach (Tuple<string, Tweet, UserToRetweet> tweet in reTweets)
                {
                    if (tweet.Item3.Id != idChoice)
                    {
                        continue;
                    }
                    else if (tweet.Item3.Id == idChoice)
                    {
                        skip = true;
                        tweetManager.DeleteReTweet(idChoice);
                        Console.WriteLine("Retweet deleted!");
                        Console.WriteLine("");
                        break;
                    }
                }
                if (skip == false)
                {
                    Console.WriteLine("This TweetId does not exist or is not yours to delete!");
                }
            }
            else
            {
                Console.WriteLine("You did not write a number!");
            }
        }
    }
}
