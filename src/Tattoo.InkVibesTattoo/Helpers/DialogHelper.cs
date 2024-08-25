using Microsoft.AspNetCore.Components;
using Radzen;
using Tattoo.InkVibesTattoo.Models.Dialogs;

namespace Tattoo.InkVibesTattoo.Helpers
{
    public class DialogHelper
    {
        public DialogBuilder<T> CreateDialog<T>(DialogService dialogService) where T : ComponentBase
        {
            return new DialogBuilder<T>(dialogService);
        }
    }

    public class DialogBuilder<T> : IDisposable where T : ComponentBase
    {
        private readonly DialogService _dialogService;
        private DialogOptions _dialogOptions = new DialogOptions();
        private DialogHeaders _dialogHeaders = new DialogHeaders();
        private Action<dynamic> _onCloseAction;

        public DialogBuilder(DialogService dialogService)
        {
            _dialogService = dialogService;
        }

        public DialogBuilder<T> OnClose(Action<dynamic> onCloseAction)
        {
            _onCloseAction = onCloseAction;
            _dialogService.OnClose -= _onCloseAction;
            _dialogService.OnClose += _onCloseAction;
            return this;
        }

        public DialogBuilder<T> WithOptions(DialogOptions dialogOptions)
        {
            _dialogOptions = dialogOptions;
            return this;
        }

        public DialogBuilder<T> WithHeaderParams(DialogHeaders dialogHeader)
        {
            _dialogHeaders = dialogHeader;
            return this;
        }

        public void Dispose()
        {
            if (_onCloseAction != null)
            {
                _dialogService.OnClose -= _onCloseAction;
                _onCloseAction = null; // Opcional, para limpiar la referencia
            }
        }

        public async Task ShowAsync()
        {
            await _dialogService.OpenAsync<T>(_dialogHeaders.DialogTitle, _dialogHeaders.dialogParams, _dialogOptions);
        }
    }
}
