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

    public partial class StoreProducts
    {
        [Inject] private IProductService _productService { get; set; }
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
            await LoadStoreProducts();
            ///TODO:Load StoreProducts list to
        }

        /// <summary>
        /// methods for inside grid edition
        /// </summary>
        RadzenDataList<ProductDto> _MerchandiseDataGridRef;
        RadzenDataList<ProductDto> _MerchandiseDataGridRefMobile;
        HashSet<ProductDto> _MerchandiseData = new();
        FormFilterGetTattoos _formFilterGetTattoos= new FormFilterGetTattoos();
        HashSet<ConfigValueDto> _tattooStylesData= new HashSet<ConfigValueDto>();
        ////TODO:combo StoreProducts for select a StoreProducts for X StoreProducts, multiple selection with all posible selection.
        //IEnumerable<ProductDto> StoreProducts; 
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
        private async Task LoadStoreProducts()
        {
            InvokeAsync(async () =>
            {
                var response = await _productService.GetAllProductsAsync();
                if (response.IsSuccess)
                {
                    var products = response.Data as IEnumerable<ProductDto>;

                    var parsedResponse = response?.Data as IEnumerable<ProductDto>;
                    _MerchandiseData = products?
                    .Where(p => p.ProductType == ProductType.Merchandise)
                    .ToHashSet() ?? new HashSet<ProductDto>();
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


        protected async Task<bool> AddStoreProducts(ProductDto StoreProducts)
        {
            var response = await _productService.AddProductAsync(StoreProducts);
            var InsertedStoreProducts = response.Data as ProductDto;
            if (response.IsSuccess && (InsertedStoreProducts != null))
            {
                //_MerchandiseData.Remove(StoreProducts);
                _MerchandiseData.Add(InsertedStoreProducts);
            }

            return HasSuccessResponse(response);
        }

        private bool _dialogOpen = false;

        private async Task OnChangeStyle()
        {
            await LoadStoreProducts();

        }

        private async Task ShowDialogVender(object args)
        {
            if (_dialogOpen)
            {
                return;
            }

            _dialogOpen = true;

            try
            {
                var products = _MerchandiseData.Where(x=>x.TotalBuy>0).ToList(); // Suponiendo que _MerchandiseData es de tipo IEnumerable<ProductDto>

                var response = await DialogService.OpenAsync<SellConfirmationDialog>($"Confirmación de Venta",
                    new Dictionary<string, object>() { { "Products", products } },
                    new DialogOptions() { Width = "780px", Resizable = false, Draggable = false });

                if (response == null || response == false)
                {
                    return;
                }

                var stockRequests = products.Select(p => new StockRequest
                {
                    Id = p.Id,
                    Quantity = p.TotalBuy,
                    Value = p.Price,
                    WithTransaction = true
                }).ToArray();

                var stockDecrement = new StockDecrement { Requests = stockRequests };

                var result = await _productService.DecrementStockAsync(stockDecrement);

                if (result.IsSuccess)
                {
                    _notificationService.Notify(NotificationSeverity.Success, "Venta completada", "La venta se ha realizado con éxito.", 3000);
                    await LoadStoreProducts();
                }
                else
                {
                    _notificationService.Notify(NotificationSeverity.Error, "Error en la venta", "Ocurrió un error al procesar la venta.", 3000);
                }
            }
            finally
            {
                _dialogOpen = false;
            }
        }


        //private async Task ShowDialogEditTattoo( ProductDto tattoo)
        //{
        //    if (_dialogOpen)
        //    {
        //        return;
        //    }

        //    _dialogOpen = true;

        //    try
        //    {
        //        var response = await DialogService.OpenAsync<EditTattooDialog>($"Edit Tattoo",
        //        new Dictionary<string, object>() { { "_tattooStyles", _tattooStylesData }, { "_tattooBase", tattoo } },
        //        new DialogOptions() { Width = "780px", Resizable = false, Draggable = false });

        //        if (response == null || !(response is ProductDto))
        //        {
        //            return;
        //        }
        //        _MerchandiseData.Remove(tattoo);
        //        _MerchandiseData.Add(response);
        //        _MerchandiseData = _MerchandiseData.OrderBy(x => x.Order).ToHashSet();
        //    }
        //    finally
        //    {
        //        _dialogOpen = false;
        //    }
        //}

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

        
        
        //protected async Task<bool> OnChangeTattooOrder(object args, long tattooId)
        //{
        //    var requestDto = new ProductDto { Id = tattooId, Order = (int)args };
        //    var response = await _productService.ReorderTattooAsync(requestDto);
        //    if (response.IsSuccess)
        //    {
        //        var parsedResponse = response?.Data as IEnumerable<ProductDto>;
        //        _MerchandiseData = parsedResponse?.OrderBy(t => t.Order).ToHashSet() ?? new();
        //    }
        //    return HasSuccessResponse(response);
        //}

        protected async Task<bool> DeleteStoreProducts(long id)
        {
            //var response = await _productService.DeleteTattooAsync(id);
            //if (HasSuccessResponse(response))
            //{
            //    if (!_MerchandiseData?.Any() ?? true)
            //    {
            //        _notificationService.Notify(NotificationSeverity.Success, "Nothing to drop", "Empty list", 4000);
            //        return false;
            //    }
            //    _MerchandiseData.Remove(_MerchandiseData!.First(c => c.Id == id));
            //    _notificationService.Notify(NotificationSeverity.Success, "Delete", "Success Operation", 4000);
            //}

            return true;
        }

        //public async Task OnClickTattoo(ProductDto tattoo)
        //{
        //    await ShowDialogEditTattoo(tattoo,tattoo);
        //}

        protected async Task ConfirmDelete(long id)
        {
            var confirm = await DialogService.Confirm("Sure you want to delete this?", "Confirmation", new ConfirmOptions { OkButtonText = "Yes", CancelButtonText = "No" });
            if (confirm.HasValue && confirm.Value)
            {
                await DeleteStoreProducts(id);
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
