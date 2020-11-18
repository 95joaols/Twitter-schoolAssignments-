using System.Collections.Generic;
using Repository;
using Repository.Enums;
using System.Linq;

namespace TwitterCore
{
    public class UserManager
    {
        SQLConnection db = new SQLConnection("Server=40.85.84.155;Database=OOPGroup1;User=Student11;Password=zombie-virus@2020;");
        private readonly IFullControl dbControl;

        public UserManager()
        {
            dbControl = Faktorio.GetRepository();
        }
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
    }
}