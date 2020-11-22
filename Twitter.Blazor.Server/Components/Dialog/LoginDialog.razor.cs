using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;
using Twitter.Blazor.Server.Data;
using TwitterCore;

namespace Twitter.Blazor.Server.Components.Dialog
{
    public partial class LoginDialog
    {

        public User User { get; set; } = new User();

        public bool ShowDialog { get; set; }

        public bool HasError { get; set; }
        public string Messege { get; set; }

        [Inject]
        private IDataAccess DataAccess { get; set; }

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
            StateHasChanged();
            DataAccess.Loading = true;
            HasError = false;
            await Task.Run(() =>
            {
                try
                {
                    if (DataAccess.LogingIn(User.Username, User.Password))
                    {
                        HasError = false;
                        ShowDialog = false;
                        DataAccess.Loading = false;
                    }
                }
                catch (Exception e)
                {
                    HasError = true;
                    Messege = e.Message;
                    DataAccess.Loading = false;
                }
            });
        }
    }
}
