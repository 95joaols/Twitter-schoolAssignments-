using System.Collections.Generic;
using Repository;
using Repository.Enums;
using System;

namespace TwitterCore
{
    // den skulle inte kunna heta UserService?
    public class TweetManager
    {
        SQLConnection db = new SQLConnection("Server=40.85.84.155;Database=OOPGroup1;User=Student11;Password=zombie-virus@2020;");

        IFullControl dbControl;

        public TweetManager()
        {
            dbControl = Faktorio.GetRepository();
        }
        public void CreateTweet(string message, int UserID)
        {
            Tweet tweet = new Tweet(message, UserID);
            db.AddTweetToDb(tweet);
            // dbControl.Add<int>(tweet, Table.Tweet, "ID", new List<string>() { "CreateDate", "Username", "isRetweet", "retweetCount", "Retweet" });
        }

        public IEnumerable<Tweet> GetUserTweets(User user)
        {
            IEnumerable<Tweet> userTweets = db.GetUserTweetsFromDb(user.Id);
            return userTweets;
        }

        public List<Tweet> GetTweets()
        {

            List<Tweet> tweets = new List<Tweet>();

            IEnumerable<Tweet> tweetsable = db.GetTweetsFromDb();

            foreach (var tweet in tweetsable)
            {
                tweets.Add(tweet);
            }
            return tweets;

        }

        public List<Tweet> GetOthersTweets(User user)
        {
            List<Tweet> tweets = new List<Tweet>();
            IEnumerable<Tweet> othersTweets = db.GetOthersTweetsFromDb(user.Id);

            foreach (Tweet tweet in othersTweets)
            {
                tweets.Add(tweet);
            }
            return tweets;
        }

        public void Retweet(int userId, int tweetId)
        {
            UserToRetweet retweet = new UserToRetweet(userId, tweetId);
            db.RetweetToDb(retweet);
        }

        public void Delete(int tweetId, User user)
        {
            db.DeleteTweetDb(tweetId);

            // Dictionary<string, string> where = new Dictionary<string, string>();
            // where.Add("ID", tweetId.ToString());

            // Tweet tweet = dbControl.GetSingel<Tweet>("*", Table.Tweet, where);
            // if (tweet != null && tweet.UserID == user.Id)
            // {
            //     return dbControl.Delete(Table.Tweet, "ID", tweetId);
            // }
            // else
            // {
            //     return false;
            // }
        }

        public IEnumerable<User> SearchUsers(string search)
        {


            //                if (!String.IsNullOrWhiteSpace(search))           // TODO: Add later when we don't need to debug any longer.
            return db.SearchUsers(search);
            //                else
            //                    return null;
        }

        public IEnumerable<Tweet> SearchTweets(string search)
        {


            //                if (!String.IsNullOrWhiteSpace(search))           // TODO: Add later when we don't need to debug any longer.
            return db.SearchTweets(search);
            //                else
            //                    return null;
        }

        // public void AddBioToUser(string bio, User user)
        // {
        //     user.Biography = bio;
        //     //db.UpdateBioToUserInDb(user);
        //     dbControl.Update<User>(user, Table.User, "Id", user.Id, new List<string>() { "Firstname", "Lastname", "IsLoggedIn" });
        // }

    }
}
