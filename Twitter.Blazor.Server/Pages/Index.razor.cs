using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using Twitter.Blazor.Server.Components.Dialog;
using Twitter.Blazor.Server.Data;

namespace Twitter.Blazor.Server.Pages
{
    public partial class Index
    {
        [Inject]
        private IDataAccess DataAccess { get; set; }

        protected AddtweetDialog AddtweetDialog { get; set; }

        protected override async Task OnInitializedAsync()
        {
            DataAccess.TweetType = TweetTyp.Top;
            await Task.Run(() => DataAccess.NotifyDataChanged += OnNotifyDataChanged);
            return;
        }

        public async Task OnNotifyDataChanged()
        {
            await InvokeAsync(() => StateHasChanged());
        }

        protected void Addtweet()
        {

            AddtweetDialog.Show();
        }
    }
}