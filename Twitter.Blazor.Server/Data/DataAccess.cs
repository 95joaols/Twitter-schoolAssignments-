using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components;
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
        [Inject]
        ISessionStorageService SessionStorage { get; set; }

        public IEnumerable<Tweet> TopTweets { get; private set; }

        public User User { get; private set; }

        public bool Runing { get; private set; }

        public event Func<Task> NotifyDataChanged;

        public DataAccess()
        {

            Start();
            User = SessionStorage?.GetItemAsync<User>("CurentUser").Result;


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
                //NotifyDataChanged.Invoke();
            }
            SessionStorage?.SetItemAsync("CurentUser", User).Wait();
            return UserReturn.Item1;
        }

        public void LogingOut()
        {
            User = null;
            //NotifyDataChanged.Invoke();
        }

        public void OnSyncTweet(object source, ElapsedEventArgs e)
        {
            TweetManager tweetManager = new TweetManager();
            IEnumerable<Tweet> Tweets = tweetManager.GetTweets(50);
            Tweets = Tweets.OrderByDescending(tweet => tweet.CreateDate);

            var set = new HashSet<Tweet>(Tweets);
            if (!set.SetEquals(TopTweets))
            {
                TopTweets = Tweets;
                //NotifyDataChanged.Invoke();
            }
        }


    }
}
