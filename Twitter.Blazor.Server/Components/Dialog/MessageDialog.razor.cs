using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using Twitter.Blazor.Server.Data;
using TwitterCore;

namespace Twitter.Blazor.Server.Components.Dialog
{
    public partial class MessageDialog
    {
        [Parameter]
        public string Nameto { get; set; }

        [Parameter]
        public int Idto { get; set; }

        public PrivateMessage PrivateMessage { get; set; } = new PrivateMessage();

        public bool ShowDialog { get; set; }

        public bool HasError { get; set; }
        public string Messege { get; set; }

        [Inject]
        private IDataAccess DataAccess { get; set; }

        protected async Task HandleValidSubmit()
        {
            StateHasChanged();
            DataAccess.Loading = true;
            HasError = false;
            await Task.Run(() =>
            {
                if (!string.IsNullOrWhiteSpace(PrivateMessage.Message))
                {

                    try
                    {
                        new UserManager().SendMessage(PrivateMessage.Message, DataAccess.LoginUser, Idto);

                        HasError = false;
                        ShowDialog = false;
                        DataAccess.Update();
                    }
                    catch (System.Exception)
                    {
                        HasError = true;
                        Messege = "something went wrong";
                    }
                }
                else
                {
                    HasError = true;
                    Messege = "Cant be emty";
                }
                DataAccess.Loading = false;
            });

        }
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
    }
}
