using System.Collections.Generic;

namespace Grupparbete
{
    // den skulle inte kunna heta UserService?
    public class TweetManager
    {
        SQLConnection db = new SQLConnection("Server=40.85.84.155;Database=OOPGroup1;User=Student11;Password=zombie-virus@2020;");

        public void CreateTweet(string message, int UserID)
        {
            Tweet tweet = new Tweet(message, UserID);
            // add user to DB
            //db.CreateUserToDB() typ
            db.AddTweetToDb(tweet);
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

        public void AddBioToUser(string bio, User user)
        {
            user.Biography = bio;
            db.UpdateBioToUserInDb(user);
        }

    }
}
