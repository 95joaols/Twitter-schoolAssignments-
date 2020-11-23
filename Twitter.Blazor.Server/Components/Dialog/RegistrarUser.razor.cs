using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;
using Twitter.Blazor.Server.Data;
using ConsoleGUI;

namespace Twitter.Blazor.Server.Components.Dialog
{
    public partial class RegistrarUser
    {
        public User User { get; set; } = new User();
        public string SaftyPassword = "";

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

                if (User.Password == SaftyPassword)
                {
                    try
                    {
                        DataAccess.RegistrarUser(User.Username, User.Password);
                    }
                    catch (Exception e)
                    {
                        HasError = true;
                        Messege = e.Message;
                        DataAccess.Loading = false;
                        return;
                    }
                    HasError = false;
                    ShowDialog = false;
                    DataAccess.Loading = false;
                }
                else
                {
                    HasError = true;
                    Messege = "The password do not mach";

                    User.Password = "";
                    SaftyPassword = "";
                    DataAccess.Loading = false;
                }
            });
        }
    }
}
