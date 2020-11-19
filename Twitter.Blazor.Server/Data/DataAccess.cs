
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
        public IEnumerable<Tweet> TopTweets { get; private set; }
        public IEnumerable<Tweet> UrerTweets { get; private set; }

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
           List<Tweet> tweets = TopTweets.ToList();
            tweets.RemoveAll(x => x.ID == tweet.ID);
            TopTweets = tweets;

            tweets = UrerTweets.ToList();
            tweets.RemoveAll(x => x.ID == tweet.ID);
            UrerTweets = tweets;
            Loading = false;
        }

        public async void OnSyncTweet(object source, ElapsedEventArgs e)
        {
            await Task.Run(() =>
            {
                TweetManager tweetManager = new TweetManager();
                IEnumerable<Tweet> Tweets = tweetManager.GetTweets();
                IEnumerable<Tweet> UserTweets = new List<Tweet>();
                if (User != null)
                {
                    UserTweets = tweetManager.GetUserTweets(User);
                }

                HashSet<int> TopComper = new HashSet<int>(Tweets.Select(x => x.ID));
                HashSet<int> UserComper = new HashSet<int>(UserTweets.Select(x => x.ID));
                if (!TopComper.SetEquals(TopTweets.Select(x => x.ID)))
                {
                    TopTweets = Tweets;
                    NotifyDataChanged.Invoke();
                }
                if (UrerTweets != null && !UserComper.SetEquals(UrerTweets.Select(x => x.ID)))
                {
                    UrerTweets = UserTweets;
                    NotifyDataChanged.Invoke();
                }
            });
        }


    }
}
