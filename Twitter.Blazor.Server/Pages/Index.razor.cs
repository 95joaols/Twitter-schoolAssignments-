using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using Twitter.Blazor.Server.Data;
using TwitterCore;

namespace Twitter.Blazor.Server.Pages
{
    public partial class Index
    {
        [Inject]
        private IDataAccess DataAccess { get; set; }

        public IEnumerable<Tweet> TopTweets { get; private set; }

        protected override async Task OnInitializedAsync()
        {
            await Task.Run(() => TopTweets = DataAccess.TopTweets);
        }
    }
}
