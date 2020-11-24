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
        private NavigationManager NavigationManager { get; set; }

        [Parameter]
        public string UserId { get; set; }

        private EditUserDialog EditUserDialog { get; set; }
        private MessageDialog MessageDialog { get; set; }

        public string FullName
        {
            get
            {
                if ((DataAccess.UserCheck != null && !string.IsNullOrWhiteSpace(DataAccess.UserCheck.Firstname)) || !string.IsNullOrWhiteSpace(DataAccess.UserCheck.Lastname))
                {
                    return $"({DataAccess.UserCheck.Firstname}  {DataAccess.UserCheck.Lastname})";
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
                if (int.TryParse(UserId, out int id))
                {
                    try
                    {
                        DataAccess.UserCheck = DataAccess.GetUser(id);
                    }
                    catch (System.Exception)
                    {
                        NavigationManager.NavigateTo("/");
                    }
                    DataAccess.TweetType = TweetTyp.User;
                    DataAccess.NotifyDataChanged += OnNotifyDataChanged;
                }
                else
                {
                    NavigationManager.NavigateTo("/");
                }
            });
        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await Task.Run(() =>
            {
                if (DataAccess.UserCheck == null)
                {
                    if (int.TryParse(UserId, out int id))
                    {
                        try
                        {
                            DataAccess.UserCheck = DataAccess.GetUser(id);
                        }
                        catch (System.Exception)
                        {
                            NavigationManager.NavigateTo("/");
                        }
                    }
                    else
                    {
                        NavigationManager.NavigateTo("/");
                    }
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
        protected void SendMessage()
        {
            MessageDialog.Show();
        }
    }
}
