using Microsoft.AspNetCore.Components;
using System;
using Twitter.Blazor.Server.Components.Dialog;

namespace Twitter.Blazor.Server.Components
{
    public partial class MessageC
    {
        [Parameter]
        public Tuple<string, string, int> MessageP { get; set; }

        protected Message Message { get; set; }
        protected void SendMessage()
        {

            Message.Show();
        }
    }
}
