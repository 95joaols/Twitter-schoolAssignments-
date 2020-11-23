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
            user.Biography = bio;
            db.UpdateBioToUserInDb(user);
        }

        public void UpdateFirstnameUser(User user, string firstname)
        {
            user.Firstname = firstname;
            db.UpdateFirstnameToUserInDb(user);
        }

        public void UpdateLastnameUser(User user, string lastname)
        {
            user.Lastname = lastname;
            db.UpdateLastnameToUserInDb(user);
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

        public void SendMassage(string message, User user, int userToId)
        {
            PrivateMessage privateMessage = new PrivateMessage(user.Id, userToId, message);
            db.PrivateMessageToDb(privateMessage);
        }

        public ReadOnlyCollection<Tuple<string, string, int>> GetUserMail(User user)
        {
            List<Tuple<string, string, int>> foo = db.GetUserMailFromDb(user);
            return new ReadOnlyCollection<Tuple<string, string, int>>(foo);
        }

        public ReadOnlyCollection<Tuple<string, PrivateMessage>> GetMailConven(User user, int userToId)
        {
            List<Tuple<string, PrivateMessage>> foo = db.GetUserMailFromDbfoo(user,userToId);
            return new ReadOnlyCollection<Tuple<string,PrivateMessage>>(foo);
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