using System;
using System.Collections.Generic;
using System.Text.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using Radzen.Blazor.Rendering;
using Tattoo.InkVibesTattoo.Components.Dialogs;
using Tattoo.InkVibesTattoo.Components.Layout;
using Tattoo.InkVibesTattoo.Helpers;
using Tattoo.InkVibesTattoo.Models.Dialogs;
using Tattoo.InkVibesTattoo.Models.Forms;
using Tattoo.InkVibesTattoo.Models.Forms.Enum;
using Tattoo.InkVibesTattoo.Models.Requests;
using Tattoo.InkVibesTattoo.Services;
using static System.Net.WebRequestMethods;

namespace Tattoo.InkVibesTattoo.Pages
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
        private string classRating= "";
        private bool ActiveRatingProcess= false;
        private bool isDataLoaded = false;
        private string selectedLanguage = "en";

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            stateService.OnGlobalLanguageChanged += HandleLanguageChanged;
            var savedLanguage = stateService.GlobalLanguage;
            if (!string.IsNullOrEmpty(savedLanguage))
            {
                selectedLanguage = savedLanguage;
            }

            var favoritesJson = await JSRuntime.InvokeAsync<string>("localStorage.getItem", "inkfavorites_tattoos") ?? "";
            var favorites = new HashSet<long>();
            if (!string.IsNullOrWhiteSpace(favoritesJson))
            {
                favorites = JsonSerializer.Deserialize<HashSet<long>>(favoritesJson) ?? new HashSet<long>();
                _favorites = favorites;
            }

            await LoadGaleryTattoos();
            //await LoadTattooStyles();

            // Iniciar el temporizador para establecer isDataLoaded a true después de 6 segundos
            var timer = new System.Timers.Timer(6000);
            timer.Elapsed += (sender, e) => SetDataLoaded();
            timer.AutoReset = false;
            timer.Start();
        }

        private void HandleLanguageChanged()
        {
            var savedLanguage = stateService.GlobalLanguage;

            //var savedLanguage = await JSRuntime.InvokeAsync<string>("localStorage.getItem", "selectedLanguage");
            //if (string.IsNullOrEmpty(savedLanguage))
            //{
            //    savedLanguage = "en";
            //    await JSRuntime.InvokeVoidAsync("localStorage.setItem", "selectedLanguage", savedLanguage);
            //}

            //selectedLanguage = NormalizeLanguage(savedLanguage);
            

            if (!string.IsNullOrEmpty(savedLanguage))
            {
                selectedLanguage = savedLanguage;
                LocalizationService.SetCultureAsync(selectedLanguage);
            }
            isDataLoaded = false;
            StateHasChanged();
            isDataLoaded = true;

        }

        private void SetDataLoaded()
        {
            isDataLoaded = true;
            InvokeAsync(StateHasChanged); // Actualizar la UI
        }

        protected override bool ShouldRender()
        {
            // Añade la lógica para determinar si se debe renderizar el componente
            // Por ejemplo, puedes usar una variable booleana para controlar esto
            return !isDataLoaded;
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
                var response = await _galeryTattooService.GetPrioritizedTattoosAsync(_formFilterGetTattoos);
                if (response.IsSuccess)
                {
                    var parsedResponse = response?.Data as IEnumerable<TattooDto>;
                    _GaleryTattoosData = parsedResponse?.ToHashSet() ?? new();
                    if (_GaleryTattoosData.Any())
                    {
                        _GaleryTattoosData = _GaleryTattoosData.Select(t=> { var fav = t; fav.IsFavorite = _favorites.Contains(t.Id); return fav; }).OrderBy(x => x.Order).ToHashSet();

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


        public void Dispose()
        {
            stateService.OnGlobalLanguageChanged -= HandleLanguageChanged;
        }

        private async Task OnClickViewMore()
        {
            if (_dialogOpen)
            {
                return;
            }

            _dialogOpen = true;

            try
            {
                await ShowNewDialog<MoreTattooDialog>(null, new DialogHeaders()
                {
                    dialogParams = new Dictionary<string, object>() {
            }
                }, "100vw!important", "more-tattoo-layout");
            }
            finally
            {
                _dialogOpen = false;
            }

        }
        

        private async Task OnClickIncrementRating(long tattooId) 
        {

            if (ActiveRatingProcess || _favorites.Contains(tattooId))
            {
                return;
            }

            ActiveRatingProcess = true;
            try {
            
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
                    var galeryIncremented = new List<TattooDto>();
                    foreach (var tattoo in _GaleryTattoosData)
                    {
                        if (tattoo.Id == tattooId)
                        {
                            tattoo.Rating++;
                            tattoo.IsFavorite = true;
                        }
                        galeryIncremented.Add(tattoo);
                    }
                        _GaleryTattoosData = galeryIncremented.OrderBy(x => x.Order).ToHashSet();
                       
                        isDataLoaded = false;

                        //await LoadGaleryTattoos() ;
                    }
                    ActiveRatingProcess = false;

            }
            else 
            {
                ActiveRatingProcess = false;
            }
            }
            catch 
            {
                ActiveRatingProcess = false;

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
                },"100vw!important","zoom-up");
            }
            finally
            {
                _dialogOpen = false;
            }
        }

        //**********************Dialog Generic***************

        private async Task ShowNewDialog<T>(object arg, DialogHeaders dialogHeaders, string width = "780px", string cssClass = "") where T : ComponentBase
        {
            using (var dialogBuilder = DialogHelper.CreateDialog<T>(DialogService))
            {
                await dialogBuilder
                    .WithOptions(new DialogOptions() { CloseDialogOnOverlayClick = false, Width = width, CssClass = cssClass })
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
