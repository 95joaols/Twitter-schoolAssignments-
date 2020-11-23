using System;

namespace ConsoleGUI
{
    public class Tweet
    {
        public int ID { get; set; }
        public string Message { get; set; }
        public int UserID { get; set; }
        public DateTime CreateDate { get; set; }

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
