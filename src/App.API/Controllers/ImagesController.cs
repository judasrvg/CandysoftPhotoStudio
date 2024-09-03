using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;
using App.Application.DTOs;
using App.API.Foundation.Validations;
using System.Collections.Generic;
using System.Threading.Tasks;
using SixLabors.ImageSharp.Metadata.Profiles.Exif;

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
    [AllowAnonymousApiKey]
    public async Task<IActionResult> UploadSingleImages(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No files uploaded");

        var uploadResults = new List<object>();
        Guid uniqueId = Guid.NewGuid();
        var webpFileName = $"img_{uniqueId}.webp";
        var thumbnailFileName = $"thumb_{uniqueId}.webp";
        var filePath = $"images/{webpFileName}";
        var thumbnailPath = $"images/{thumbnailFileName}";

        try
        {
            using (var ftpStream = new MemoryStream())
            using (var thumbnailStream = new MemoryStream())
            {
                // Load the image using ImageSharp
                using (var image = await Image.LoadAsync(file.OpenReadStream()))
                {
                    // Correct orientation based on EXIF data
                    image.Mutate(x => x.AutoOrient());

                    // Create and save the full-size WebP image
                    var webpEncoder = new WebpEncoder();
                    await image.SaveAsync(ftpStream, webpEncoder);
                    ftpStream.Seek(0, SeekOrigin.Begin);

                    // Calculate the size for the thumbnail (30% of the original size)
                    var thumbnailSize = new Size((int)(image.Width * 0.2), (int)(image.Height * 0.2));

                    // Create and save the thumbnail WebP image
                    var thumbnail = image.Clone(ctx => ctx.Resize(new ResizeOptions
                    {
                        Mode = ResizeMode.Max,
                        Size = thumbnailSize
                    }));
                    await thumbnail.SaveAsync(thumbnailStream, webpEncoder);
                    thumbnailStream.Seek(0, SeekOrigin.Begin);

                    // Ensure the directory exists (optional)
                    await EnsureDirectoryExists("images");

                    // Upload the full-size image
                    var result = await UploadFileWithRetries(ftpStream, filePath);
                    var fileUrl = $"{_ftpSettings.Host}/images/{webpFileName}";

                    // Upload the thumbnail image
                    var thumbnailResult = await UploadFileWithRetries(thumbnailStream, thumbnailPath);
                    var thumbnailUrl = $"{_ftpSettings.Host}/images/{thumbnailFileName}";

                    uploadResults.Add(new { file = webpFileName, result.status, result.reason, url = $"/images/{webpFileName}", thumbnailFileName });
                }
            }

            return Ok(uploadResults);
        }
        catch (ImageFormatException ex)
        {
            return BadRequest("Invalid image format: " + ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error: " + ex.Message);
        }
    }

    [HttpPost("upload")]
    [AllowAnonymousApiKey]
    public async Task<IActionResult> UploadImages(IFormFileCollection files)
    {
        if (files == null || files.Count == 0)
            return BadRequest("No files uploaded");

        var uploadResults = new List<object>();

        foreach (var file in files)
        {
            if (file.Length == 0)
                continue;

            Guid uniqueId = Guid.NewGuid();
            var webpFileName = $"img_{uniqueId}.webp";
            var thumbnailFileName = $"thumb_{uniqueId}.webp";
            var filePath = $"images/{webpFileName}";
            var thumbnailPath = $"images/{thumbnailFileName}";

            using (var ftpStream = new MemoryStream())
            using (var thumbnailStream = new MemoryStream())
            {
                // Load the image using ImageSharp
                using (var image = await Image.LoadAsync(file.OpenReadStream()))
                {
                    // Correct orientation based on EXIF data
                    image.Mutate(x => x.AutoOrient());

                    // Create and save the full-size WebP image
                    var webpEncoder = new WebpEncoder();
                    await image.SaveAsync(ftpStream, webpEncoder);
                    ftpStream.Seek(0, SeekOrigin.Begin);

                    // Calculate the size for the thumbnail (30% of the original size)
                    var thumbnailSize = new Size((int)(image.Width * 0.2), (int)(image.Height * 0.2));

                    // Create and save the thumbnail WebP image
                    var thumbnail = image.Clone(ctx => ctx.Resize(new ResizeOptions
                    {
                        Mode = ResizeMode.Max,
                        Size = thumbnailSize
                    }));
                    await thumbnail.SaveAsync(thumbnailStream, webpEncoder);
                    thumbnailStream.Seek(0, SeekOrigin.Begin);

                    // Ensure the directory exists (optional)
                    await EnsureDirectoryExists("images");

                    // Upload the full-size image
                    var result = await UploadFileWithRetries(ftpStream, filePath);
                    var fileUrl = $"{_ftpSettings.Host}/images/{webpFileName}";

                    // Upload the thumbnail image
                    var thumbnailResult = await UploadFileWithRetries(thumbnailStream, thumbnailPath);
                    var thumbnailUrl = $"{_ftpSettings.Host}/images/{thumbnailFileName}";

                    uploadResults.Add(new { file = webpFileName, result.status, result.reason, url = $"/images/{webpFileName}", thumbnailFileName });
                }
            }
        }

        return Ok(uploadResults);
    }

    [HttpPost("Uploadimagesclient")]
    [AllowAnonymousApiKey]
    public async Task<IActionResult> Uploadimagesclient(IFormFileCollection files)
    {
        if (files == null || files.Count == 0)
            return BadRequest("No files uploaded");

        var uploadResults = new List<object>();

        foreach (var file in files)
        {
            if (file.Length == 0)
                continue;

            Guid uniqueId = Guid.NewGuid();
            var webpFileName = $"img_{uniqueId}.webp";
            var thumbnailFileName = $"thumb_{uniqueId}.webp";
            var filePath = $"imagesclient/{webpFileName}";
            var thumbnailPath = $"imagesclient/{thumbnailFileName}";

            using (var ftpStream = new MemoryStream())
            using (var thumbnailStream = new MemoryStream())
            {
                // Load the image using ImageSharp
                using (var image = await Image.LoadAsync(file.OpenReadStream()))
                {
                    // Correct orientation based on EXIF data
                    image.Mutate(x => x.AutoOrient());

                    // Create and save the full-size WebP image
                    var webpEncoder = new WebpEncoder();
                    await image.SaveAsync(ftpStream, webpEncoder);
                    ftpStream.Seek(0, SeekOrigin.Begin);

                    // Calculate the size for the thumbnail (30% of the original size)
                    var thumbnailSize = new Size((int)(image.Width * 0.2), (int)(image.Height * 0.2));

                    // Create and save the thumbnail WebP image
                    var thumbnail = image.Clone(ctx => ctx.Resize(new ResizeOptions
                    {
                        Mode = ResizeMode.Max,
                        Size = thumbnailSize
                    }));
                    await thumbnail.SaveAsync(thumbnailStream, webpEncoder);
                    thumbnailStream.Seek(0, SeekOrigin.Begin);

                    // Ensure the directory exists (optional)
                    await EnsureDirectoryExists("imagesclient");

                    // Upload the full-size image
                    var result = await UploadFileWithRetries(ftpStream, filePath);
                    var fileUrl = $"{_ftpSettings.Host}/imagesclient/{webpFileName}";

                    // Upload the thumbnail image
                    var thumbnailResult = await UploadFileWithRetries(thumbnailStream, thumbnailPath);
                    var thumbnailUrl = $"{_ftpSettings.Host}/imagesclient/{thumbnailFileName}";

                    uploadResults.Add(new { file = webpFileName, result.status, result.reason, url = $"/imagesclient/{webpFileName}", thumbnailFileName });
                }
            }
        }

        return Ok(uploadResults);
    }

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
}
