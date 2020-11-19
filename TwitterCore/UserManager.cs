using System.Collections.Generic;

namespace TwitterCore
{
    public class UserManager
    {
        SQLConnection db = new SQLConnection("Server=40.85.84.155;Database=OOPGroup1;User=Student11;Password=zombie-virus@2020;");

        public void AddBioToUser(string bio, User user)
        {

            user.Biography = bio;
            db.UpdateBioToUserInDb(user);
            // dbControl.Update<User>(user, Table.User, "Id", user.Id, new List<string>() { "Firstname", "Lastname", "IsLoggedIn" });
        }

        public void AddFollwingOfUser(User loggdInUser,int followId) 
        {
            UserToUser userToUser = new UserToUser(loggdInUser.Id,followId);
            db.AddUserFollowingToDb(userToUser);
 
        }

        public IEnumerable<User> SearchUsers(string search)
        {


            //                if (!String.IsNullOrWhiteSpace(search))           // TODO: Add later when we don't need to debug any longer.
            return db.SearchUsers(search);
            //                else
            //                    return null;
        }
    }
}