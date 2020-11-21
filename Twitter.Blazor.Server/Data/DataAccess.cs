
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
        public IEnumerable<Tuple<string, Tweet>> Tweets { get; private set; }

        public IEnumerable<User> UrersSearch { get; private set; }

        public TweetTyp TweetType
        {
            get { return tweetType; }
            set
            {
                tweetType = value;
                OnSyncTweet(null, null);
                NotifyDataChanged.Invoke();
            }
        }
        public TweetTyp tweetType;

        public User User { get; set; }

        public bool Runing { get; private set; }

        public string Searching { get; set; }

        public bool Loading
        {
            get { return loading; }
            set
            {
                loading = value;
                NotifyDataChanged.Invoke();
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
                switch (TweetType)
                {
                    case TweetTyp.Top:
                        Tweets = tweetManager.GetTweets();
                        break;
                    case TweetTyp.User:
                        if (User != null)
                        {
                            Tweets = tweetManager.GetUserTweets(User);
                        }
                        break;
                    case TweetTyp.Search:
                        Tweets = tweetManager.SearchTweets(Searching);
                        break;
                    default:
                        break;
                }


                Timer timer = new Timer(5000);
                timer.Elapsed += OnSyncTweet;
                timer.AutoReset = true;
                timer.Enabled = true;

                Runing = true;
            }
        }
        public void Update()
        {
            NotifyDataChanged.Invoke();
        }

        public bool LogingIn(string username, string password)
        {
            LoginSystem loginSystem = new LoginSystem();
            (bool, User) UserReturn = loginSystem.LogInUser(username, password);
            if (UserReturn.Item1)
            {
                User = UserReturn.Item2;
                LoggedIn.Invoke();
                NotifyDataChanged.Invoke();
            }
            return UserReturn.Item1;
        }

        public void LogingOut()
        {
            User = null;
            LoggedOut.Invoke();
            NotifyDataChanged.Invoke();
        }
        public void RemoveTweet(Tweet tweet)
        {
            TweetManager tweetManager = new TweetManager();
            tweetManager.Delete(tweet.ID, User);
            List<Tuple<string, Tweet>> tweets = Tweets.ToList();
            tweets.RemoveAll(x => x.Item2.ID == tweet.ID);
            Tweets = tweets;
            Loading = false;
        }

        public async void OnSyncTweet(object source, ElapsedEventArgs e)
        {
            await Task.Run(() =>
            {
                try
                {
                    TweetManager tweetManager = new TweetManager();
                    IEnumerable<Tuple<string, Tweet>> NewTweets = new List<Tuple<string, Tweet>>();
                    switch (TweetType)
                    {
                        case TweetTyp.Top:
                            NewTweets = tweetManager.GetTweets();
                            break;
                        case TweetTyp.User:
                            if (User != null)
                            {
                                NewTweets = tweetManager.GetUserTweets(User);
                            }
                            break;
                        case TweetTyp.Search:
                            NewTweets = tweetManager.SearchTweets(Searching);
                            break;
                        default:
                            break;
                    }
                    HashSet<int> TopComper = new HashSet<int>(Tweets.Select(x => x.Item2.ID));
                    if (!TopComper.SetEquals(NewTweets.Select(x => x.Item2.ID)))
                    {
                        Tweets = NewTweets;
                        NotifyDataChanged.Invoke();
                    }
                }
                catch (Exception)
                {
                }

            });
        }


    }
}
