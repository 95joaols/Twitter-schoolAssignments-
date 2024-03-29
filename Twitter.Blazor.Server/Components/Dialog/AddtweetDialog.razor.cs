﻿using Microsoft.AspNetCore.Components;

using System;
using System.Threading.Tasks;

using Twitter.Blazor.Server.Data;

using TwitterCore;

namespace Twitter.Blazor.Server.Components.Dialog
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
                if (!string.IsNullOrWhiteSpace(Tweet.Message) && DataAccess.LoginUser != null)
                {
                    TweetManager tweetManager = new TweetManager();
                    try
                    {
                        tweetManager.CreateTweet(Tweet.Message, DataAccess.LoginUser.Id);
                        HasError = false;
                        ShowDialog = false;

                    }
                    catch (Exception e)
                    {
                        if (e.Message.Contains("Your message was too long!"))
                        {
                            Messege = e.Message;
                        }
                        else
                        {
                            Messege = "something went wrong";
                        }
                        HasError = true;
                    }
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
