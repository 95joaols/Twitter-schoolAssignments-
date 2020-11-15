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
        User User { get;}
        bool Runing { get; }

        void Start();
        void OnSyncTweet(object source, ElapsedEventArgs e);

        bool LogingIn(string username, string password);
        void LogingOut();

        event Func<Task> NotifyDataChanged;
    }
}
