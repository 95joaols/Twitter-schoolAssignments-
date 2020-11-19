using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;
using Twitter.Blazor.Server.Data;
using TwitterCore;

namespace Twitter.Blazor.Server.Components
{
    public partial class TweetC
    {
        [Parameter]
        public Tuple<string, Tweet> TweetP { get; set; }

        [Inject]
        private IDataAccess DataAccess { get; set; }

         private async Task Remove()
        {
            DataAccess.Loading = true;
             await Task.Run(() =>
            {
                DataAccess.RemoveTweet(TweetP.Item2);
            });
        }
    }
}
