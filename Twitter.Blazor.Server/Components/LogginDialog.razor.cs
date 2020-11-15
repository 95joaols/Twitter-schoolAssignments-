using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwitterCore;

namespace Twitter.Blazor.Server.Components
{
    public partial class LogginDialog
    {

        public User User { get; set; } = new User();

        public bool ShowDialog { get; set; }

        public bool HasError { get; set; }
        public string Messege { get; set; }

        [Inject]
        ISessionStorageService sessionStorage { get; set; }

        public void Show()
        {
            ShowDialog = true;
            StateHasChanged();
        }

        public void Close()
        {
            ShowDialog = false;
            StateHasChanged();
        }

        protected async Task HandleValidSubmit()
        {
            HasError = false;
            (bool, User) UserReturn = (false, null);
            await Task.Run(() =>
            {
                LoginSystem loginSystem = new LoginSystem();
                UserReturn = loginSystem.LogInUser(User.Username, User.Password);
                if (UserReturn.Item1)
                {
                    HasError = false;
                    ShowDialog = false;
                }
                else
                {
                    HasError = true;
                    Messege = "Unable to login";
                }
            });
            if (UserReturn.Item1)
            {
                await sessionStorage.SetItemAsync("CurentUser", UserReturn.Item2);
            }
            return;
        }
    }
}
