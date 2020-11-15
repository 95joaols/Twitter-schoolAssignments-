using TwitterCore;
using Microsoft.AspNetCore.Components;

namespace Twitter.Blazor.Server.Components
{
    public partial class CurrentUser
    {
        [Parameter]
        public User User { get; set; }

        [Parameter]
        public EventCallback<bool> Loggedout { get; set; }

        protected LogginDialog LogginDialog { get; set; }


        protected void ShowLoginDialog()
        {
            LogginDialog.Show();
        }
       
    }
}
