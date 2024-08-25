using System.Collections.Generic;
using System.Globalization;
using System.Resources;
using Microsoft.JSInterop;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Tattoo.InkVibesTattoo.Helpers;

public class LocalizationService
{
    private readonly IJSRuntime _jsRuntime;
    [Inject] private GlobalState stateService { get; set; }

    private readonly Dictionary<string, ResourceManager> _resourceManagers;
    private ResourceManager _currentResourceManager;

    public LocalizationService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;

        // Inicializar los ResourceManager para cada cultura
        _resourceManagers = new Dictionary<string, ResourceManager>
        {
            { "en", new ResourceManager("Tattoo.InkVibesTattoo.Resources.Resource", typeof(LocalizationService).Assembly) },
            { "es", new ResourceManager("Tattoo.InkVibesTattoo.Resources.ResourceEs", typeof(LocalizationService).Assembly) }
        };

        // Establecer el ResourceManager actual por defecto
        _currentResourceManager = _resourceManagers["en"];
    }

    public string GetString(string key)
    {
        return _currentResourceManager.GetString(key) ?? key;
    }

    public async Task SetCultureAsync(string culture)
    {
        // Normalizar el idioma a "es" si es una variante
        culture = NormalizeLanguage(culture);

        if (_resourceManagers.ContainsKey(culture))
        {
            _currentResourceManager = _resourceManagers[culture];
        }
        else
        {
            _currentResourceManager = _resourceManagers["en"];
        }

        // Guardar el idioma seleccionado en el almacenamiento local
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "selectedLanguage", culture);
        // Ajustar la cultura actual
        var cultureInfo = new CultureInfo(culture);
        CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
        CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
    }

    private string NormalizeLanguage(string culture)
    {
        return culture.StartsWith("es") ? "es" : culture;
    }
}
