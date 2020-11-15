using Microsoft.AspNetCore.Components;
using TwitterCore;

namespace Twitter.Blazor.Server.Components
{
    public partial class TweetC
    {
        [Parameter]
        public Tweet TweetP { get; set; }
    }
}
