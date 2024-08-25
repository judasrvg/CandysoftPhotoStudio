using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Tattoo.InkVibesTattoo.Components.Dialogs;
using Tattoo.InkVibesTattoo.Helpers;
using Tattoo.InkVibesTattoo.Models.Forms;
using Tattoo.InkVibesTattoo.Security;
using Radzen;
using Microsoft.AspNetCore.Components.WebAssembly.Services;
using System.Globalization;
using System.Text.RegularExpressions;
using Tattoo.InkVibesTattoo.Pages;

namespace Tattoo.InkVibesTattoo.Layout
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
            try
            {
                var savedLanguage = await JSRuntime.InvokeAsync<string>("localStorage.getItem", "selectedLanguage");
                if (string.IsNullOrEmpty(savedLanguage))
                {
                    await JSRuntime.InvokeVoidAsync("localStorage.setItem", "selectedLanguage", "en");
                }

                var result = await JSRuntime.InvokeAsync<string>("localStorage.getItem", "hasSeenInitView");
                hasSeenInitView = result == "true";
                var cultureInfo = new CultureInfo((string)savedLanguage);
                CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
                CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
            }
            catch (Exception ex)
            {
                // Manejar la excepción, quizás mostrar un mensaje al usuario
                //Console.WriteLine($"Error during initialization: {ex.Message}");
            }

            stateService.OnGlobalBasicValuesChanged += StateHasChanged;
            stateService.OnGlobalLanguageChanged += StateHasChanged;

        //    var savedLanguage = await JSRuntime.InvokeAsync<string>("localStorage.getItem", "selectedLanguage");
        //    if (string.IsNullOrEmpty(savedLanguage)) 
        //    {
        //        await JSRuntime.InvokeVoidAsync("localStorage.setItem", "selectedLanguage", "en");

        //    }
        //    var result = await JSRuntime.InvokeAsync<string>("localStorage.getItem", "hasSeenInitView");
        //    hasSeenInitView = result == "true";
        //stateService.OnGlobalBasicValuesChanged += StateHasChanged;
        //stateService.OnGlobalLanguageChanged += StateHasChanged;

        }

        private void NavigateToMain()
        {
            hasSeenInitView = true;
            JSRuntime.InvokeVoidAsync("localStorage.setItem", "hasSeenInitView", "true");
            JSRuntime.InvokeVoidAsync("localStorage.setItem", "cookieConsent", "accepted");
            JSRuntime.InvokeVoidAsync("hideCookieConsent");
            StateHasChanged();
        }

        private async Task OnClickBookNow()
        {
            ReservationDto data = await DialogService.OpenAsync<AddAppointmentPage>(LocalizationService.GetString("BookNow"),
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

        public void Dispose()
        {
            stateService.OnGlobalBasicValuesChanged -= StateHasChanged;
            stateService.OnGlobalLanguageChanged -= StateHasChanged;
        }
    }
}
