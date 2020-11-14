using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwitterCore;
using Microsoft.AspNetCore.Components;

namespace Twitter.Blazor.Server.Components
{
    public partial class CurrentUser
    {
        [Parameter]
        public User User { get; set; }
    }
}
