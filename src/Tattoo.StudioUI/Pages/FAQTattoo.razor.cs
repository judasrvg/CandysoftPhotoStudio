using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using Radzen.Blazor.Rendering;
using Tattoo.StudioUI.Components.Layout;
using Tattoo.StudioUI.Models.Forms;
using Tattoo.StudioUI.Models.Forms.Enum;
using Tattoo.StudioUI.Models.Requests;
using Tattoo.StudioUI.Services;
using static System.Net.WebRequestMethods;

namespace Tattoo.StudioUI.Pages
{


    public partial class FAQTattoo
    {

        [Inject] private IConfigValueService _configValueService { get; set; }
        [Inject] private NotificationService NotificationService { get; set; }
        [Inject] private DialogService DialogService { get; set; }
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await LoadConfigValues();
            ///TODO:Load ConfigValue list to
        }

        /// <summary>
        /// methods for inside grid edition
        /// </summary>
        RadzenDataGrid<ConfigValueDto> _ConfigValuesDataGridRef;
        RadzenDataGrid<ConfigValueDto> _ConfigValuesDataGridRefMobile;
        HashSet<ConfigValueDto> _ConfigValuesData = new();
        ////TODO:combo ConfigValues for select a ConfigValue for X ConfigValue, multiple selection with all posible selection.
        //IEnumerable<ConfigValueDto> ConfigValues;

        DataGridEditMode editMode = DataGridEditMode.Single;
        private readonly CacheValueType _customValueType = CacheValueType.AskAnswerFAQ;

        ///End methods for inside grid edition

        private bool HasSuccessResponse(ResponseAdapterDto response)
        {
            if (!response.IsSuccess)
            {
                NotificationService.Notify(NotificationSeverity.Error, "Error: ", response.ErrorMessage, 4000);
                return false;
            }
            return true;
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

        /// <summary>
        /// Data Load and Actions
        /// </summary>
        private async Task LoadConfigValues()
        {

            InvokeAsync(async () =>
            {
                var response = await _configValueService.GetConfigValueAsync(_customValueType);
                if (response.IsSuccess)
                {
                    var parsedResponse = response?.Data as IEnumerable<ConfigValueDto>;
                    _ConfigValuesData = parsedResponse?.ToHashSet() ?? new();
                }
                else
                {
                    NotificationService.Notify(NotificationSeverity.Error, "Failed to Load", response.ErrorMessage, 4000);
                }

                // Close the dialog
                DialogService.Close();
                StateHasChanged();
            });

            await BusyDialog();
            StateHasChanged();


        }
    }

}
