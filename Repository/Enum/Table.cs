using System.ComponentModel;

namespace Repository.Enums
{
    public enum Table
    {
        [Description("[User]")]
        User,
        Tweet,
        [Description("Tweet INNER JOIN [User] on Tweet.UserId = [User].Id")]
        TweetUser,
        [Description("INNER JOIN [UserToUser] On UserToUser.FollowingId = [User].id INNER JOIN  [User] AS [Following] ON [Following].Id = UserToUser.UserId")]
        UserToUser
    }
}
