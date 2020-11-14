using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwitterCore;
using Microsoft.AspNetCore.Components;

namespace Twitter.Blazor.Server.Components
{
    public partial class CurrentUser
    {
        [Parameter]
        public User User { get; set; }

        [Parameter]
        public EventCallback<User> LoggedIn { get; set; }
        [Parameter]
        public EventCallback<bool> Loggedout { get; set; }

        protected LogginDialog LogginDialog { get; set; }


        protected void ShowLoginDialog()
        {
            LogginDialog.Show();
        }
        public async void LogginDialog_OnLoggin(User LogginUser)
        {
            User = LogginUser;
            await LoggedIn.InvokeAsync(LogginUser);
        }
    }
}
