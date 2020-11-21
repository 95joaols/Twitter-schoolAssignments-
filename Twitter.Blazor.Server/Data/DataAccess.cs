﻿
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

        public User User { get; set; }

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
                        UserSearch = new UserManager().SearchUsers(Searching);
                        break;
                    default:
                        break;
                }


                Timer timer = new Timer(5000);
                timer.Elapsed += OnSync;
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

        public async void OnSync(object source, ElapsedEventArgs e)
        {
            await Task.Run(() =>
            {
                try
                {
                    TweetManager tweetManager = new TweetManager();
                    IEnumerable<Tuple<string, Tweet>> NewTweets = new List<Tuple<string, Tweet>>();
                    IEnumerable<User> NewUser = new List<User>();

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
                            NewUser = new UserManager().SearchUsers(Searching);
                            break;
                        default:
                            break;
                    }
                    HashSet<int> tweetComper = new HashSet<int>(Tweets.Select(x => x.Item2.ID));
                    HashSet<int> userComper = new HashSet<int>(UserSearch.Select(x => x.Id));
                    if (!tweetComper.SetEquals(NewTweets.Select(x => x.Item2.ID)) || !userComper.SetEquals(NewUser.Select(x => x.Id)))
                    {
                        Tweets = NewTweets;
                        UserSearch = NewUser;
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
