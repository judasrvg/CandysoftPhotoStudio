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

    public partial class ServicesPackage
    {
        private List<ConfigValueDto> NinosOffers = new List<ConfigValueDto>();

        private List<ConfigValueDto> EmbarazadasOffers = new List<ConfigValueDto>();

        private List<ConfigValueDto> QuinceanerasOffers = new List<ConfigValueDto>();

        private List<ConfigValueDto> BodasOffers = new List<ConfigValueDto>();

        private List<ConfigValueDto> CasualOffers = new List<ConfigValueDto>();
        private List<ConfigValueDto> IndividualOffers = new List<ConfigValueDto>();

        [Inject] private ITattooService _Serviceservice { get; set; }
        [Inject] private IConfigValueService _configValueService { get; set; }
        [Inject] private NotificationService _notificationService { get; set; }
        [Inject] private DialogService DialogService { get; set; }
        [Inject] private NavigationManager? NavigationManager { get; set; }
        [Inject] private GlobalState stateService { get; set; }
        [Inject] private DialogHelper DialogHelper { get; set; }
        private string classRating = "";
        private bool ActiveRatingProcess = false;
        private bool isDataLoaded = false;
        private string selectedLanguage = "es";

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            stateService.OnGlobalLanguageChanged += HandleLanguageChanged;
            var savedLanguage = stateService.GlobalLanguage;
            if (!string.IsNullOrEmpty(savedLanguage))
            {
                selectedLanguage = savedLanguage;
            }
            await LoadConfigValues();
            
        }

        private void HandleLanguageChanged()
        {
            var savedLanguage = stateService.GlobalLanguage;
                        
        }

        private async Task LoadConfigValues()
        {
            InvokeAsync(async () =>
            {
                var response = await _configValueService.GetOfferConfigValuesAsync();
                if (response.IsSuccess)
                {
                    var parsedResponse = response?.Data as IEnumerable<ConfigValueDto>;
                    var _ConfigValuesData = parsedResponse?.ToHashSet() ?? new HashSet<ConfigValueDto>();

                    if (_ConfigValuesData != null)
                    {
                        // Limpiar listas antes de llenarlas
                        NinosOffers.Clear();
                        EmbarazadasOffers.Clear();
                        QuinceanerasOffers.Clear();
                        BodasOffers.Clear();
                        CasualOffers.Clear();
                        IndividualOffers.Clear();

                        // Llenar las listas de acuerdo al tipo de oferta
                        foreach (var data in _ConfigValuesData)
                        {
                            switch (data.ValueType)
                            {
                                case CacheValueType.OfferChild:
                                    NinosOffers.Add(data);
                                    break;
                                case CacheValueType.OfferPegnant:
                                    EmbarazadasOffers.Add(data);
                                    break;
                                case CacheValueType.Offer15:
                                    QuinceanerasOffers.Add(data);
                                    break;
                                case CacheValueType.OfferWedding:
                                    BodasOffers.Add(data);
                                    break;
                                case CacheValueType.OfferCasual:
                                    CasualOffers.Add(data);
                                    break;
                                case CacheValueType.OfferIndividual:
                                    IndividualOffers.Add(data);
                                    break;
                                default:
                                    // Manejo de casos no esperados si es necesario
                                    break;
                            }
                        }
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


        //private void SetDataLoaded()
        //{
        //    isDataLoaded = true;
        //    InvokeAsync(StateHasChanged); // Actualizar la UI
        //}

        //protected override bool ShouldRender()
        //{
        //    // Añade la lógica para determinar si se debe renderizar el componente
        //    // Por ejemplo, puedes usar una variable booleana para controlar esto
        //    return !isDataLoaded;
        //}

        /// <summary>
        /// methods for inside grid edition
        /// </summary>
        RadzenDataList<TattooDto> _ServicesDataGridRef;
        RadzenDataList<TattooDto> _ServicesDataGridRefMobile;
        HashSet<TattooDto> _ServicesData = new();
        FormFilterGetTattoos _formFilterGetTattoos = new FormFilterGetTattoos();
        HashSet<ConfigValueDto> _tattooStylesData = new HashSet<ConfigValueDto>();
        HashSet<long> _favorites = new HashSet<long>();
        ////TODO:combo Services for select a GaleryTattoo for X GaleryTattoo, multiple selection with all posible selection.
        //IEnumerable<TattooDto> Services; 
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
        
        private bool _dialogOpen = false;

        public void Dispose()
        {
            stateService.OnGlobalLanguageChanged -= HandleLanguageChanged;
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
        private int _activeTabIndex = 0;
        void OnChange()
        {
            //_activeTabIndex = index;
            StateHasChanged();
        }
    }

}
