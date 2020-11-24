using Microsoft.AspNetCore.Components;
using System;
using TwitterCore;

namespace Twitter.Blazor.Server.Components
{
    public partial class MessageC
    {
        [Parameter]
        public Tuple<string, PrivateMessage> MessageP { get; set; }   
    }
}
