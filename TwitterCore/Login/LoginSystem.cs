using System;
using System.Collections.Generic;
using System.Linq;

namespace TwitterCore
{
    public class LoginSystem
    {
        private readonly SQLConnection db = new SQLConnection();
        public User loginUser
        {
            get
            {
                return _user;
            }
            set
            {
                _user = value;
            }
        }
        private User _user;
        public void CreateUser(string username, string password)
        {
            if (!String.IsNullOrWhiteSpace(username) && !String.IsNullOrWhiteSpace(password))
            {
                string salt = Cryptography.CreatSalt();
                string hassedPass = Cryptography.Encrypt(Cryptography.Hash(Cryptography.Encrypt(password, salt), salt), salt);

                User user = new User(username, hassedPass)
                {
                    PasswordSalt = salt
                };
                db.AddUserToDb(user);
            }
            else
                throw new Exception("username or password cannot be whitespace or blank.");
        }

        public (bool, User) LogInUser(string username, string password)
        {
            int userID = -1;
            bool LoginSuccessful = false;
            IEnumerable<User> usersable = db.GetUsersFromDb();
            User loggedInUser = null;
            User user = null;
            try
            {
                user = usersable.Where(userx => userx.Username.ToLower() == username.ToLower()).First();

            }
            catch (Exception)
            {
                throw new Exception("The username or password is wrong");
            }

            string salt = user.PasswordSalt;
            string hassedPass = Cryptography.Encrypt(Cryptography.Hash(Cryptography.Encrypt(password, salt), salt), salt);
            if (user.Password == hassedPass)
            {
                user.IsLoggedIn = true;
                LoginSuccessful = true;
                userID = user.Id;
                loggedInUser = user;
                _user = user;
                
                db.SetUserLogdIn(user);
            }
            else
            {
                throw new Exception("The username or password is wrong");
            }

            return (LoginSuccessful, loggedInUser);
        }

        public void LogOutUser(User user)
        {
            db.SetUserLogout(user);
        }

    }
}