using TwitterCore;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using Twitter.Blazor.Server.Data;
using Blazored.SessionStorage;
using Twitter.Blazor.Server.Components.Dialog;

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
        protected RegistrarUser RegistrarUser { get; set; }

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
            DataAccess.LoginUser = await SessionStorage.GetItemAsync<User>("CurentUser");

            if (DataAccess.LoginUser?.Id != DataAccess.LoginUser?.Id)
            {
                DataAccess.Update();
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
        protected void ShowRegistrarUser()
        {
            RegistrarUser.Show();
        }

        public async Task OnNotifyDataChanged()
        {
            await InvokeAsync(() => StateHasChanged());
        }
        public async Task OnLoggedIn()
        {
            await SessionStorage.SetItemAsync("CurentUser", DataAccess.LoginUser);
        }
        public async Task OnLoggedOut()
        {
            await SessionStorage.SetItemAsync<User>("CurentUser", null);

        }

    }
}
