using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwitterCore;

namespace Twitter.Blazor.Server.Pages
{
    public partial class TweetsOverview
    {
        public IEnumerable<Tweet> Tweets { get; set; }

        protected override void OnInitialized()
        {
            TweetManager tweetManager = new TweetManager();
            Tweets = tweetManager.GetTweets(50);
        }
    }
}
