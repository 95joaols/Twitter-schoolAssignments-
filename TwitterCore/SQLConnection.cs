using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Dapper;
using System.Text.Json;
using System.IO;

namespace TwitterCore
{
    public class JsonLoginClass                 // Used for json-file.
    {
        public string Connection { get; set; }
    }

    class SQLConnection
    {
        private const string jsonFile = "appsettings2.json";            // json file to use.
        private readonly JsonLoginClass connectionJson;

        public SQLConnection()
        {
            string readJsonFile = File.ReadAllText(jsonFile);
            connectionJson = JsonSerializer.Deserialize<JsonLoginClass>(readJsonFile);
        }

        public IEnumerable<User> GetUsersFromDb()
        {
            using (SqlConnection connection = new SqlConnection(connectionJson.Connection))
            {
                return connection.Query<User>("SELECT Id, Username, Password, Biography, Firstname, Lastname, BINARYBITDEFAULTZERO,PasswordSalt FROM [User]");
            }
        }

        public IEnumerable<User> GetOnlineUserFromDb()
        {
            using (SqlConnection connection = new SqlConnection(connectionJson.Connection))
            {
                return connection.Query<User>("SELECT * From [User] WHERE BINARYBITDEFAULTZERO = 1;");
            }
        }

        public List<Tuple<string, Tweet>> GetTweetsFromDb()
        {
            List<Tuple<string,  Tweet>> tweetsFromDb = new List<Tuple<string ,Tweet>>();
            using (SqlConnection connection = new SqlConnection(connectionJson.Connection))
            {
                var foo = connection.Query("SELECT TOP 10 t.Id, t.CreateDate, t.Message, u.Username, t.UserId FROM Tweet as t INNER JOIN [User] as u on t.UserId = u.Id ORDER BY CreateDate DESC");
                foreach (var item in foo)
                {
                    tweetsFromDb.Add(new Tuple<string, Tweet>(
                        (string)item.Username,
                        new Tweet {ID = item.Id, CreateDate = (DateTime)item.CreateDate, Message = item.Message, UserID = item.UserId }));
                }
            }
            return tweetsFromDb;
        }

        internal void SetUserLogdIn(User user)
        {
            using (SqlConnection connection = new SqlConnection(connectionJson.Connection))
            {
                connection.Execute("update [User] SET BINARYBITDEFAULTZERO = 1 where [User].Id = " + user.Id);
            }
        }

        internal void SetUserLogout(User user)
        {
            using (SqlConnection connection = new SqlConnection(connectionJson.Connection))
            {
                connection.Execute("update [User] SET BINARYBITDEFAULTZERO = 0 where [User].Id = " + user.Id);
            }
        }

        public List<Tuple<string, Tweet>> GetUserTweetsFromDb(int id)
        {
            List<Tuple<string, Tweet>> tweetsFromDb = new List<Tuple<string, Tweet>>();
            using (SqlConnection connection = new SqlConnection(connectionJson.Connection))
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
            using (SqlConnection connection = new SqlConnection(connectionJson.Connection))
            {
                dynamic foo = connection.Query(@"SELECT oginal.Username, UserToRetweet.Id , CreateDate, Tweet.Message 
                FROM [User] as retweet INNER JOIN UserToRetweet ON UserToRetweet.UserId = retweet.Id
                INNER JOIN Tweet on Tweet.Id = UserToRetweet.TweetId
                INNER JOIN [User] as oginal on oginal.Id = Tweet.UserId
                WHERE retweet.Id = " + id);
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
            using (SqlConnection connection = new SqlConnection(connectionJson.Connection))
            {
                connection.Execute("INSERT INTO [User] (Username, Password,PasswordSalt) values (@username, @password, @PasswordSalt)", user);
            }
        }

        public List<Tuple<string, Tweet>> GetOthersTweetsFromDb(int id)
        {
            List<Tuple<string, Tweet>> tweetsFromDb = new List<Tuple<string, Tweet>>();
            using (SqlConnection connection = new SqlConnection(connectionJson.Connection))
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
            using (SqlConnection connection = new SqlConnection(connectionJson.Connection))
            {
                connection.Execute("delete from Tweet where Id = " + tweetId);
            }
        }

        internal void DeleteReTweetDb(int reTweetId)
        {
            using (SqlConnection connection = new SqlConnection(connectionJson.Connection))
            {
                connection.Execute("delete from UserToRetweet where id = " + reTweetId);
            }
        }

        internal void RetweetToDb(UserToRetweet retweet)
        {
            using (SqlConnection connection = new SqlConnection(connectionJson.Connection))
            {
                connection.Execute("INSERT INTO UserToRetweet (UserId, TweetId) values (@userId, @tweetId)", retweet);
            }
        }

        public void AddTweetToDb(Tweet tweet)
        {
            using (SqlConnection connection = new SqlConnection(connectionJson.Connection))
            {
                connection.Execute("INSERT INTO Tweet (UserId, Message) values (@userId, @message)", tweet);
            }
        }

        public void UpdateBioToUserInDb(User user)
        {
            using (SqlConnection connection = new SqlConnection(connectionJson.Connection))
            {
                connection.Execute($"UPDATE [User] SET Biography = @Biography WHERE Id = @Id;", user);
            }
        }

        public void UpdateFirstnameToUserInDb(User user)
        {
            using (SqlConnection connection = new SqlConnection(connectionJson.Connection))
            {
                connection.Execute($"UPDATE [User] SET Firstname = @Firstname WHERE Id = @Id;", user);
            }
        }

        public void UpdateLastnameToUserInDb(User user)
        {
            using (SqlConnection connection = new SqlConnection(connectionJson.Connection))
            {
                connection.Execute($"UPDATE [User] SET Lastname = @Lastname WHERE Id = @Id;", user);
            }
        }
        
        public void AddUserFollowingToDb(UserToUser userToUser)
        {
            using (SqlConnection connection = new SqlConnection(connectionJson.Connection))
            {
                connection.Execute("INSERT INTO UserToUser (UserId,FollowingId) values (@UserId, @FollowingId)", userToUser);                 
            }
        }

        public IEnumerable<User> SearchUsers(string search, User user)
        {
            using (SqlConnection connection = new SqlConnection(connectionJson.Connection))
            {
                return connection.Query<User>("EXEC SearchProcedureUsers @SearchString = @Search, @IdToExclude = @Id", new { @Search = search, @Id = user.Id });
            }
        }

        /*public IEnumerable<User> SearchUsers(string search)
        {
            using (SqlConnection connection = new SqlConnection(connectionJson.Connection))
            {
                return connection.Query<User>("EXEC SearchProcedureUsers @SearchString = @Search", new { @Search = search });
            }
        } */

        public List<Tuple<string,Tweet>> SearchTweets(string search)
        {
            List<Tuple<string, Tweet>> tweetsFromDb = new List<Tuple<string, Tweet>>();
            using (SqlConnection connection = new SqlConnection(connectionJson.Connection))
            {
                var foo = connection.Query("EXEC SearchProcedureTweets @SearchString = @Search", new { @Search = search });
                foreach (var item in foo)
                {
                    tweetsFromDb.Add(new Tuple<string, Tweet>(
                        (string)item.Username,
                        new Tweet { CreateDate = (DateTime)item.CreateDate, Message = item.Message, ID = item.Id, UserID = item.UserId }));
                }
            }

            return tweetsFromDb;
        }

        public List<Tuple<string, int>> GetFollowersFromDb(User user)
        {
            List<Tuple<string, int>> following = new List<Tuple<string, int>>();
            using (SqlConnection connection = new SqlConnection(connectionJson.Connection))
            {
                var foo = connection.Query(@"select distinct FollowingId, x.Username, Orginal.Id
                                            from[User] as Orginal
                                            full join UserToUser on UserToUser.UserId = Orginal.Id
                                            full join[User] as x on x.Id = UserToUser.FollowingId
                                            where Orginal.Id = " + user.Id);
                foreach (var item in foo)
                {
                    following.Add(new Tuple<string, int>(
                        (string)item.Username,
                        (int)item.FollowingId));
                }
            }

            return following;
        }

        public void PrivateMessageToDb(PrivateMessage privateMessage)
        {
            using (SqlConnection connection = new SqlConnection(connectionJson.Connection))
            {
                connection.Execute("INSERT INTO PrivateMessage (UserFromId,UserToId,Message) values (@UserFromId, @UserToId, @Message)", privateMessage);
            }
        }

        public List<Tuple<string, string, int>> GetUserMailFromDb(User user)
        {
            List<Tuple<string, string, int>> following = new List<Tuple<string, string, int>>();
            using (SqlConnection connection = new SqlConnection(connectionJson.Connection))
            {
                var foo = connection.Query(@"select UserFromId, MailFrom.Username, Message 
                from PrivateMessage
                inner join [User] as MailFrom on MailFrom.Id = PrivateMessage.UserFromId
                inner join [User] as MailTo on MailTo.Id = PrivateMessage.UserToId where MailTo.Id = " + user.Id);
                foreach (var item in foo)
                {
                    following.Add(new Tuple<string, string, int>(
                        (string)item.Username,
                        (string)item.Message,
                        (int)item.UserFromId));
                }
            }

            return following;
        }

        public IEnumerable<User> GetFriendsBioFromDb(User user)
        {
            using (SqlConnection connection = new SqlConnection(connectionJson.Connection))
            {
                return connection.Query<User>(@"select distinct Friend.Id ,Friend.Username, Friend.Firstname, Friend.Lastname,Friend.Biography
                from [User] as I
                inner join UserToUser on UserToUser.UserId = I.Id
                inner join [User] as Friend on Friend.Id = UserToUser.FollowingId
                where I.Id = " + user.Id);
            }
        }
        
        public IEnumerable<User> SINGLEUSER(int user)
        {
        using (SqlConnection connection = new SqlConnection(connectionJson.Connection))
            {
                return connection.Query<User>(@"SELECT * FROM [User] WHERE Id = @UserId", new { @UserId = user });
            }
        }
    }
}
