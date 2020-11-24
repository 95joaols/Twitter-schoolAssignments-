using Microsoft.AspNetCore.Components;
using System;

namespace Twitter.Blazor.Server.Components
{
    public partial class Conversation
    {
        [Parameter]
        public Tuple<string, int> MessageP { get; set; }
    }
}
