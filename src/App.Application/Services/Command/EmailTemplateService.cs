using App.Application.Abstractions.Services;
using RazorLight;
using System.IO;
using System.Threading.Tasks;

namespace App.Application.Services
{
    public class EmailTemplateService : IEmailTemplateService
    {
        private readonly RazorLightEngine _razorLightEngine;

        public EmailTemplateService()
        {
            _razorLightEngine = new RazorLightEngineBuilder()
                .UseFileSystemProject(Path.Combine(Directory.GetCurrentDirectory(), "EmailTemplates"))
                .UseMemoryCachingProvider()
                .Build();
        }

        public async Task<string> GetTemplateAsync(string templateName, object model)
        {
            var templatePath = Path.Combine("EmailTemplates", templateName + ".cshtml");
            if (!File.Exists(templatePath))
                throw new FileNotFoundException("Template not found", templatePath);

            var templateKey = templateName + ".cshtml";
            var result = await _razorLightEngine.CompileRenderAsync(templateKey, model);
            return result;
        }
    }
}
