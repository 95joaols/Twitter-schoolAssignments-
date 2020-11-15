using System.Threading.Tasks;
using TwitterCore;

namespace Twitter.Blazor.Server.Shared
{
    public partial class MainLayout
    {
        public User User { get; set; }

        public async void CurrentUser_LoggedIn(User LogginUser)
        {
            await Task.Run(() =>
            {
                User = LogginUser;
            });
        }

    }
}
