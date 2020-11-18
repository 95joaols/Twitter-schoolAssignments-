using Microsoft.AspNetCore.Components;
using Twitter.Blazor.Server.Data;
using TwitterCore;

namespace Twitter.Blazor.Server.Components
{
    public partial class TweetC
    {
        [Parameter]
        public Tweet TweetP { get; set; }

        [Inject]
        private IDataAccess DataAccess { get; set; }

        private void Remove()
        {
            TweetManager tweetManager = new TweetManager();
            tweetManager.Delete(TweetP.ID, DataAccess.User);
        }
    }
}
