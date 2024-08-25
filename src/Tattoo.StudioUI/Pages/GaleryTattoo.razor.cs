using System;
using System.Collections.Generic;
using System.Text.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using Radzen.Blazor.Rendering;
using Tattoo.StudioUI.Components.Dialogs;
using Tattoo.StudioUI.Components.Layout;
using Tattoo.StudioUI.Helpers;
using Tattoo.StudioUI.Models.Dialogs;
using Tattoo.StudioUI.Models.Forms;
using Tattoo.StudioUI.Models.Forms.Enum;
using Tattoo.StudioUI.Models.Requests;
using Tattoo.StudioUI.Services;
using static System.Net.WebRequestMethods;

namespace Tattoo.StudioUI.Pages
{

    public partial class GaleryTattoo
    {
        [Inject] private ITattooService _galeryTattooService { get; set; }
        [Inject] private IConfigValueService _configValueService { get; set; }
        [Inject] private NotificationService _notificationService { get; set; }
        [Inject] private DialogService DialogService { get; set; }
        [Inject] private NavigationManager? NavigationManager { get; set; }
        [Inject] private GlobalState stateService { get; set; }
        [Inject] private DialogHelper DialogHelper { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await LoadGaleryTattoos();
            await LoadTattooStyles();
            var favoritesJson = await JSRuntime.InvokeAsync<string>("localStorage.getItem", "inkfavorites_tattoos") ?? "";

            var favorites = new HashSet<long>();
            if (!string.IsNullOrWhiteSpace(favoritesJson))
            {
                favorites = JsonSerializer.Deserialize<HashSet<long>>(favoritesJson) ?? new HashSet<long>();
                _favorites = favorites;
            }
            ///TODO:Load GaleryTattoo list to
        }

        /// <summary>
        /// methods for inside grid edition
        /// </summary>
        RadzenDataList<TattooDto> _GaleryTattoosDataGridRef;
        RadzenDataList<TattooDto> _GaleryTattoosDataGridRefMobile;
        HashSet<TattooDto> _GaleryTattoosData = new();
        FormFilterGetTattoos _formFilterGetTattoos= new FormFilterGetTattoos();
        HashSet<ConfigValueDto> _tattooStylesData= new HashSet<ConfigValueDto>();
        HashSet<long> _favorites = new HashSet<long>();
        ////TODO:combo GaleryTattoos for select a GaleryTattoo for X GaleryTattoo, multiple selection with all posible selection.
        //IEnumerable<TattooDto> GaleryTattoos; 
        private bool IsConsumedDialog = false;
        private bool moreTattoos = false;
        DataGridEditMode editMode = DataGridEditMode.Single;


        private bool HasSuccessResponse(ResponseAdapterDto response)
        {
            if (!response.IsSuccess)
            {
                _notificationService.Notify(NotificationSeverity.Error, "Error: ", response.ErrorMessage, 4000);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Data Load and Actions
        /// </summary>
        private async Task LoadGaleryTattoos()
        {
            InvokeAsync(async () =>
            {
                var response = await _galeryTattooService.GetTattoosAsync(_formFilterGetTattoos);
                if (response.IsSuccess)
                {
                    var parsedResponse = response?.Data as IEnumerable<TattooDto>;
                    _GaleryTattoosData = parsedResponse?.ToHashSet() ?? new();
                    if (_GaleryTattoosData.Any())
                    {
                        _GaleryTattoosData = _GaleryTattoosData.OrderBy(x => x.Id).ToHashSet();

                    }

                }
                else
                {
                    _notificationService.Notify(NotificationSeverity.Error, "Failed to Load", response.ErrorMessage, 4000);
                }

                // Close the dialog
                DialogService.Close();
                StateHasChanged();
            });

            await BusyDialog();
            StateHasChanged();
            
        }

        private async Task LoadTattooStyles()
        {
            var response = await _configValueService.GetConfigValueAsync(CacheValueType.TattooStyle);

            if (response.IsSuccess)
            {
                var parsedResponse = response?.Data as IEnumerable<ConfigValueDto>;
                _tattooStylesData = parsedResponse?.ToHashSet() ?? new();
            }
            else
            {
                _notificationService.Notify(NotificationSeverity.Error, "Failed to Load", response.ErrorMessage, 4000);
            }
        }


        private bool _dialogOpen = false;

        private async Task OnChangeStyle()
        {
            await LoadGaleryTattoos();

        }

        private async Task OnClickIncrementRating(long tattooId) 
        {
            if (_favorites.Contains(tattooId))
            {
                return;
            }
            var favoritesJson = await JSRuntime.InvokeAsync<string>("localStorage.getItem", "inkfavorites_tattoos") ?? "";

            var favorites = new HashSet<long>();
            if (!string.IsNullOrWhiteSpace(favoritesJson))
            {
                favorites = JsonSerializer.Deserialize<HashSet<long>>(favoritesJson) ?? new HashSet<long>();
            }
            var response= await _galeryTattooService.IncrementRatingAsync(tattooId);
            if (response.IsSuccess)
            {
                
                    if (!favorites.Contains(tattooId))
                    {
                        favorites.Add(tattooId);
                        favoritesJson = JsonSerializer.Serialize(favorites);
                        await JSRuntime.InvokeVoidAsync("localStorage.setItem", "inkfavorites_tattoos", favoritesJson);
                        _favorites = favorites;
                        await LoadGaleryTattoos() ;
                }

            }

        }


        private async Task ShowDialogZoomImage(object args, string imagePath)
        {
            if (_dialogOpen)
            {
                return;
            }

            _dialogOpen = true;

            try
            {
                await ShowNewDialog<ImageZoomDialog>(args, new DialogHeaders()
                {
                    dialogParams = new Dictionary<string, object>() {
                {"_imagePath",  imagePath}
            }
                });
            }
            finally
            {
                _dialogOpen = false;
            }
        }

        //**********************Dialog Generic***************

        private async Task ShowNewDialog<T>(object arg, DialogHeaders dialogHeaders, string width = "780px") where T : ComponentBase
        {
            using (var dialogBuilder = DialogHelper.CreateDialog<T>(DialogService))
            {
                await dialogBuilder
                    .WithOptions(new DialogOptions() { CloseDialogOnOverlayClick = false, Width = width })
                    .WithHeaderParams(dialogHeaders)
                    .OnClose(async (args) =>
                    {
                        if (stateService.GlobalIsSuccessDialogRequest || stateService.GlobalIsPartialSuccessDialogRequest)
                        {
                            await InvokeAsync(async () =>
                            {
                                _notificationService.Notify(new NotificationMessage
                                {
                                    Severity = stateService.GlobalIsPartialSuccessDialogRequest ? NotificationSeverity.Error : NotificationSeverity.Success,
                                    Detail = stateService.GlobalDialogRequestMessage,
                                    Duration = 4000
                                });
                                stateService.GlobalIsSuccessDialogRequest = false;
                                stateService.GlobalIsPartialSuccessDialogRequest = false;
                                StateHasChanged();
                            });
                        }
                    })
                    .ShowAsync();
            }
        }


        private async Task BusyDialog()
        {
            await DialogService.OpenAsync<BusyLoading>(
                "",
                null,
                new DialogOptions()
                {
                    ShowTitle = false,
                    Style = "display: contents;background-color: #ff000000 !important;border-radius: 50px 50px 50px 50px!important;min-height:auto;min-width:auto;width:auto",
                    CloseDialogOnEsc = false
                }
            );
        }

    }

}
