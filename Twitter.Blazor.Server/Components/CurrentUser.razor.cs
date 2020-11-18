using TwitterCore;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using Twitter.Blazor.Server.Data;
using Blazored.SessionStorage;

namespace Twitter.Blazor.Server.Components
{
    public partial class CurrentUser
    {
        [Inject]
        private IDataAccess DataAccess { get; set; }

        [Inject]
        ISessionStorageService SessionStorage { get; set; }

        protected LoginDialog LoginDialog { get; set; }
        protected LogoutDialog LogoutDialog { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await Task.Run(() =>
            {
                DataAccess.NotifyDataChanged += OnNotifyDataChanged;
                DataAccess.LoggedIn += OnLoggedIn;
                DataAccess.LoggedOut += OnLoggedOut;

            });


        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            User t = DataAccess.User;
            DataAccess.User = await SessionStorage.GetItemAsync<User>("CurentUser");

            if (t?.Id != DataAccess.User?.Id)
            {
                await OnNotifyDataChanged();
            }
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
        public async Task OnLoggedIn()
        {
            await SessionStorage.SetItemAsync("CurentUser", DataAccess.User);
        }
        public async Task OnLoggedOut()
        {
            await SessionStorage.SetItemAsync<User>("CurentUser", null);

        }

    }
}
