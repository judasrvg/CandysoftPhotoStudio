using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using Radzen.Blazor.Rendering;
using Tattoo.Management.Components.Layout;
using Tattoo.Management.Models.Forms;
using Tattoo.Management.Models.Forms.Enum;
using Tattoo.Management.Models.Requests;
using Tattoo.Management.Services;
using static System.Net.WebRequestMethods;

namespace Tattoo.Management.Pages
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
        List<ConfigValueDto> ConfigValuesToInsert = new List<ConfigValueDto>();
        List<ConfigValueDto> ConfigValuesToUpdate = new List<ConfigValueDto>();

        void Reset()
        {
            ConfigValuesToInsert.Clear();
            ConfigValuesToUpdate.Clear();
        }

        void Reset(ConfigValueDto ConfigValue)
        {
            ConfigValuesToInsert.Remove(ConfigValue);
            ConfigValuesToUpdate.Remove(ConfigValue);
        }


        async Task EditRow(ConfigValueDto ConfigValue)
        {
            if (editMode == DataGridEditMode.Single && ConfigValuesToInsert.Count() > 0)
            {
                Reset();
            }

            ConfigValuesToUpdate.Add(ConfigValue);
            await _ConfigValuesDataGridRef.EditRow(ConfigValue);
        }

        async Task OnUpdateRow(ConfigValueDto ConfigValue)
        {
            Reset(ConfigValue);

            //dbContext.Update(ConfigValue);

            //dbContext.SaveChanges();
            //UPDATE ConfigValue
            var ConfigValueEdit = _ConfigValuesData.FirstOrDefault(c => c.Value == ConfigValue.Value);
            bool hasSucess = await EditConfigValue(ConfigValueEdit);
            if (hasSucess)
            {
                NotificationService.Notify(NotificationSeverity.Success, "Edition", "Success Operation", 4000);
            }

        }

        async Task SaveRow(ConfigValueDto ConfigValue)
        {
            await _ConfigValuesDataGridRef.UpdateRow(ConfigValue);
        }

        void CancelEdit(ConfigValueDto ConfigValue)
        {
            Reset(ConfigValue);

            _ConfigValuesDataGridRef.CancelEditRow(ConfigValue);

            //var ConfigValueEntry = dbContext.Entry(ConfigValue);
            //if (ConfigValueEntry.State == EntityState.Modified)
            //{
            //    ConfigValueEntry.CurrentValues.SetValues(ConfigValueEntry.OriginalValues);
            //    ConfigValueEntry.State = EntityState.Unchanged;
            //}
            //Cancel Edit edition
        }

        async Task DeleteRow(ConfigValueDto ConfigValue)
        {
            Reset(ConfigValue);
            var ConfigValueEdit = _ConfigValuesData.FirstOrDefault(c => c.Value == ConfigValue.Value);

            if (ConfigValueEdit != null)
            {
                //dbContext.Remove<Order>(ConfigValue);

                //dbContext.SaveChanges();
                //delete ConfigValue

                bool hasSucess = await ConfirmDelete(ConfigValueEdit!.Id);
                if (!hasSucess) return;
                await _ConfigValuesDataGridRef.Reload();
            }
            else
            {
                _ConfigValuesDataGridRef.CancelEditRow(ConfigValue);
                await _ConfigValuesDataGridRef.Reload();
            }
        }

        async Task InsertRow()
        {
            if (editMode == DataGridEditMode.Single)
            {
                Reset();
            }

            var order = new ConfigValueDto();
            order.ValueType = _customValueType;
            ConfigValuesToInsert.Add(order);
            await _ConfigValuesDataGridRef.InsertRow(order);
        }

        async Task OnCreateRow(ConfigValueDto ConfigValue)
        {
            InvokeAsync(async () =>
            {
                bool hasSucess = await AddConfigValue(ConfigValue);
                if (!hasSucess)
                {
                    return;
                }
                NotificationService.Notify(NotificationSeverity.Success, "Creation", "Success Operation", 4000);
                ConfigValuesToInsert.Remove(ConfigValue);

                // Close the dialog
                DialogService.Close();
                StateHasChanged();
            });

            await BusyDialog();
            StateHasChanged();
            //CreateConfigValue
            
            //StateHasChanged();
        }
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


        protected async Task<bool> AddConfigValue(ConfigValueDto ConfigValue)
        {
            var response = await _configValueService.AddConfigValueAsync(ConfigValue);
            var InsertedConfigValue = response.Data as ConfigValueDto;
            if (response.IsSuccess && (InsertedConfigValue != null))
            {
                //_ConfigValuesData.Remove(ConfigValue);
                _ConfigValuesData.Add(InsertedConfigValue);
            }

            return HasSuccessResponse(response);
        }

        protected async Task<bool> EditConfigValue(ConfigValueDto ConfigValue)
        {
            var response = await _configValueService.UpdateConfigValueAsync(ConfigValue);
            return HasSuccessResponse(response);
        }

        protected async Task<bool> DeleteConfigValue(long id)
        {
            var response = await _configValueService.DeleteConfigValueAsync(id, _customValueType);
            if (HasSuccessResponse(response))
            {
                if (!_ConfigValuesData?.Any() ?? true)
                {
                    NotificationService.Notify(NotificationSeverity.Success, "Nothing to drop", "Empty list", 4000);
                    return false;
                }
                _ConfigValuesData.Remove(_ConfigValuesData!.First(c => c.Id == id));
                NotificationService.Notify(NotificationSeverity.Success, "Delete", "Success Operation", 4000);
            }

            return true;
        }


        protected async Task<bool> ConfirmDelete(long id)
        {
            var confirm = await DialogService.Confirm("Sure you want to delete this?", "Confirmation", new ConfirmOptions { OkButtonText = "Yes", CancelButtonText = "No" });
            if (confirm.HasValue && confirm.Value)
            {
                return await DeleteConfigValue(id);
            }
            return false;
        }


        ///End DataLoad and Actions
        ///

        ///*****************MobileMethods*****************
        async Task EditRowMobile(ConfigValueDto ConfigValue)
        {
            if (editMode == DataGridEditMode.Single && ConfigValuesToInsert.Count() > 0)
            {
                Reset();
            }

            ConfigValuesToUpdate.Add(ConfigValue);
            await _ConfigValuesDataGridRefMobile.EditRow(ConfigValue);
        }

        async Task SaveRowMobile(ConfigValueDto ConfigValue)
        {
            await _ConfigValuesDataGridRefMobile.UpdateRow(ConfigValue);
        }

        void CancelEditMobile(ConfigValueDto ConfigValue)
        {
            Reset(ConfigValue);

            _ConfigValuesDataGridRefMobile.CancelEditRow(ConfigValue);

            //var ConfigValueEntry = dbContext.Entry(ConfigValue);
            //if (ConfigValueEntry.State == EntityState.Modified)
            //{
            //    ConfigValueEntry.CurrentValues.SetValues(ConfigValueEntry.OriginalValues);
            //    ConfigValueEntry.State = EntityState.Unchanged;
            //}
            //Cancel Edit edition
        }

        async Task DeleteRowMobile(ConfigValueDto ConfigValue)
        {
            Reset(ConfigValue);
            var ConfigValueEdit = _ConfigValuesData.FirstOrDefault(c => c.Value == ConfigValue.Value);

            if (ConfigValueEdit != null)
            {
                //dbContext.Remove<Order>(ConfigValue);

                //dbContext.SaveChanges();
                //delete ConfigValue

                bool hasSucess = await ConfirmDelete(ConfigValueEdit!.Id);
                if (!hasSucess) return;
                await _ConfigValuesDataGridRefMobile.Reload();
            }
            else
            {
                _ConfigValuesDataGridRef.CancelEditRow(ConfigValue);
                await _ConfigValuesDataGridRefMobile.Reload();
            }
        }

        async Task InsertRowMobile()
        {
            if (editMode == DataGridEditMode.Single)
            {
                Reset();
            }

            var order = new ConfigValueDto();
            order.ValueType = _customValueType;
            ConfigValuesToInsert.Add(order);
            await _ConfigValuesDataGridRefMobile.InsertRow(order);
        }
    }

}
