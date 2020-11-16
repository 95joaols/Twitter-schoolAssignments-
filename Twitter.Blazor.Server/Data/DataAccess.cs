
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

        public User User { get; set; }

        public bool Runing { get; private set; }

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
                TopTweets = tweetManager.GetTweets(50);

                Timer timer = new Timer(5000);
                timer.Elapsed += OnSyncTweet;
                timer.AutoReset = true;
                timer.Enabled = true;

                Runing = true;
            }
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

        public async void OnSyncTweet(object source, ElapsedEventArgs e)
        {
            await Task.Run(() =>
            {
                TweetManager tweetManager = new TweetManager();
                IEnumerable<Tweet> Tweets = tweetManager.GetTweets(50);

                HashSet<int> set = new HashSet<int>(Tweets.Select(x => x.ID));
                if (!set.SetEquals(TopTweets.Select(x => x.ID)))
                {
                    TopTweets = Tweets;
                    NotifyDataChanged.Invoke();
                }
            });
        }


    }
}
