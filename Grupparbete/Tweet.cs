using System;

namespace Grupparbete
{
    public class Tweet
    {
        public int ID { get; set; }
        public string Message { get; set; }
        public int UserID { get; set; }
        public string Username { get; set; }
        public DateTime CreateDate { get; set; }
        public int isRetweet { get; set; }
        public int retweetCount { get; set; }

        public Tweet()
        {
        }
        public Tweet(string message, int UserId)
        {
            Message = message;
            UserID = UserId;
        }
    }
}
