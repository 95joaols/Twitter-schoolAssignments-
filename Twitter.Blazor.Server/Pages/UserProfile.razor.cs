using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Twitter.Blazor.Server.Data;

namespace Twitter.Blazor.Server.Pages
{
    public partial class UserProfile
    {
        [Inject]
        private IDataAccess DataAccess { get; set; }

        public string FullName
        {
            get
            {
                if (DataAccess.User != null && !string.IsNullOrWhiteSpace(DataAccess.User.Firstname)|| !string.IsNullOrWhiteSpace(DataAccess.User.Lastname))
                {
                    return $"({DataAccess.User.Firstname}  {DataAccess.User.Lastname})";
                }
                else
                {
                    return "";
                }
            }
        }


        protected override async Task OnInitializedAsync()
        {
            await Task.Run(() => DataAccess.NotifyDataChanged += OnNotifyDataChanged);
            return;
        }


        public async Task OnNotifyDataChanged()
        {
            await InvokeAsync(() => StateHasChanged());
        }
    }
}
