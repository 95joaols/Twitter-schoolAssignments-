using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwitterCore;

namespace Twitter.Blazor.Server.Pages
{
    public partial class Index
    {
        public IEnumerable<Tweet> Tweets { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await Task.Run(() =>
            {
                TweetManager tweetManager = new TweetManager();
                Tweets = tweetManager.GetTweets(50);
                Tweets = Tweets.OrderByDescending(tweet => tweet.CreateDate);
            });
            return;
        }
    }
}