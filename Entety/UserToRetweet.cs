﻿using System;
using System.Collections.Generic;

#nullable disable

namespace Entety
{
    public partial class UserToRetweet
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int TweetId { get; set; }

        public virtual Tweet Tweet { get; set; }
        public virtual User User { get; set; }
    }
}