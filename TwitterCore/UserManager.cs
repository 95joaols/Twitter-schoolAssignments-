using System.Collections.Generic;
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
            // dbControl.Update<User>(user, Table.User, "Id", user.Id, new List<string>() { "Firstname", "Lastname", "IsLoggedIn" });
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

        public IEnumerable<User> SearchUsers(string search)
        {


            //                if (!String.IsNullOrWhiteSpace(search))           // TODO: Add later when we don't need to debug any longer.
            return db.SearchUsers(search);
            //                else
            //                    return null;
        }

        public List<Tuple<string, int, int>> GetFollowing(User user)
        {
            return db.GetFollowersFromDb(user);
        }

        public void SendMassage(string message, User user, int userToId)
        {
            PrivateMessage privateMessage = new PrivateMessage(user.Id,userToId,message);
            db.PrivateMessageToDb(privateMessage);
        }
    }
}