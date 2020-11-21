using Microsoft.AspNetCore.Components;
using TwitterCore;

namespace Twitter.Blazor.Server.Components
{

    public partial class UserC
    {
        [Parameter]
        public User UserP { get; set; }
    }
}
