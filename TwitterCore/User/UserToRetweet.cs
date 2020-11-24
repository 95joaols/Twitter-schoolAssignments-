namespace TwitterCore
{
    public class UserToRetweet
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int TweetId { get; set; }

        public UserToRetweet(int userId, int tweetId)
        {
            UserId = userId;
            TweetId = tweetId;
        }
        public UserToRetweet() { }

    }
}