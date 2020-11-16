using TwitterCore;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using Twitter.Blazor.Server.Data;

namespace Twitter.Blazor.Server.Components
{
    public partial class CurrentUser
    {
        [Inject]
        private IDataAccess DataAccess { get; set; }

        protected LoginDialog LoginDialog { get; set; }
        protected LogoutDialog LogoutDialog { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await Task.Run(() => DataAccess.NotifyDataChanged += OnNotifyDataChanged);
        }

        protected void ShowLoginDialog()
        {
            LoginDialog.Show();
        }
        protected void ShowLogoutDialog()
        {
            LogoutDialog.Show();
        }

        public async Task OnNotifyDataChanged()
        {
            await InvokeAsync(() => StateHasChanged());
        }
    }
}
