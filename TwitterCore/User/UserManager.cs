using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;
using System.Linq;

namespace TwitterCore
{
    public class UserManager
    {
        readonly SQLConnection db = new SQLConnection();

        public void AddFollwingOfUser(User loggdInUser, int followId)
        {
            UserToUser userToUser = new UserToUser(loggdInUser.Id, followId);
            db.AddUserFollowingToDb(userToUser);
        }

        public void AddBioToUser(string bio, User user)
        {
            if (bio.Length <= 200)
            {
                user.Biography = bio;
                db.UpdateBioToUserInDb(user);
            }
            else
                throw new Exception("Biography must be under 200 characters.");
        }

        public void UpdateFirstnameUser(User user, string firstname)
        {
            if (firstname.Length <= 50)
            {
                user.Firstname = firstname;
                db.UpdateFirstnameToUserInDb(user);
            }
            else
                throw new Exception("Username must be under 50 characters.");
        }

        public void UpdateLastnameUser(User user, string lastname)
        {
            if (lastname.Length <= 50)
            {
                user.Lastname = lastname;
                db.UpdateLastnameToUserInDb(user);
            }
            else
                throw new Exception("Lastname must be under 50 characters.");            
        }

        public ReadOnlyCollection<User> SearchUsers(string search, User user)
        {
            IEnumerable<User> userFound = db.SearchUsers(search, user);
            return new ReadOnlyCollection<User>(userFound.ToList());
        }

        public ReadOnlyCollection<Tuple<string, int>> GetFollowing(User user)
        {
            List<Tuple<string, int>> foo = db.GetFollowersFromDb(user);
            return new ReadOnlyCollection<Tuple<string, int>>(foo);
        }

        public void SendMessage(string message, User user, int userToId)
        {
            PrivateMessage privateMessage = new PrivateMessage(user.Id, userToId, message);

            try
            {
                db.PrivateMessageToDb(privateMessage);
            }
            catch (System.Exception e)
            {
                if(e.HResult == -2146232060)
                    throw new Exception("Your message was too long! A tweet can be up to 100 characters long.");
            }
        }

        public ReadOnlyCollection<Tuple<string, int>> GetUserMail(User user)
        {
            List<Tuple<string, int>> foo = db.GetUserMailFromDb(user);
            return new ReadOnlyCollection<Tuple<string, int>>(foo);
        }

        public ReadOnlyCollection<Tuple<string, PrivateMessage>> GetMailConven(User user, int userToId)
        {
            List<Tuple<string, PrivateMessage>> foo = db.GetPrivateMessageConvoFromDb(user, userToId);
            return new ReadOnlyCollection<Tuple<string, PrivateMessage>>(foo);
        }

        public ReadOnlyCollection<User> GetFriendsBio(User user)
        {
            IEnumerable<User> friendsBio = db.GetFriendsBioFromDb(user);
            return new ReadOnlyCollection<User>(friendsBio.ToList());
        }

        public ReadOnlyCollection<User> GetOnlineUser()
        {
            IEnumerable<User> onlineUser = db.GetOnlineUserFromDb();
            return new ReadOnlyCollection<User>(onlineUser.ToList());
        }

        public ReadOnlyCollection<User> SINGLEUSER(int id)
        {
            IEnumerable<User> onlineUser = db.SINGLEUSER(id);
            return new ReadOnlyCollection<User>(onlineUser.ToList());
        }
    }
}