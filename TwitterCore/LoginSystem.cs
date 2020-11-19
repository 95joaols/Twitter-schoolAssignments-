using System.Collections.Generic;

namespace TwitterCore
{
    public class LoginSystem
    {
        SQLConnection db = new SQLConnection();

        public void CreateUser(string username, string password)
        {
            User user = new User(username, password);
            // dbControl.Add<int>(user, Table.User, "Id",new List<string>() { "CreateDate", "Username", "isRetweet", "retweetCount" });
            db.AddUserToDb(user);
        }

        public (bool, User) LogInUser(string username, string password)
        {
            int userID = -1;
            bool LoginSuccessful = false;
            // Dictionary<string, string> where = new Dictionary<string, string>();
            // where.Add("Username", username);
            IEnumerable<User> usersable = db.GetUsers();
            // User user = dbControl.GetSingel<User>("*", Table.User, where);
            User loggedInUser = null;
            foreach (User user in usersable)
            {
                
                if (password == user?.Password && username.ToLower() == user?.Username.ToLower())
                {
                    user.IsLoggedIn = true;
                    LoginSuccessful = user.IsLoggedIn;

                    userID = user.Id;

                    loggedInUser = user;
                    // break;
                }
            }
            return (LoginSuccessful, loggedInUser);
        }

        public void LogOutUser(User user)
        {
            user.IsLoggedIn = false;
            //Program.PrintHeadMenu();
            //Utloggad
        }
    }
}