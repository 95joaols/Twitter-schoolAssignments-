using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;
using TwitterCore;

namespace Twitter.Blazor.Server.Data
{
    interface IDataAccess
    {
        IEnumerable<Tweet> TopTweets { get;}
        IEnumerable<Tweet> UrerTweets { get; }
        User User { get; set; }
        bool Runing { get; }
        void Update();

        void Start();

        bool LogingIn(string username, string password);
        void LogingOut();

        event Func<Task> NotifyDataChanged;
        event Func<Task> LoggedIn;
        event Func<Task> LoggedOut;
    }
}
