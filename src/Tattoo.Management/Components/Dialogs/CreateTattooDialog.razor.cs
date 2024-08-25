
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using System;
using System.Net.Http.Headers;
using System.Text.Json;
using Tattoo.Management.Helpers;
using Tattoo.Management.Models.Forms;
using Tattoo.Management.Models.Requests;
using Tattoo.Management.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Tattoo.Management.Components.Dialogs
{
    public partial class CreateTattooDialog
    {
        //Declaration

        [Inject] private ITattooService _tattooService { get; set; }
        [Inject] private IImagesService _imageService { get; set; }
        [Inject] private GlobalState stateService { get; set; }

        [Parameter] public IEnumerable<ConfigValueDto> _tattooStyles { get; set; }
        [Parameter] public int _countTattoos{ get; set; }
        public int CountGamesToChange { get; set; }
        public string btnType { get; set; } = string.Empty;
        public bool _isLoading { get; set; } = false;
        NotificationMessage notificationMessage = new NotificationMessage();
        public TattooDto _creationForm = new TattooDto();

        protected override async Task OnInitializedAsync()
        {
            _isLoading = false;
            await base.OnInitializedAsync();
        }

        RadzenUpload upload;

        int progress;
        bool showProgress;
        bool showComplete;
        string completionMessage;
        bool cancelUpload = false;
        List<(string FileName, string Url, string ThumbnailFileName)> uploadedFilePaths = new List<(string, string, string)>();

        void TrackProgress(UploadProgressArgs args)
        {
            showProgress = true;
            showComplete = false;
            progress = args.Progress;

            // cancel upload
            args.Cancel = cancelUpload;

            // reset cancel flag
            cancelUpload = false;
        }

        async void OnComplete(UploadCompleteEventArgs args)
        {
            if (args.JsonResponse != null)
            {
                var jsonResponse = args.JsonResponse.RootElement;
                if (jsonResponse.ValueKind == JsonValueKind.Array)
                {
                    uploadedFilePaths = jsonResponse.EnumerateArray()
                        .Select(e => (e.GetProperty("file").GetString(), e.GetProperty("url").GetString(), e.GetProperty("thumbnailFileName").GetString()))
                        .ToList();
                }
            }
            else
            {
                _notificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Error,
                    Detail = "Image upload failed",
                    Duration = 4000
                });
                _isLoading = false;
                return;
            }

            // Set the image path to the first uploaded file's URL (or handle accordingly)
            if (uploadedFilePaths.Any())
            {
                _creationForm.ImagePath = $"{Configuration["ImagesBaseUrl"]}/imagesshared/images/{uploadedFilePaths.First().FileName}";
                _creationForm.MiniatureImagePath = $"{Configuration["ImagesBaseUrl"]}/imagesshared/images/{uploadedFilePaths.First().ThumbnailFileName}";
            }

            await SubmitTattoo();
        }

        void CancelUpload()
        {
            cancelUpload = true;
        }

        void OnChange(UploadChangeEventArgs args)
        {
            // Handle file selection change if needed
        }

        void OnProgress(UploadProgressArgs args)
        {
            // Progress tracking code
        }

        async void OnSubmit()
        {
            _isLoading = true;

            if (!ValidateGlobalFields())
            {
                _isLoading = false;
                return;
            }

            // Ensure the files are uploaded first
            if (upload.HasValue)
            {
                await upload.Upload();
            }
            else
            {
                await SubmitTattoo();
            }
        }

        async Task SubmitTattoo()
        {
            _creationForm.Order = _countTattoos+1;
            var response = await _tattooService.AddTattooAsync(_creationForm);
            if (response.IsSuccess)
            {
                _notificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Success,
                    Detail = "Operation completed successfully",
                    Duration = 4000
                });
                _creationForm.Id = response.Data.Id;
                _dialogService.Close(_creationForm);
            }
            else
            {
                _notificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Error,
                    Detail = response.ErrorMessage,
                    Duration = 4000
                });
            }

            _isLoading = false;
        }

        bool ValidateGlobalFields()
        {
            if (string.IsNullOrWhiteSpace(_creationForm.TattooName))
            {
                return false;
            }
            return true;
        }

        void OnInvalidSubmit()
        {
            // Handle invalid form submission
        }

        private void OnCancelClick(object args)
        {
            _dialogService.Close(null);
        }
    }
}
