using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using Twitter.Blazor.Server.Components.Dialog;
using Twitter.Blazor.Server.Data;

namespace Twitter.Blazor.Server.Pages
{
    public partial class UserProfile
    {
        [Inject]
        private IDataAccess DataAccess { get; set; }
        [Inject]
        NavigationManager NavigationManager { get; set; }

        [Parameter]
        public string UserId { get; set; }

        EditUserDialog EditUserDialog { get; set; }

        public string FullName
        {
            get
            {
                if (DataAccess.LoginUser != null && !string.IsNullOrWhiteSpace(DataAccess.LoginUser.Firstname) || !string.IsNullOrWhiteSpace(DataAccess.LoginUser.Lastname))
                {
                    return $"({DataAccess.LoginUser.Firstname}  {DataAccess.LoginUser.Lastname})";
                }
                else
                {
                    return "";
                }
            }
        }


        protected override async Task OnInitializedAsync()
        {
            await Task.Run(() =>
            {
                //DataAccess
                DataAccess.TweetType = TweetTyp.User;
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
            });
        }

        public async Task OnNotifyDataChanged()
        {
            await InvokeAsync(() => StateHasChanged());
        }

        protected void EditUser()
        {
            EditUserDialog.Show();
        }
    }
}
