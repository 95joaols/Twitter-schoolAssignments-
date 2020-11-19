using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;
using TwitterCore;

namespace Twitter.Blazor.Server.Data
{
    interface IDataAccess
    {
        IEnumerable<Tuple<string, Tweet>> TopTweets { get; }
        IEnumerable<Tuple<string, Tweet>> UrerTweets { get; }
        User User { get; set; }
        bool Runing { get; }
        bool Loading { get; set; }

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
