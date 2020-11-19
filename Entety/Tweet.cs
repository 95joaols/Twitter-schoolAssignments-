using System;
using System.Collections.Generic;

#nullable disable

namespace Entety
{
    public partial class Tweet
    {
        public Tweet()
        {
            UserToRetweets = new HashSet<UserToRetweet>();
        }

        public int Id { get; set; }
        public DateTime? CreateDate { get; set; }
        public int UserId { get; set; }
        public string Message { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<UserToRetweet> UserToRetweets { get; set; }
    }
}
