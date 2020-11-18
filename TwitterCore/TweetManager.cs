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

        public List<Tweet> GetTweets(int top)
        {

            List<Tweet> tweets = new List<Tweet>();

            // IEnumerable<Tweet> tweetsable = db.GetTweetsFromDb();
            IEnumerable<Tweet> tweetsable = dbControl.GetMenyEntitys<Tweet>(top, "Tweet.ID, CreateDate, Message, Username", Table.TweetUser, null, "CreateDate");

            foreach (var tweet in tweetsable)
            {
                tweets.Add(tweet);
            }
            return tweets;

        }

        public bool Delete(int tweetId, User user)
        {
            Dictionary<string, string> where = new Dictionary<string, string>();
            where.Add("ID", tweetId.ToString());

            Tweet tweet = dbControl.GetSingel<Tweet>("*", Table.Tweet, where);
            if (tweet != null && tweet.UserID == user.Id)
            {
                return dbControl.Delete(Table.Tweet, "ID", tweetId);
            }
            else
            {
                return false;
            }
        }

        public IEnumerable<Search> SearchUsersAndTweets(string search)         // TODO: Old garbage! Remove later.
        {


            //                if (!String.IsNullOrWhiteSpace(search))           // TODO: Want to check this! The problem is that something has to be returned; would like to return an empty IEnumerable<Search>. At least it's easier to debug right now.. :)
            return db.SearchUsersAndTweets(search);
            //                else
            //                    return null;
        }

        public IEnumerable<User> SearchUsers(string search)
        {


            //                if (!String.IsNullOrWhiteSpace(search))           // TODO: Want to check this! The problem is that something has to be returned; would like to return an empty IEnumerable<Search>. At least it's easier to debug right now.. :)
            return db.SearchUsers(search);
            //                else
            //                    return null;
        }

        public IEnumerable<Tweet> SearchTweets(string search)
        {


            //                if (!String.IsNullOrWhiteSpace(search))           // TODO: Want to check this! The problem is that something has to be returned; would like to return an empty IEnumerable<Search>. At least it's easier to debug right now.. :)
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
