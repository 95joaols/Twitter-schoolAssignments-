using Microsoft.AspNetCore.Components;

using System.Threading.Tasks;

using Twitter.Blazor.Server.Components.Dialog;
using Twitter.Blazor.Server.Data;

namespace Twitter.Blazor.Server.Pages
{
    public partial class Message
    {
        [Inject]
        private IDataAccess DataAccess { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        [Parameter]
        public string UserId { get; set; }

        protected MessageDialog MessageDialog { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await Task.Run(() =>
            {
                if (int.TryParse(UserId, out int idUser))
                {
                    DataAccess.UserCheck = DataAccess.GetUser(idUser);
                }

                DataAccess.NotifyDataChanged += OnNotifyDataChanged;
            });
        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await Task.Run(() =>
            {
                if (DataAccess.LoginUser == null)
                {
                    NavigationManager.NavigateTo("/");
                }
                if (int.TryParse(UserId, out int idUser))
                {
                    DataAccess.UserCheck = DataAccess.GetUser(idUser);
                }
                else
                {
                    DataAccess.UserCheck = null;
                }
            });
        }
        public async Task OnNotifyDataChanged()
        {
            await InvokeAsync(() => StateHasChanged());
        }

        protected void SendMessage()
        {
            MessageDialog.Show();
        }
    }
}
