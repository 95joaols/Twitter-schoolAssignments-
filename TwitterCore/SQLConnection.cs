using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Dapper;
using TwitterCore;

namespace TwitterCore
{

    class SQLConnection
    {
        private readonly string connectionString;

        public SQLConnection(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public IEnumerable<User> GetUsers()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                return connection.Query<User>("SELECT Id, Username, Password, Biography FROM [User]");
            }
        }

        public IEnumerable<Tweet> GetTweetsFromDb()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                return connection.Query<Tweet>("SELECT TOP 10 CreateDate, Message, Username FROM Tweet INNER JOIN [User] on Tweet.UserId = [User].Id ORDER BY CreateDate DESC");
            }
        }

        public IEnumerable<Tweet> GetUserTweetsFromDb(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                return connection.Query<Tweet>("SELECT Tweet.Id, Tweet.UserId, CreateDate, Message, Username FROM Tweet INNER JOIN [User] on Tweet.UserId = [User].Id WHERE [User].Id =" + id + "ORDER BY CreateDate DESC");
            }
        }

        public void AddUserToDb(User user)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Execute("INSERT INTO [User] (Username, Password) values (@username, @password)", user);
            }
        }

        public IEnumerable<Tweet> GetOthersTweetsFromDb(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                return connection.Query<Tweet>("SELECT Tweet.Id, CreateDate, Message, Username FROM Tweet INNER JOIN [User] on Tweet.UserId = [User].Id WHERE [User].Id !=" + id + "ORDER BY CreateDate DESC");
            }
        }

        internal void DeleteTweetDb(int tweetId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Execute("delete from Tweet where Id = " + tweetId);
            }
        }

        internal void RetweetToDb(UserToRetweet retweet)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Execute("INSERT INTO UserToRetweet (UserId, TweetId) values (@userId, @tweetId)", retweet);
            }
        }

        public void AddTweetToDb(Tweet tweet)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Execute("INSERT INTO Tweet (UserId, Message) values (@userId, @message)", tweet);
            }
        }

        public void UpdateBioToUserInDb(User user)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Execute($"UPDATE [User] SET Biography = @Biography WHERE Id = @Id;", user);
            }
        }

        public void AddUserFollowingToDb(UserToUser userToUser)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Execute("INSERT INTO UserToUser (UserId,FollowingId) values (@UserId, @FollowingId)", userToUser);
            }
        }

        public IEnumerable<User> SearchUsers(string search)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                return connection.Query<User>("EXEC SearchProcedureUsers @SearchString = @Search", new { @Search = search });
            }
        }

        public IEnumerable<Tweet> SearchTweets(string search)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                return connection.Query<Tweet>("EXEC SearchProcedureTweets @SearchString = @Search", new { @Search = search });
            }
        }
    }
}
