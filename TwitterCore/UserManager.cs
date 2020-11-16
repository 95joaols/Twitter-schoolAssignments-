using System.Collections.Generic;
using Repository;
using Repository.Enums;

namespace TwitterCore
{
    public class UserManager
    {
        private readonly IFullControl dbControl;

        public UserManager()
        {
            dbControl = Faktorio.GetRepository();
        }
        public void AddBioToUser(string bio, User user)
        {

            user.Biography = bio;
            dbControl.Update<User>(user, Table.User, "Id", user.Id, new List<string>() { "Firstname", "Lastname", "IsLoggedIn" });
        }

        public void AddFollwingOfUser()
        {
            

        }
    }
}