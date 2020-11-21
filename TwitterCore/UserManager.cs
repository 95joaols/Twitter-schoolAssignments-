using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;

namespace TwitterCore
{
    public class UserManager
    {
        SQLConnection db = new SQLConnection();

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

        public ReadOnlyCollection<User> SearchUsers(string search)
        {
            IEnumerable<User> foo = db.SearchUsers(search);
            List<User> foos = new List<User>(foo);
            return new ReadOnlyCollection<User>(foos);
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

        public ReadOnlyCollection<User> GetFriendsBio(User user)
        {
            IEnumerable<User> foo = db.GetFriendsBioFromDb(user);
            List<User> fooo = new List<User>(foo);
            return new ReadOnlyCollection<User>(fooo);
        }
    }
}