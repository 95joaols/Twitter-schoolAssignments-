using System;
using System.ComponentModel;

namespace Repository.Enums
{
    public enum Table
    {
        User,
        Tweet,
        [Description("Tweet INNER JOIN [User] on Tweet.UserId = [User].Id")]
        TweetUser
    }
}
