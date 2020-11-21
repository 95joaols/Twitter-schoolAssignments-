using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TwitterCore;

namespace Twitter.Blazor.Server.Data
{
    interface IDataAccess
    {
        IEnumerable<Tuple<string, Tweet>> Tweets { get; }
        IEnumerable<User> UrersSearch { get; }
        TweetTyp TweetType { get; set; }
        User User { get; set; }
        bool Runing { get; }
        bool Loading { get; set; }

        string Searching { get; set; }


        void Update();
        void RemoveTweet(Tweet tweet);

        void Start();

        bool LogingIn(string username, string password);
        void LogingOut();

        event Func<Task> NotifyDataChanged;
        event Func<Task> LoggedIn;
        event Func<Task> LoggedOut;
    }
}
