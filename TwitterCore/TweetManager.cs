using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;

namespace TwitterCore
{
    // den skulle inte kunna heta UserService?
    public class TweetManager
    {
        SQLConnection db = new SQLConnection();

        public TweetManager()
        {
        }
        public void CreateTweet(string message, int UserID)
        {
            Tweet tweet = new Tweet(message, UserID);
            db.AddTweetToDb(tweet);
            // dbControl.Add<int>(tweet, Table.Tweet, "ID", new List<string>() { "CreateDate", "Username", "isRetweet", "retweetCount", "Retweet" });
        }

        public ReadOnlyCollection<Tuple<string, Tweet>> GetUserTweets(User user)
        {
            List < Tuple<string, Tweet> > foo = db.GetUserTweetsFromDb(user.Id);
            return new ReadOnlyCollection<Tuple<string, Tweet>>(foo);
        }

        public ReadOnlyCollection<Tuple<string, Tweet>> GetTweets()
        {
            List < Tuple<string, Tweet> > foo = db.GetTweetsFromDb();
            return new ReadOnlyCollection<Tuple<string, Tweet>>(foo);
        }

        public ReadOnlyCollection<Tuple<string, Tweet, UserToRetweet>> GetRetweets(User user)
        {
            List < Tuple<string, Tweet, UserToRetweet> > foo = db.GetUserRetweetsFromDb(user.Id);
            return new ReadOnlyCollection<Tuple<string, Tweet, UserToRetweet>>(foo);
        }
        public ReadOnlyCollection<Tuple<string, Tweet>> GetOthersTweets(User user)
        {
            List<Tuple<string, Tweet>> foo = db.GetOthersTweetsFromDb(user.Id);  
            return new ReadOnlyCollection<Tuple<string, Tweet>>(foo);
        }

        public void Retweet(int userId, int tweetId)
        {
            UserToRetweet retweet = new UserToRetweet(userId, tweetId);
            db.RetweetToDb(retweet);
        }

        public void Delete(int tweetId, User user)
        {
            db.DeleteTweetDb(tweetId);
        }

        public void DeleteReTweet(int reTweetId)
        {
            db.DeleteReTweetDb(reTweetId);
        }

        public ReadOnlyCollection<Tuple<string, Tweet>> SearchTweets(string search)
        {
            List < Tuple<string, Tweet> > foo = db.SearchTweets(search);
            return new ReadOnlyCollection<Tuple<string, Tweet>>(foo);
        }
    }
}
