using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Tattoo.StudioUI.Components.Dialogs;
using Tattoo.StudioUI.Helpers;
using Tattoo.StudioUI.Models.Forms;
using Tattoo.StudioUI.Security;
using Radzen;
using Microsoft.AspNetCore.Components.WebAssembly.Services;

namespace Tattoo.StudioUI.Layout
{
    public partial class MainLayout : LayoutComponentBase
    {
        private bool hasSeenInitView = false;
        private bool sidebarExpanded = false;

        [Inject] private GlobalState stateService { get; set; }
        [Inject] private IJSRuntime JSRuntime { get; set; }
        [Inject] private DialogService DialogService { get; set; }

        // Constructor por defecto

        protected override async Task OnInitializedAsync()
        {
            var result = await JSRuntime.InvokeAsync<string>("localStorage.getItem", "hasSeenInitView");
            hasSeenInitView = result == "true";
        }

        private void NavigateToMain()
        {
            hasSeenInitView = true;
            JSRuntime.InvokeVoidAsync("localStorage.setItem", "hasSeenInitView", "true");
            StateHasChanged();
        }

        private async Task OnClickBookNow()
        {
            ReservationDto data = await DialogService.OpenAsync<AddAppointmentPage>("Book Now",
                new Dictionary<string, object> { { "Start", DateTime.Now }, { "End", DateTime.Now } });
        }

        private void ShowCookiePolicy()
        {
            JSRuntime.InvokeVoidAsync("showCookiePolicy");
        }

        private async Task ScrollToSection(string sectionId)
        {
            await JSRuntime.InvokeVoidAsync("scrollToSection", sectionId);
        }
    }
}
