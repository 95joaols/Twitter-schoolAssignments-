using System.Collections.Generic;
using System;


namespace TwitterCore
{
    public class LoginSystem
    {
        SQLConnection db = new SQLConnection();
        public User _user;
        public void CreateUser(string username, string password)
        {
            User user = new User(username, password);
            db.AddUserToDb(user);
        }

        public (bool, User) LogInUser(string username, string password)
        {
            int userID = -1;
            bool LoginSuccessful = false;
            IEnumerable<User> usersable = db.GetUsersFromDb();
            User loggedInUser = null;
            foreach (User user in usersable)
            {
                if (password == user?.Password && username.ToLower() == user?.Username.ToLower())
                {
                    user.IsLoggedIn = true;
                    LoginSuccessful = user.IsLoggedIn;
                    userID = user.Id;
                    loggedInUser = user;
                    _user = user;
                    user.BINARYBITDEFAULTZERO = true; // bool, istället för IsloggdIn, fast konstigt namn på denna kanske
                    db.SetUserLogdIn(user);
                    // detta är inte snyggt nåbstans, måste städas
                }
            }

            return (LoginSuccessful, loggedInUser);
        }

        public void LogOutUser(User user)
        {
            user.IsLoggedIn = false;
            db.SetUserLogout(user);
        }

    }
}