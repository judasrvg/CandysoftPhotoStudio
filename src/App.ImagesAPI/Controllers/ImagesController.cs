using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using App.ImagesAPI.Infrastructure;
using Microsoft.AspNetCore.WebUtilities;
using static System.Net.Mime.MediaTypeNames;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;

[Route("api/[controller]")]
[ApiController]
[DisableRequestSizeLimit]
public class ImagesController : ControllerBase
{
    private readonly FTPSettings _ftpSettings;

    public ImagesController(IOptions<FTPSettings> ftpSettings)
    {
        _ftpSettings = ftpSettings.Value;
    }
    [HttpPost("uploadSingle")]
    public async Task<IActionResult> UploadSingleImages(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No files uploaded");

        var uploadResults = new List<object>();
        Guid uniqueId = Guid.NewGuid();
        var webpFileName = $"img_{uniqueId}.webp";
        var filePath = $"images/{webpFileName}";

        try
        {
            using (var ftpStream = new MemoryStream())
            {
                // Load the image using ImageSharp
                using (var image = await SixLabors.ImageSharp.Image.LoadAsync(file.OpenReadStream()))
                {
                    // Convert the image to WebP and save it to the MemoryStream
                    var webpEncoder = new WebpEncoder();
                    await image.SaveAsync(ftpStream, webpEncoder);
                    ftpStream.Seek(0, SeekOrigin.Begin);

                    // Ensure the directory exists (optional)
                    await EnsureDirectoryExists("images");

                    // Upload the file with retries
                    var result = await UploadFileWithRetries(ftpStream, filePath);
                    var fileUrl = $"{_ftpSettings.Host}/images/{webpFileName}";
                    uploadResults.Add(new { file = webpFileName, result.status, result.reason, url = fileUrl });
                }
            }

            return Ok(uploadResults);
        }
        catch (ImageFormatException ex)
        {
            // Log the specific error related to image format
            // Log.Error("Image format error: " + ex.Message);
            return BadRequest("Invalid image format: " + ex.Message);
        }
        catch (Exception ex)
        {
            // Log any other error that might occur
            // Log.Error("Error uploading image: " + ex.Message);
            return StatusCode(500, "Internal server error: " + ex.Message);
        }
    }


    //[HttpPost("upload")]
    //public async Task<IActionResult> UploadImages(IFormFileCollection files)
    //{
    //    if (files == null || files.Count == 0)
    //        return BadRequest("No files uploaded");

    //    var uploadResults = new List<object>();

    //    foreach (var file in files)
    //    {
    //        if (file.Length == 0)
    //            continue;

    //        var filePath = $"images/{file.FileName}";

    //        using (var ftpStream = new MemoryStream())
    //        {
    //            await file.CopyToAsync(ftpStream);

    //            // Ensure the directory exists (optional)
    //            await EnsureDirectoryExists("images");

    //            // Upload the file with retries
    //            var result = await UploadFileWithRetries(ftpStream, filePath);
    //            var fileUrl = $"{_ftpSettings.Host}/images/{file.FileName}";
    //            uploadResults.Add(new { file = file.FileName, result.status, result.reason, url = fileUrl });
    //        }
    //    }

    //    return Ok(uploadResults);
    //}

    private async Task<(string status, string reason)> UploadFileWithRetries(MemoryStream ftpStream, string filePath, int maxRetries = 3, int delayMilliseconds = 1000)
    {
        int retries = 0;
        while (retries < maxRetries)
        {
            try
            {
                var ftpRequest = (FtpWebRequest)WebRequest.Create($"{_ftpSettings.Host}/{filePath}");
                ftpRequest.Method = WebRequestMethods.Ftp.UploadFile;
                ftpRequest.Credentials = new NetworkCredential(_ftpSettings.Username, _ftpSettings.Password);
                ftpRequest.UsePassive = true;
                ftpRequest.KeepAlive = false;

                using (var requestStream = ftpRequest.GetRequestStream())
                {
                    ftpStream.Position = 0;
                    await ftpStream.CopyToAsync(requestStream);
                }

                using (var response = (FtpWebResponse)ftpRequest.GetResponse())
                {
                    if (response.StatusCode == FtpStatusCode.ClosingData)
                    {
                        return ("Success", null);
                    }
                    else
                    {
                        return ("Failed", response.StatusDescription);
                    }
                }
            }
            catch (WebException ex)
            {
                var response = (FtpWebResponse)ex.Response;
                if (retries == maxRetries - 1)
                {
                    return ("Failed", response.StatusDescription);
                }
            }

            retries++;
            await Task.Delay(delayMilliseconds);
        }

        return ("Failed", "Maximum retry attempts exceeded");
    }

    private async Task EnsureDirectoryExists(string directory)
    {
        var createDirectoryRequest = (FtpWebRequest)WebRequest.Create($"{_ftpSettings.Host}/{directory}/");
        createDirectoryRequest.Method = WebRequestMethods.Ftp.MakeDirectory;
        createDirectoryRequest.Credentials = new NetworkCredential(_ftpSettings.Username, _ftpSettings.Password);
        createDirectoryRequest.UsePassive = true;
        createDirectoryRequest.KeepAlive = false;

        try
        {
            using (var response = (FtpWebResponse)createDirectoryRequest.GetResponse())
            {
                // Directory created successfully or already exists
            }
        }
        catch (WebException ex)
        {
            var response = (FtpWebResponse)ex.Response;
            if (response.StatusCode != FtpStatusCode.ActionNotTakenFileUnavailable)
            {
                throw;
            }
        }
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadImages(IFormFileCollection files)
    {
        if (files == null || files.Count == 0)
            return BadRequest("No files uploaded");

        var uploadResults = new List<object>();

        foreach (var file in files)
        {
            if (file.Length == 0)
                continue;

           //var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file.FileName);
        Guid uniqueId = Guid.NewGuid();
        var webpFileName = $"img_{uniqueId}.webp";
            var filePath = $"images/{webpFileName}";

            using (var ftpStream = new MemoryStream())
            {
                // Load the image using ImageSharp
                using (var image = await SixLabors.ImageSharp.Image.LoadAsync(file.OpenReadStream()))
                {
                    // Convert the image to WebP and save it to the MemoryStream
                    var webpEncoder = new WebpEncoder();
                    await image.SaveAsync(ftpStream, webpEncoder);
                    ftpStream.Seek(0, SeekOrigin.Begin);

                    // Ensure the directory exists (optional)
                    await EnsureDirectoryExists("images");

                    // Upload the file with retries
                    var result = await UploadFileWithRetries(ftpStream, filePath);
                    var fileUrl = $"{_ftpSettings.Host}/images/{webpFileName}";
                    uploadResults.Add(new { file = webpFileName, result.status, result.reason, url = fileUrl });
                }
            }
        }

        return Ok(uploadResults);
    }

    [HttpPost("Uploadimagesclient")]
    public async Task<IActionResult> Uploadimagesclient(IFormFileCollection files)
    {
        if (files == null || files.Count == 0)
            return BadRequest("No files uploaded");

        var uploadResults = new List<object>();

        foreach (var file in files)
        {
            if (file.Length == 0)
                continue;

            //var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file.FileName);
            Guid uniqueId = Guid.NewGuid();
            var webpFileName = $"img_{uniqueId}.webp";
            var filePath = $"imagesclient/{webpFileName}";

            using (var ftpStream = new MemoryStream())
            {
                // Load the image using ImageSharp
                using (var image = await SixLabors.ImageSharp.Image.LoadAsync(file.OpenReadStream()))
                {
                    // Convert the image to WebP and save it to the MemoryStream
                    var webpEncoder = new WebpEncoder();
                    await image.SaveAsync(ftpStream, webpEncoder);
                    ftpStream.Seek(0, SeekOrigin.Begin);

                    // Ensure the directory exists (optional)
                    await EnsureDirectoryExists("imagesclient");

                    // Upload the file with retries
                    var result = await UploadFileWithRetries(ftpStream, filePath);
                    var fileUrl = $"{_ftpSettings.Host}/imagesclient/{webpFileName}";
                    uploadResults.Add(new { file = webpFileName, result.status, result.reason, url = fileUrl });
                }
            }
        }

        return Ok(uploadResults);
    }
}
