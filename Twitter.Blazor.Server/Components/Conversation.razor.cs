using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwitterCore;

namespace Twitter.Blazor.Server.Components
{
    public partial class Conversation
    {
        [Parameter]
        public Tuple<string, int> MessageP { get; set; }

    }
}
