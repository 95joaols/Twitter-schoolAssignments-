
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using TwitterCore;


namespace Twitter.Blazor.Server.Data
{
    public class DataAccess : IDataAccess
    {
        public IEnumerable<Tuple<string, Tweet>> Tweets { get; private set; } = new List<Tuple<string, Tweet>>();

        public IEnumerable<User> UserSearch { get; private set; } = new List<User>();

        public IEnumerable<Tuple<string, string, int>> Messages { get; private set; } = new List<Tuple<string, string, int>>();

        public TweetTyp TweetType
        {
            get { return tweetType; }
            set
            {
                tweetType = value;
                OnSync(null, null);
                NotifyDataChanged?.Invoke();
            }
        }
        private TweetTyp tweetType;

        public User LoginUser { get; } = new LoginSystem()._user;

        public User UserCheck { get; set; }

        public bool Runing { get; private set; }

        public string Searching
        {
            get { return searching; }
            set
            {
                searching = value;
                OnSync(null, null);
                NotifyDataChanged?.Invoke();
            }
        }
        private string searching;
        public bool Loading
        {
            get { return loading; }
            set
            {
                loading = value;
                NotifyDataChanged?.Invoke();
            }
        }
        private bool loading;

        public event Func<Task> NotifyDataChanged;
        public event Func<Task> LoggedIn;
        public event Func<Task> LoggedOut;

        public DataAccess()
        {
            Searching = "";
            Start();
        }
        public void Start()
        {
            if (!Runing)
            {
                TweetManager tweetManager = new TweetManager();
                UserManager userManager = new UserManager();
                switch (TweetType)
                {
                    case TweetTyp.Top:
                        try
                        {
                            Tweets = tweetManager.GetTweets();
                        }
                        catch (Exception)
                        {
                        }
                        break;
                    case TweetTyp.User:
                        if (UserCheck != null)
                        {
                            try
                            {
                                Tweets = tweetManager.GetUserTweets(UserCheck);
                            }
                            catch (Exception)
                            {
                            }
                        }
                        break;
                    case TweetTyp.Search:
                        try
                        {
                            Tweets = tweetManager.SearchTweets(Searching);

                        }
                        catch (Exception)
                        {
                        }
                        try
                        {
                            UserSearch = userManager.SearchUsers(Searching, LoginUser);

                        }
                        catch (Exception)
                        {
                        }
                        break;
                    default:
                        break;
                }
                if (LoginUser != null)
                {
                    try
                    {
                        Messages = userManager.GetUserMail(LoginUser);
                    }
                    catch (Exception)
                    {
                    }
                }


                Timer timer = new Timer(5000);
                timer.Elapsed += OnSync;
                timer.AutoReset = true;
                timer.Enabled = true;

                Runing = true;
            }
        }

        public User GetUser(int id)
        {
            return new UserManager().SINGLEUSER(id).First();
        }

        public void RegistrarUser(string userName,string password)
        {
            new LoginSystem().CreateUser(userName, password);
        }
        public void Update()
        {
            NotifyDataChanged.Invoke();
        }

        public bool LogingIn(string username, string password)
        {
            LoginSystem loginSystem = new LoginSystem();
            (bool, User) UserReturn;
            try
            {
                UserReturn = loginSystem.LogInUser(username, password);
            }
            catch
            {
                throw;
            }
            if (UserReturn.Item1)
            {
                //LoginUser = UserReturn.Item2;
                LoggedIn.Invoke();
                NotifyDataChanged.Invoke();
            }
            return UserReturn.Item1;
        }

        public void LogingOut()
        {
            //LoginUser = null;
            LoggedOut.Invoke();
            NotifyDataChanged.Invoke();
        }
        public void RemoveTweet(Tweet tweet)
        {
            TweetManager tweetManager = new TweetManager();
            try
            {
                tweetManager.Delete(tweet.ID, LoginUser);
            }
            catch (Exception)
            {

                return;
            }
            List<Tuple<string, Tweet>> tweets = Tweets.ToList();
            tweets.RemoveAll(x => x.Item2.ID == tweet.ID);
            Tweets = tweets;
            Loading = false;
        }

        public async void OnSync(object source, ElapsedEventArgs e)
        {
            await Task.Run(() =>
            {
                try
                {
                    TweetManager tweetManager = new TweetManager();
                    UserManager userManager = new UserManager();
                    IEnumerable<Tuple<string, Tweet>> NewTweets = new List<Tuple<string, Tweet>>();
                    IEnumerable<User> NewUser = new List<User>();
                    IEnumerable<Tuple<string, string, int>> newMessages = new List<Tuple<string, string, int>>();

                    switch (TweetType)
                    {
                        case TweetTyp.Top:
                            NewTweets = tweetManager.GetTweets();
                            break;
                        case TweetTyp.User:
                            if (UserCheck != null)
                            {
                                NewTweets = tweetManager.GetUserTweets(UserCheck);
                            }
                            break;
                        case TweetTyp.Search:
                            NewTweets = tweetManager.SearchTweets(Searching);
                            NewUser = new UserManager().SearchUsers(Searching, LoginUser);
                            break;
                        default:
                            break;
                    }
                    if (LoginUser != null)
                    {
                        newMessages = userManager.GetUserMail(LoginUser);
                    }

                    HashSet<int> tweetComper = new HashSet<int>(Tweets.Select(x => x.Item2.ID));
                    HashSet<int> userComper = new HashSet<int>(UserSearch.Select(x => x.Id));
                    HashSet<int> messageComper = new HashSet<int>(Messages.Select(x => x.Item3));
                    if (!tweetComper.SetEquals(NewTweets.Select(x => x.Item2.ID)) || !userComper.SetEquals(NewUser.Select(x => x.Id)) || !messageComper.SetEquals(newMessages.Select(x => x.Item3)))
                    {
                        Tweets = NewTweets;
                        UserSearch = NewUser;
                        Messages = newMessages;
                        NotifyDataChanged?.Invoke();
                    }
                }
                catch (Exception)
                {
                }

            });
        }


    }
}
