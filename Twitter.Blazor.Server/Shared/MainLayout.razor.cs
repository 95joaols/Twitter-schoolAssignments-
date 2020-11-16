using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using Twitter.Blazor.Server.Data;

namespace Twitter.Blazor.Server.Shared
{
    public partial class MainLayout
    {

        [Inject]
        private IDataAccess DataAccess { get; set; }

        public override async Task SetParametersAsync(ParameterView parameters)
        {
            //await Task.Run(() => DataAccess.NotifyDataChanged += OnNotifyDataChanged);
        }

       // public async Task OnNotifyDataChanged()
        //{
        //    await InvokeAsync(() => StateHasChanged());
        //}
    }
}


