using System;
using System.Collections.Generic;

#nullable disable

namespace Entety
{
    public partial class User
    {
        public User()
        {
            Tweets = new HashSet<Tweet>();
            UserToRetweets = new HashSet<UserToRetweet>();
            UserToUserFollowings = new HashSet<UserToUser>();
            UserToUserUsers = new HashSet<UserToUser>();
        }

        public int Id { get; set; }
        public string Username { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Password { get; set; }
        public string Biography { get; set; }

        public virtual ICollection<Tweet> Tweets { get; set; }
        public virtual ICollection<UserToRetweet> UserToRetweets { get; set; }
        public virtual ICollection<UserToUser> UserToUserFollowings { get; set; }
        public virtual ICollection<UserToUser> UserToUserUsers { get; set; }
    }
}
