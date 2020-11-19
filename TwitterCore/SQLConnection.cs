using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Dapper;

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

        public List<Tuple<string, Tweet>> GetTweetsFromDb()
        {
            List<Tuple<string, Tweet>> tweetsFromDb = new List<Tuple<string, Tweet>>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                var foo = connection.Query("SELECT TOP 10 t.Id, t.CreateDate, t.Message, u.Username FROM Tweet as t INNER JOIN [User] as u on t.UserId = u.Id ORDER BY CreateDate DESC");
                foreach (var item in foo)
                {
                    tweetsFromDb.Add(new Tuple<string, Tweet>(
                        (string)item.Username,
                        new Tweet {ID = item.Id, CreateDate = (DateTime)item.CreateDate, Message = item.Message }));
                }
            }
            return tweetsFromDb;
        }

        public List<Tuple<string, Tweet>> GetUserTweetsFromDb(int id)
        {
            List<Tuple<string, Tweet>> tweetsFromDb = new List<Tuple<string, Tweet>>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                dynamic foo = connection.Query("SELECT Tweet.Id, Tweet.UserId, CreateDate, Message, Username FROM Tweet INNER JOIN [User] on Tweet.UserId = [User].Id WHERE [User].Id =" + id + "ORDER BY CreateDate DESC");
                foreach (var item in foo)
                {
                    tweetsFromDb.Add(new Tuple<string, Tweet>(
                        (string)item.Username,
                        new Tweet { CreateDate = (DateTime)item.CreateDate, Message = item.Message, ID = item.Id, UserID = item.UserId }));
                }
            }

            return tweetsFromDb;
        }

        public List<Tuple<string, Tweet, UserToRetweet>> GetUserRetweetsFromDb(int id)
        {
            List<Tuple<string, Tweet, UserToRetweet>> tweetsFromDb = new List<Tuple<string, Tweet, UserToRetweet>>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                dynamic foo = connection.Query("SELECT oginal.Username, UserToRetweet.Id , CreateDate, Tweet.Message FROM [User] as retweet INNER JOIN UserToRetweet ON UserToRetweet.UserId = retweet.Id INNER JOIN Tweet on Tweet.Id = UserToRetweet.TweetId INNER JOIN [User] as oginal on oginal.Id = Tweet.UserId where retweet.Id = " + id);
                foreach (var item in foo)
                {
                    tweetsFromDb.Add(new Tuple<string, Tweet,UserToRetweet>(
                        (string)item.Username,
                        new Tweet { CreateDate = (DateTime)item.CreateDate, Message = item.Message },
                        new UserToRetweet{Id = item.Id}));
                }
            }

            return tweetsFromDb;
        }

        public void AddUserToDb(User user)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Execute("INSERT INTO [User] (Username, Password) values (@username, @password)", user);
            }
        }

        public List<Tuple<string, Tweet>> GetOthersTweetsFromDb(int id)
        {
            List<Tuple<string, Tweet>> tweetsFromDb = new List<Tuple<string, Tweet>>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                var foo =  connection.Query("SELECT Tweet.Id, CreateDate, Message, [User].Username, Tweet.UserId FROM Tweet INNER JOIN [User] on Tweet.UserId = [User].Id WHERE [User].Id !=" + id + "ORDER BY CreateDate DESC");
                foreach (var item in foo)
                {
                    tweetsFromDb.Add(new Tuple<string, Tweet>(
                        (string)item.Username,
                        new Tweet { CreateDate = (DateTime)item.CreateDate, Message = item.Message, ID = item.Id, UserID = item.UserId }));
                }
            }

            return tweetsFromDb;
        }

        internal void DeleteTweetDb(int tweetId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Execute("delete from Tweet where Id = " + tweetId);
            }
        }

        internal void DeleteReTweetDb(int reTweetId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Execute("delete from UserToRetweet where id = " + reTweetId);
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

        public void UpdateFirstnameToUserInDb(User user)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Execute($"UPDATE [User] SET Firstname = @Firstname WHERE Id = @Id;", user);
            }
        }

        public void UpdateLastnameToUserInDb(User user)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Execute($"UPDATE [User] SET Lastname = @Lastname WHERE Id = @Id;", user);
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
