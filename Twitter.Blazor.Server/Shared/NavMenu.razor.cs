using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Twitter.Blazor.Server.Data;

namespace Twitter.Blazor.Server.Shared
{
    public partial class NavMenu
    {
        [Inject]
        private IDataAccess DataAccess { get; set; }

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
