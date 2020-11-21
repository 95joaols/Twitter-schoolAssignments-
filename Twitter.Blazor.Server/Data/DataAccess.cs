
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
        public IEnumerable<Tuple<string, Tweet>> TopTweets { get; private set; }
        public IEnumerable<Tuple<string, Tweet>> UrerTweets { get; private set; }

        public User User { get; set; }

        public bool Runing { get; private set; }
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
            Start();
        }
        public void Start()
        {
            if (!Runing)
            {
                TweetManager tweetManager = new TweetManager();
                TopTweets = tweetManager.GetTweets();
                if (User != null)
                {
                    UrerTweets = tweetManager.GetUserTweets(User);
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

                TweetManager tweetManager = new TweetManager();
                UrerTweets = tweetManager.GetUserTweets(User);
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
            List<Tuple<string, Tweet>> tweets = TopTweets.ToList();
            tweets.RemoveAll(x => x.Item2.ID == tweet.ID);
            TopTweets = tweets;

            tweets = UrerTweets.ToList();
            tweets.RemoveAll(x => x.Item2.ID == tweet.ID);
            UrerTweets = tweets;
            Loading = false;
        }

        public async void OnSyncTweet(object source, ElapsedEventArgs e)
        {
            await Task.Run(() =>
            {
                try
                {
                    TweetManager tweetManager = new TweetManager();
                    IEnumerable<Tuple<string, Tweet>> Tweets = tweetManager.GetTweets();
                    IEnumerable<Tuple<string, Tweet>> UserTweets = new List<Tuple<string, Tweet>>();
                    if (User != null)
                    {
                        UserTweets = tweetManager.GetUserTweets(User);
                    }
                    HashSet<int> TopComper = new HashSet<int>(Tweets.Select(x => x.Item2.ID));
                    HashSet<int> UserComper = new HashSet<int>(UserTweets.Select(x => x.Item2.ID));
                    if (!TopComper.SetEquals(TopTweets.Select(x => x.Item2.ID)))
                    {
                        TopTweets = Tweets;
                        NotifyDataChanged.Invoke();
                    }
                    if (UrerTweets != null && !UserComper.SetEquals(UrerTweets.Select(x => x.Item2.ID)))
                    {
                        UrerTweets = UserTweets;
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
