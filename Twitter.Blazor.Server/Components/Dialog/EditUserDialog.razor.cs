using Microsoft.AspNetCore.Components;

using System.Threading.Tasks;

using Twitter.Blazor.Server.Data;

using TwitterCore;

namespace Twitter.Blazor.Server.Components.Dialog
{
    public partial class EditUserDialog
    {
        public User EditUser { get; set; } = new User();

        [Parameter]
        public User UserLoging { get; set; }

        public bool ShowDialog { get; set; }

        public bool HasError { get; set; }
        public string Messege { get; set; }

        [Inject]
        private IDataAccess DataAccess { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await Task.Run(() =>
            {
                EditUser.Firstname = UserLoging.Firstname;
                EditUser.Lastname = UserLoging.Lastname;
                EditUser.Biography = UserLoging.Biography;
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

        protected async Task HandleValidSubmit()
        {
            StateHasChanged();
            DataAccess.Loading = true;
            HasError = false;
            await Task.Run(() =>
            {

                UserManager userManager = new UserManager();
                if (EditUser.Firstname != UserLoging.Firstname)
                {
                    try
                    {
                        userManager.UpdateFirstnameUser(UserLoging, EditUser.Firstname);
                        DataAccess.LoginUser.Firstname = EditUser.Firstname;

                    }
                    catch (System.Exception)
                    {

                    }
                }
                if (EditUser.Lastname != UserLoging.Lastname)
                {
                    try
                    {
                        userManager.UpdateLastnameUser(UserLoging, EditUser.Lastname);
                        DataAccess.LoginUser.Lastname = EditUser.Lastname;


                    }
                    catch (System.Exception)
                    {
                    }
                }
                if (EditUser.Biography != UserLoging.Biography)
                {
                    try
                    {
                        userManager.AddBioToUser(EditUser.Biography, UserLoging);
                        DataAccess.LoginUser.Biography = EditUser.Biography;

                    }
                    catch (System.Exception)
                    {
                    }
                }
                ShowDialog = false;
                DataAccess.Loading = false;
            });
        }
    }
}
