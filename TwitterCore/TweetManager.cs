using System.Collections.Generic;
using System;

namespace TwitterCore
{
    // den skulle inte kunna heta UserService?
    public class TweetManager
    {
        SQLConnection db = new SQLConnection("Server=40.85.84.155;Database=OOPGroup1;User=Student11;Password=zombie-virus@2020;");

        public TweetManager()
        {
        }
        public void CreateTweet(string message, int UserID)
        {
            Tweet tweet = new Tweet(message, UserID);
            db.AddTweetToDb(tweet);
            // dbControl.Add<int>(tweet, Table.Tweet, "ID", new List<string>() { "CreateDate", "Username", "isRetweet", "retweetCount", "Retweet" });
        }

        public List<Tuple<string, Tweet>> GetUserTweets(User user)
        {
            return db.GetUserTweetsFromDb(user.Id);
            
        }

        public List<Tuple<string,Tweet>> GetTweets()
        {
            return db.GetTweetsFromDb();
        }

        public List<Tuple<string, Tweet, UserToRetweet>> GetRetweets(User user)
        {
            return db.GetUserRetweetsFromDb(user.Id);
        }
        public List<Tuple<string, Tweet>> GetOthersTweets(User user)
        {
           return db.GetOthersTweetsFromDb(user.Id);  
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

        public void DeleteReTweet(int reTweetId)
        {
            db.DeleteReTweetDb(reTweetId);
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
