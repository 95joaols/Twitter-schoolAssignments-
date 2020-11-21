using Microsoft.AspNetCore.Components;
using System;
using Twitter.Blazor.Server.Data;

namespace Twitter.Blazor.Server.Components
{
    public partial class MessageC
    {
        [Parameter]
        public Tuple<string, string, int> MessageP { get; set; }

        [Inject]
        private IDataAccess DataAccess { get; set; }
    }
}
