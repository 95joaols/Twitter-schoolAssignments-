using System.Collections.Generic;
using Repository;
using Repository.Enums;

namespace TwitterCore
{
    public class LoginSystem
    {
        //SQLConnection db = new SQLConnection("Server=40.85.84.155;Database=OOPGroup1;User=Student11;Password=zombie-virus@2020;");

        List<User> users = new List<User>();
        IFullControl dbControl;

        public LoginSystem()
        {
            dbControl = Faktorio.GetRepository();
        }

        public void CreateUser(string username, string password)
        {
            User user = new User(username, password);
            // add user to DB
            //db.CreateUserToDB() typ

            dbControl.Add<int>(user, Table.User, "Id");
            //db.AddUserToDb(user);
        }

        public (bool, User) LogInUser(string username, string password)
        {
            int userID = -1;
            bool LoginSuccessful = false;

            //get users from db

            Dictionary<string, string> where = new Dictionary<string, string>();
            where.Add("Username", username);
            //IEnumerable<User> usersable = db.GetUsers();
            User user = dbControl.GetSingel<User>("*", Table.User, where);

            // foreach (User user in usersable)
            // {
            User loggedInUser = null;
            if (password == user.Password && username == user.Username)
            {
                user.IsLoggedIn = true;
                LoginSuccessful = user.IsLoggedIn;

                userID = user.Id;

                loggedInUser = user;
                // break;
            }
            // }
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