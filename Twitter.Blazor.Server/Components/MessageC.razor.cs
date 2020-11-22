using Microsoft.AspNetCore.Components;
using System;

namespace Twitter.Blazor.Server.Components
{
    public partial class MessageC
    {
        [Parameter]
        public Tuple<string, string, int> MessageP { get; set; }
    }
}
