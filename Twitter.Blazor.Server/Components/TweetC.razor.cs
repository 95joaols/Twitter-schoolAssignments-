using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwitterCore;

namespace Twitter.Blazor.Server.Components
{
    public partial class TweetC
    {
        [Parameter]
        public Tweet TweetP { get; set; }
    }
}
