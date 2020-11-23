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
            if (!String.IsNullOrWhiteSpace(username) && !String.IsNullOrWhiteSpace(password) && username.Length <= 50 && password.Length <= 50)
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
                throw new Exception("Username/password cannot be blank. Username and password must be under 50 characters.");
        }

        public Tuple<bool, User> LogInUser(string username, string password)
        {
            int userID = -1;
            bool LoginSuccessful = false;
            IEnumerable<User> usersable = db.GetUsersFromDb();
            User loggedInUser = null;
            User user = null;
            Console.WriteLine("- 1");

            try
            {
                user = usersable.Where(userx => userx.Username.ToLower() == username.ToLower()).First();
                Console.WriteLine("- 4");
            }
            catch (Exception)
            {
                Console.WriteLine("- 2");
                throw new Exception("The username or password is wrong");
            }
            Console.WriteLine("- 3");

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

            return new Tuple<bool,User>(LoginSuccessful, loggedInUser);
        }

        public void LogOutUser(User user)
        {
            db.SetUserLogout(user);
        }

    }
}