using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using TwitterCore;

namespace Twitter.Blazor.Server.Components
{
    public partial class LogginDialog
    {
        public User User { get; set; } = new User();

        public bool ShowDialog { get; set; }
        [Parameter]
        public EventCallback<User> LoggedIn { get; set; }

        public void Show()
        {
            ShowDialog = true;
        }

        public void Close()
        {
            ShowDialog = false;
        }

        protected async Task HandleValidSubmit()
        {
            await Task.Run(() =>
            {
                LoginSystem loginSystem = new LoginSystem();
                (bool, User) UserReturn = loginSystem.LogInUser(User.Username, User.Password);
               if(UserReturn.Item1)
                {
                    ShowDialog = false;
                    LoggedIn.InvokeAsync(UserReturn.Item2);
                }
            });
            return;
        }
    }
}
