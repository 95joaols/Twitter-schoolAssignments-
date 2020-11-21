using System;

#nullable disable

namespace Entety
{
    public partial class UsersAndTweetsView
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Biography { get; set; }
        public int? IdTweet { get; set; }
        public string Message { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}
