using Microsoft.AspNetCore.Components;
using ConsoleGUI;

namespace Twitter.Blazor.Server.Components
{

    public partial class UserC
    {
        [Parameter]
        public User UserP { get; set; }
    }
}
