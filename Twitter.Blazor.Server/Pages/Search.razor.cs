using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Twitter.Blazor.Server.Data;

namespace Twitter.Blazor.Server.Pages
{
    public enum SearchType
    {
        user,
        tweet
    }

    public partial class Search
    {
        [Inject]
        private IDataAccess DataAccess { get; set; }
        public SearchType SearchTypeP { get; set; }

        protected override async Task OnInitializedAsync()
        {
            DataAccess.TweetType = TweetTyp.Search;
            await Task.Run(() => DataAccess.NotifyDataChanged += OnNotifyDataChanged);
            return;
        }
        public async Task OnNotifyDataChanged()
        {
            await InvokeAsync(() => StateHasChanged());
        }
    }
}
