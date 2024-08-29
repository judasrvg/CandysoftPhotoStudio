using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using Radzen.Blazor.Rendering;
using Tattoo.Management.Components.Dialogs;
using Tattoo.Management.Components.Layout;
using Tattoo.Management.Helpers;
using Tattoo.Management.Models.Dialogs;
using Tattoo.Management.Models.Forms;
using Tattoo.Management.Models.Forms.Enum;
using Tattoo.Management.Models.Requests;
using Tattoo.Management.Services;
using static System.Net.WebRequestMethods;

namespace Tattoo.Management.Pages
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
        [Inject] private string? ImagesBaseUrl { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await LoadGaleryTattoos();
            await LoadTattooStyles();
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
        ////TODO:combo GaleryTattoos for select a GaleryTattoo for X GaleryTattoo, multiple selection with all posible selection.
        //IEnumerable<TattooDto> GaleryTattoos; 
        private bool IsConsumedDialog = false;

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
                    _GaleryTattoosData = parsedResponse?.OrderBy(t=>t.Order).ToHashSet() ?? new();
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
            //var response = await _configValueService.GetConfigValueAsync(CacheValueType.TattooStyle);

            //if (response.IsSuccess)
            //{
            //    var parsedResponse = response?.Data as IEnumerable<ConfigValueDto>;
            //    _tattooStylesData = parsedResponse?.ToHashSet() ?? new();
            //}
            //else
            //{
            //    _notificationService.Notify(NotificationSeverity.Error, "Failed to Load", response.ErrorMessage, 4000);
            //}
        }

        protected async Task<bool> AddGaleryTattoo(TattooDto GaleryTattoo)
        {
            var response = await _galeryTattooService.AddTattooAsync(GaleryTattoo);
            var InsertedGaleryTattoo = response.Data as TattooDto;
            if (response.IsSuccess && (InsertedGaleryTattoo != null))
            {
                //_GaleryTattoosData.Remove(GaleryTattoo);
                _GaleryTattoosData.Add(InsertedGaleryTattoo);
            }

            return HasSuccessResponse(response);
        }

        private bool _dialogOpen = false;

        private async Task OnChangeStyle()
        {
            await LoadGaleryTattoos();

        }

        private async Task ShowDialogAddTattoo(object args)
        {
            if (_dialogOpen)
            {
                return;
            }

            _dialogOpen = true;

            try
            {
               var response =  await DialogService.OpenAsync<CreateTattooDialog>($"Add new Tattoo",
               new Dictionary<string, object>() { { "_tattooStyles", _tattooStylesData }, { "_countTattoos", _GaleryTattoosData.Count()} },
               new DialogOptions() { Width = "780px", Resizable = false, Draggable = false });

                if (response == null || !(response is TattooDto))
                {
                    return;
                }
                _GaleryTattoosData.Add(response);
            }
            finally
            {
                _dialogOpen = false;
            }
        }

        private async Task ShowDialogEditTattoo( TattooDto tattoo)
        {
            if (_dialogOpen)
            {
                return;
            }

            _dialogOpen = true;

            try
            {
                var response = await DialogService.OpenAsync<EditTattooDialog>($"Edit Tattoo",
                new Dictionary<string, object>() { { "_tattooStyles", _tattooStylesData }, { "_tattooBase", tattoo } },
                new DialogOptions() { Width = "780px", Resizable = false, Draggable = false });

                if (response == null || !(response is TattooDto))
                {
                    return;
                }
                _GaleryTattoosData.Remove(tattoo);
                _GaleryTattoosData.Add(response);
                _GaleryTattoosData = _GaleryTattoosData.OrderBy(x => x.Order).ToHashSet();
            }
            finally
            {
                _dialogOpen = false;
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


        protected async Task<bool> EditGaleryTattoo(TattooDto GaleryTattoo)
        {
            var response = await _galeryTattooService.UpdateTattooAsync(GaleryTattoo);
            return HasSuccessResponse(response);
        }

        
        protected async Task<bool> OnChangeTattooOrder(object args, long tattooId)
        {
            var requestDto = new TattooDto { Id = tattooId, Order = (int)args };
            var response = await _galeryTattooService.ReorderTattooAsync(requestDto);
            if (response.IsSuccess)
            {
                var parsedResponse = response?.Data as IEnumerable<TattooDto>;
                _GaleryTattoosData = parsedResponse?.OrderBy(t => t.Order).ToHashSet() ?? new();
            }
            return HasSuccessResponse(response);
        }

        protected async Task<bool> DeleteGaleryTattoo(long id)
        {
            var response = await _galeryTattooService.DeleteTattooAsync(id);
            if (HasSuccessResponse(response))
            {
                if (!_GaleryTattoosData?.Any() ?? true)
                {
                    _notificationService.Notify(NotificationSeverity.Success, "Nothing to drop", "Empty list", 4000);
                    return false;
                }
                _GaleryTattoosData.Remove(_GaleryTattoosData!.First(c => c.Id == id));
                _notificationService.Notify(NotificationSeverity.Success, "Delete", "Success Operation", 4000);
            }

            return true;
        }

        //public async Task OnClickTattoo(TattooDto tattoo)
        //{
        //    await ShowDialogEditTattoo(tattoo,tattoo);
        //}

        protected async Task ConfirmDelete(long id)
        {
            var confirm = await DialogService.Confirm("Sure you want to delete this?", "Confirmation", new ConfirmOptions { OkButtonText = "Yes", CancelButtonText = "No" });
            if (confirm.HasValue && confirm.Value)
            {
                await DeleteGaleryTattoo(id);
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
