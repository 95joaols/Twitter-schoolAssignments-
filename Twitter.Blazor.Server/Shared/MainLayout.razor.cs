using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using TwitterCore;

namespace Twitter.Blazor.Server.Shared
{
    public partial class MainLayout
    {
        [Inject]
        ISessionStorageService SessionStorage { get; set; }

        public User User { get; set; }

        protected override async Task OnInitializedAsync()
        {
            User = await SessionStorage.GetItemAsync<User>("CurentUser");
        }
    }
}
