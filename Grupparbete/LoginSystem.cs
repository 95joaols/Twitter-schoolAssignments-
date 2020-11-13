using System.Collections.Generic;

namespace Grupparbete
{
    public class LoginSystem
    {
        SQLConnection db = new SQLConnection("Server=40.85.84.155;Database=OOPGroup1;User=Student11;Password=zombie-virus@2020;");

        List<User> users = new List<User>();

        public void CreateUser(string username, string password)
        {
            User user = new User(username, password);
            // add user to DB
            //db.CreateUserToDB() typ
            db.AddUserToDb(user);
        }

        public (bool, User) LogInUser(string username, string password)
        {
            int userID = -1;
            bool LoginSuccessful = false;

            //get users from db
            IEnumerable<User> usersable = db.GetUsers();
            User loggedInUser = null;

            foreach (User user in usersable)
            {
                if (password == user.Password && username == user.Username)
                {
                    user.IsLoggedIn = true;
                    LoginSuccessful = user.IsLoggedIn;

                    userID = user.Id;

                    loggedInUser = user;
                    break;
                }
            }
            return (LoginSuccessful, loggedInUser);
        }

        public void LogOutUser(User user)
        {
            user.IsLoggedIn = false;
            Program.PrintHeadMenu();         
            //Utloggad
        }
    }
}