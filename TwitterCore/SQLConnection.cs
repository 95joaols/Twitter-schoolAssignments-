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

        public void AddUserToDb(User user)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Execute("INSERT INTO [User] (Username, Password) values (@username, @password)", user);
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
    }
}