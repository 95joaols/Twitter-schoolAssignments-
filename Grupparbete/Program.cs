using System;
using System.Collections.Generic;
using TwitterCore;
using System.Collections.ObjectModel;
using System.Linq;

namespace Grupparbete
{
    class Program
    {
        static readonly LoginSystem loginSystem = new LoginSystem();
        static readonly TweetManager tweetManager = new TweetManager();
        static readonly UserManager userManager = new UserManager();
        static ConsoleKey userKey;

        static void Main(string[] args)
        {
            InitializeEventListener();
            PrintTweets();
            PrintHeadMenu();
        }

        static void InitializeEventListener()
        {
            Console.CancelKeyPress += (object sender, ConsoleCancelEventArgs e) =>
            {
                handleLogOut();
            };
            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                handleLogOut();
            };

            static void handleLogOut()
            {
                if (loginSystem.loginUser != null && loginSystem.loginUser.IsLoggedIn)
                {
                    loginSystem.LogOutUser(loginSystem.loginUser);
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
                ConsoleKeyInfo inputKey = System.Console.ReadKey(true);

                if (inputKey.Key == ConsoleKey.Enter)
                    break;

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

            var value = loginSystem.LogInUser(username, password);
            bool auth = value.Item1;
            User user = value.Item2;
            if (auth)
            {
                System.Console.WriteLine(Environment.NewLine + "Login successful!");
                TweetMenu(user);
            }
            else
            {
                System.Console.WriteLine(Environment.NewLine + "Access denied.");
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
                Console.WriteLine("[6] My Friends and Mail");
                Console.WriteLine("[7] See online users");
                Console.WriteLine("[Esc] Logout");
                Console.WriteLine();

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
                        //Visa alla tweets
                        PrintOthersTweets(user);
                        break;
                    case ConsoleKey.D4:
                        SearchTweets(user);
                        break;
                    case ConsoleKey.D5:
                        PrintYourBioAndTweets(user);
                        break;
                    case ConsoleKey.D6:
                        PrintUserInBox(user);
                        break;
                    case ConsoleKey.D7:
                        PrintAllLogdinNow(user);
                        break;
                    case ConsoleKey.Escape:
                        loginSystem.LogOutUser(user);
                        PrintHeadMenu();
                        System.Console.WriteLine("Logged out");
                        break;
                    default:
                        System.Console.WriteLine("Invalid menu input");
                        break;
                }
            }
        }

        private static void PrintAllLogdinNow(User user)
        {
            ReadOnlyCollection<User> onlineUsers = userManager.GetOnlineUser();
            if (onlineUsers.Count == 1)
            {
                System.Console.WriteLine("No one else is online, press any key to countinue..");
                Console.ReadKey(true);
            }
            else
            {
                System.Console.WriteLine("Awsome people online below!\n");
                foreach (var onlineUser in onlineUsers)
                {
                    Console.WriteLine("Id:{0} Name: {1}", onlineUser.Id, onlineUser.Username);
                }
                System.Console.WriteLine("press any key to countinue..");
                Console.ReadKey(true);
            }

        }

        private static void PrintUserInBox(User user)
        {
            Console.WriteLine("[1] See Bios of those I follow");
            Console.WriteLine("[2] Send Mail to one I follow");
            Console.WriteLine("[3] My InBox"); // denna blir väl överföldig
            Console.WriteLine("[4] My Mail Conversation ");
            Console.WriteLine();
            userKey = Console.ReadKey(true).Key;
            switch (userKey)
            {
                case ConsoleKey.D1:
                    PrintBiosOfFriends(user);
                    break;
                case ConsoleKey.D2:
                    PrintSendMailMenue(user);
                    break;
                case ConsoleKey.D3:
                    PrintMyInbox(user);
                    break;
                case ConsoleKey.D4:
                    ChoseMailConversationOfUser(user);
                    break;

                default:
                    System.Console.WriteLine("Invalid choice");
                    break;
            }
        }

        private static void PrintSendMailMenue(User user)
        {
            ReadOnlyCollection<Tuple<string, int>> following = userManager.GetFollowing(user); // int = UserToUser.FollowingId, samma som User.Id
            System.Console.WriteLine("These are the users that you follow:");
            foreach (var idAndName in following)
            {
                Console.WriteLine("{0}: {1}", idAndName.Item2, idAndName.Item1);
            }
            System.Console.Write("Press enter to continue or type an ID of the person you want to send a mail to: ");
            string userInput = Console.ReadLine();
            bool success = Int32.TryParse(userInput, out int idChoice);
            if (userInput == string.Empty)
            {
                System.Console.WriteLine("Back to main menu");
            }
            else if (success)
            {
                bool print = true;
                foreach (var idAndName in following)
                {
                    if (idAndName.Item2 == idChoice)
                    {
                        print = false;
                        System.Console.Write("Send your message to " + idAndName.Item1 + ": ");
                        string message = Console.ReadLine();
                        userManager.SendMassage(message, user, idAndName.Item2);
                        System.Console.WriteLine("Message has been sent!");
                        break;
                    }
                }
                if (print == true)
                {
                    System.Console.WriteLine("You do not follow this user, yet!");
                }
            }
            else
            {
                Console.WriteLine("You did not enter a number!");
            }

        }

        private static void ChoseMailConversationOfUser(User user)
        {
            ReadOnlyCollection<Tuple<string, string, int>> myMail = userManager.GetUserMail(user);
            foreach (var mail in myMail)
            {
                Console.WriteLine("Id:{0} Name: {1}", mail.Item3, mail.Item1);
            }
            System.Console.WriteLine("Press enter to continue or type an Id to reply to that message. ");
            string userInput = Console.ReadLine();
            bool success = Int32.TryParse(userInput, out int idChoice);
            if (userInput == string.Empty)
            {
                System.Console.WriteLine("Back to menu");
            }
            else if (success)
            {
                bool print = true;
                foreach (var mail in myMail)
                {
                    if (mail.Item3 == idChoice)
                    {
                        print = false;
                        PrintMailConversation(user, idChoice);

                    }
                }
                if (print == true)
                {
                    System.Console.WriteLine("This Id could not be found in your inbox.");
                }
            }
            else
            {
                Console.WriteLine("You did not write a number!");
            }

        }

        private static void PrintMailConversation(User user, int mailToId)
        {
            ReadOnlyCollection<Tuple<string, PrivateMessage>> mailConvo = userManager.GetMailConven(user, mailToId);
            string friendsName = string.Empty;
            foreach (var m in mailConvo)
            {
                friendsName = m.Item1;
                Console.WriteLine("Name: {1} : {2}", m.Item1, m.Item2.Message);
            }
            System.Console.WriteLine("Press enter to continue, or start typing your message to reply to " + friendsName + ":");
            string answer = Console.ReadLine();
            if (answer == string.Empty)
            {
                System.Console.WriteLine("Back to menu");
            }
            else
            {
                userManager.SendMassage(answer, user, mailToId);
                System.Console.WriteLine("Message has been sent!");
            }
        }

        private static void PrintMyInbox(User user)
        {
            ReadOnlyCollection<Tuple<string, string, int>> myMail = userManager.GetUserMail(user); //1 username, 2 message, 3 id
            foreach (var mail in myMail)
            {
                Console.WriteLine("Id:{0} Name: {1} : {2}", mail.Item3, mail.Item1, mail.Item2);
            }
            Console.WriteLine();
            System.Console.Write("Press enter to continue, or enter an Id to reply to: ");
            string userInput = Console.ReadLine();
            bool success = Int32.TryParse(userInput, out int idChoice);
            if (userInput == string.Empty)
            {
                System.Console.WriteLine("Back to menu");
            }
            else if (success)
            {
                bool print = true;
                foreach (var mail in myMail)
                {
                    if (mail.Item3 == idChoice)
                    {
                        print = false;
                        System.Console.Write("Send a message to " + mail.Item1 + ": ");
                        string message = Console.ReadLine();
                        userManager.SendMassage(message, user, mail.Item3);
                        System.Console.WriteLine("Message has been sent");
                        break;
                    }
                }
                if (print == true)
                {
                    System.Console.WriteLine("This Id could not be found in your inbox!");
                }
            }
            else
            {
                Console.WriteLine("You did not write a number!");
            }
        }

        private static void PrintBiosOfFriends(User user)
        {
            IEnumerable<User> friends = userManager.GetFriendsBio(user);
            System.Console.WriteLine("These are your friends biographies!\n");
            foreach (var friend in friends)
            {
                System.Console.WriteLine("Username: " + friend.Username);
                System.Console.WriteLine("Fristname: " + friend.Firstname);
                System.Console.WriteLine("Lastname: " + friend.Lastname);
                System.Console.WriteLine("Bio: " + friend.Biography + "\n");
            }
            System.Console.WriteLine("Press any key to countinue..");
            userKey = Console.ReadKey(true).Key;
        }

        private static void PrintOthersTweets(User user)
        {
            ReadOnlyCollection<Tuple<string, Tweet>> tweets = tweetManager.GetOthersTweets(user);
            foreach (var tweet in tweets)
            {
                Console.WriteLine("{0}: {1}, {2}, {3}", tweet.Item2.ID, tweet.Item1, tweet.Item2.Message, tweet.Item2.CreateDate);
            }

            Console.WriteLine();
            System.Console.Write("Press enter to continue, or choose a TweetId to retweet: ");
            string userInput = Console.ReadLine();
            bool success = Int32.TryParse(userInput, out int idChoice);
            if (userInput == string.Empty)
            {
                System.Console.WriteLine("Back to menu");
            }
            else if (success)
            {
                bool print = false;
                foreach (var tweet in tweets)
                {
                    if (tweet.Item2.ID == idChoice)
                    {
                        print = true;
                        tweetManager.Retweet(user.Id, idChoice);
                        System.Console.WriteLine("This tweet can now be seen on your profile as a retweet!");
                        break;
                    }
                }
                if (print == false)
                {
                    System.Console.WriteLine("This TweetId is not available for you to retweet.");
                }
            }
            else
            {
                Console.WriteLine("You did not write a number!");
            }
        }

        private static void CreateUserMenu()
        {
            System.Console.WriteLine("Create a user");
            System.Console.Write("Write a username: ");
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
                System.Console.WriteLine("[1] Create a user");
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
                        System.Console.WriteLine("Incorrect menu input");
                        break;
                }
            }
        }

        public static void PrintTweets()
        {
            ReadOnlyCollection<Tuple<string, Tweet>> tweets = tweetManager.GetTweets();

            foreach (var tweet in tweets)
            {
                Console.WriteLine("{0}: {1}, {2}", tweet.Item1, tweet.Item2.Message, tweet.Item2.CreateDate);
            }
            Console.WriteLine();
        }

        private static void PrintYourBioAndTweets(User user)
        {
            System.Console.WriteLine("Bio: ");
            System.Console.WriteLine(user.Biography);
            Console.WriteLine();

            ReadOnlyCollection<Tuple<string, Tweet>> userTweets = tweetManager.GetUserTweets(user);
            Console.WriteLine("Tweets:");
            foreach (var tweet in userTweets)
            {
                Console.WriteLine("{0}: {1}, {2}, {3}", tweet.Item2.ID, tweet.Item1, tweet.Item2.Message, tweet.Item2.CreateDate);
            }
            System.Console.WriteLine("");


            System.Console.WriteLine("Press [Enter] to continue");
            System.Console.WriteLine("Type [R] to see your retweets");
            System.Console.Write("Or type and Id for a Tweet to delete it: ");
            string userInput = Console.ReadLine();
            bool success = Int32.TryParse(userInput, out int idChoice);
            if (userInput == string.Empty)
            {
                System.Console.WriteLine("Back to menu");
            }
            else if (userInput.ToLower() == "r")
            {
                RetweetMenue(user);
            }
            else if (success)
            {
                bool skip = false;
                foreach (var tweet in userTweets)
                {
                    if (tweet.Item2.ID != idChoice)
                    {
                        continue;
                    }
                    else if (tweet.Item2.ID == idChoice)
                    {
                        skip = true;
                        tweetManager.Delete(idChoice, user);
                        System.Console.WriteLine("Tweet deleted!");
                        break;
                    }

                }
                if (skip == false)
                {
                    System.Console.WriteLine("This TweetId does not exist or is not yours to delete!");
                }
            }
            else
            {
                Console.WriteLine("You did not write a number!");
            }
        }

        public static void SearchTweets(User loggedInUser)
        {
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

                    //IEnumerable<User> fetchedUsers = userManager.SearchUsers(searchString);
                    IEnumerable<User> fetchedUsers = userManager.SearchUsers(searchString, loggedInUser);
                    Console.WriteLine();
                    foreach (var x in fetchedUsers)
                    {
                        Console.WriteLine("- {0} ------- {1} {2}", x.Username, x.Firstname, x.Lastname);
                        Console.WriteLine("  Biography: {0}", x.Biography);
                    }

                    Console.WriteLine(Environment.NewLine + "[1] Follow/unfollow");
                    Console.WriteLine("[Any button] Return to search.");
                    Console.Write("Option: ");
                    userKey = Console.ReadKey(false).Key;

                    if (userKey == ConsoleKey.D1)
                    {
                        Console.WriteLine(Environment.NewLine);
                        /*for (int i = 0; i < fetchedUsers.Count(); i++)
                        {
                            fetchedUsers[i].
                            Console.WriteLine();
                        } */
                        foreach (var item in fetchedUsers)
                        {
                            Console.WriteLine($"{item.Id} Username: {item.Username}");
                        }
                        Console.Write(Environment.NewLine + "Choose an Id to follow: ");
                        int userKeyInt = Convert.ToInt32(Console.ReadLine());
                        var selectedUser = fetchedUsers.Where(u => u.Id == userKeyInt).FirstOrDefault();
                        Console.WriteLine("You follow " + "selectedUser.Username"
                            + "(Id: " + selectedUser.Id + ").");
                        userManager.AddFollwingOfUser(loggedInUser, selectedUser.Id);
                    }

                    else if (userKey == ConsoleKey.Escape)
                        break;
                }

                else if (userKey == ConsoleKey.D2)
                {
                    Console.Write(Environment.NewLine + "Search: ");
                    string searchString = Console.ReadLine();               // What tweet to search for.

                    ReadOnlyCollection<Tuple<string, Tweet>> fetchedTweets = tweetManager.SearchTweets(searchString);

                    Console.WriteLine();
                    foreach (var x in fetchedTweets)
                        Console.WriteLine("{0} : \"{1}\" {2}", x.Item1, x.Item2.Message, x.Item2.CreateDate);

                    Console.WriteLine(Environment.NewLine + "[1] Retweet");
                    Console.WriteLine("[Any button] Return to search.");
                    Console.Write("Option: ");
                    userKey = Console.ReadKey(false).Key;

                    if (userKey == ConsoleKey.D1)
                    {
                        for (int i = 0; i < fetchedTweets.Count; i++)
                        {
                            Console.WriteLine("[{0}] {1} : \"{2}\"", i, fetchedTweets[i].Item1, fetchedTweets[i].Item2.Message);
                        }
                        Console.Write(Environment.NewLine + "Choose an Id to retweet: ");
                        int userKeyInt = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine("You retweeted \"" + fetchedTweets[userKeyInt].Item2.Message + "\" (Tweet Id: " + fetchedTweets[userKeyInt].Item2.ID + ").");
                        tweetManager.Retweet(loggedInUser.Id, fetchedTweets[userKeyInt].Item2.ID);
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
            Console.WriteLine("Choose a menu option: ");
            Console.WriteLine("[1] Set/change your firstname");
            Console.WriteLine("[2] Set/change your lastname");
            Console.WriteLine("[3] Set/change your biography");

            userKey = Console.ReadKey(true).Key;

            switch (userKey)
            {
                case ConsoleKey.D1:
                    Console.WriteLine("Firstname: ");
                    string input = Console.ReadLine();
                    userManager.UpdateFirstnameUser(user, input);
                    break;
                case ConsoleKey.D2:
                    Console.WriteLine("Lastname: ");
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
            ReadOnlyCollection<Tuple<string, Tweet, UserToRetweet>> reTweets = tweetManager.GetRetweets(user);
            System.Console.WriteLine("My Retweets: ");
            foreach (var reTweet in reTweets)
            {
                Console.WriteLine("{0}: {1}, {2}, {3}", reTweet.Item3.Id, reTweet.Item1, reTweet.Item2.Message, reTweet.Item2.CreateDate);
            }
            System.Console.WriteLine("Press enter to continue");
            System.Console.Write("Or type the Id of a Tweet to delete it: ");
            string userInput = Console.ReadLine();
            bool success = Int32.TryParse(userInput, out int idChoice);
            if (userInput == string.Empty)
            {
                System.Console.WriteLine("Back to menu");
            }
            else if (success)
            {
                bool skip = false;
                foreach (var tweet in reTweets)
                {
                    if (tweet.Item3.Id != idChoice)
                    {
                        continue;
                    }
                    else if (tweet.Item3.Id == idChoice)
                    {
                        skip = true;
                        tweetManager.DeleteReTweet(idChoice);
                        System.Console.WriteLine("Retweet deleted!");
                        System.Console.WriteLine("");
                        break;
                    }
                }
                if (skip == false)
                {
                    System.Console.WriteLine("This TweetId does not exist or is not yours to delete!");
                }
            }
            else
            {
                Console.WriteLine("You did not write a number!");
            }
        }
    }
}
