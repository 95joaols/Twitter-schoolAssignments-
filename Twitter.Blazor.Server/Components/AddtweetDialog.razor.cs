using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Twitter.Blazor.Server.Data;
using TwitterCore;

namespace Twitter.Blazor.Server.Components
{
    public partial class AddtweetDialog
    {
        public Tweet Tweet { get; set; } = new Tweet();

        public bool ShowDialog { get; set; }

        public bool HasError { get; set; }
        public string Messege { get; set; }

        [Inject]
        private IDataAccess DataAccess { get; set; }

        protected async Task HandleValidSubmit()
        {
            StateHasChanged();
            DataAccess.Loading = true;
            HasError = false;
            await Task.Run(() =>
            {
                if (!string.IsNullOrWhiteSpace(Tweet.Message) && DataAccess.User != null)
                {
                    TweetManager tweetManager = new TweetManager();
                    tweetManager.CreateTweet(Tweet.Message, DataAccess.User.Id);
                    HasError = false;
                    ShowDialog = false;
                }
                else
                {
                    HasError = true;
                    Messege = "Unable to crate tweet";
                }
                DataAccess.Loading = false;
            });

            }
        public void Show()
        {
            ShowDialog = true;
            StateHasChanged();
        }

        public void Close()
        {
            ShowDialog = false;
            StateHasChanged();
        }
    }
}
