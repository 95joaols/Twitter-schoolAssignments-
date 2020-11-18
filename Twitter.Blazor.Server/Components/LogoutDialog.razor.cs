using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using Twitter.Blazor.Server.Data;

namespace Twitter.Blazor.Server.Components
{
    public partial class LogoutDialog
    {
        public bool ShowDialog { get; set; }

        [Inject]
        private IDataAccess DataAccess { get; set; }

        [Inject]
        NavigationManager NavigationManager { get; set; }

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

        protected async Task LogOut()
        {
            await Task.Run(() =>
            {
                DataAccess.LogingOut();
                NavigationManager.NavigateTo("/");
            });
        }
    }
}
