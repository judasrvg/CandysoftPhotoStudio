using System;
using System.Collections.Generic;
using System.Text.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using Radzen.Blazor.Rendering;
using Tattoo.InkVibesTattoo.Components.Dialogs;
using Tattoo.InkVibesTattoo.Components.Layout;
using Tattoo.InkVibesTattoo.Helpers;
using Tattoo.InkVibesTattoo.Models.Dialogs;
using Tattoo.InkVibesTattoo.Models.Forms;
using Tattoo.InkVibesTattoo.Models.Forms.Enum;
using Tattoo.InkVibesTattoo.Models.Requests;
using Tattoo.InkVibesTattoo.Services;
using static System.Net.WebRequestMethods;

namespace Tattoo.InkVibesTattoo.Pages
{

    public partial class ServicesPackage
    {
        private List<Offer> NinosOffers = new List<Offer>
    {
        new Offer { Name = "Mínimo", Price = 7500, Icon = "child_care", IsHighlighted = false, Features = new List<string>
            {
                "·5 fotos impresas de 8x10",
                "·Lienzo 40 x 60 cm (Impresión de lona sobre bastidor)",
                "·Estilista (maquillaje y peinado)",
                "·2 escenografías (vestuarios y accesorios)",
                "·Edición profesional",
                "·DVD con las fotos digitales (editadas digitales)"
            }
        },
        new Offer { Name = "Básico", Price = 12500, Icon = "child_friendly", IsHighlighted = false, Features = new List<string>
            {
                "·10 fotos impresas de 8x10",
                "·Lienzo 40 x 60 cm (Impresión de lona sobre bastidor)",
                "·Álbum personalizado de 8x10 (o revista personalizada)",
                "·Estilista (maquillaje y peinado)",
                "·3 escenografías (vestuarios y accesorios)",
                "·Edición profesional",
                "·DVD con las fotos digitales (editadas digitales)"
            }
        },
        new Offer { Name = "Estándar", Price = 16500, Icon = "toys", IsHighlighted = true, Features = new List<string>
            {
                "·15 fotos impresas de 8x12",
                "·Lienzo 50 x 75 cm (Impresión de lona sobre bastidor)",
                "·Álbum personalizado de 8x12 (o revista personalizada)",
                "·Estilista (maquillaje y peinado)",
                "·4 o 5 escenografías (vestuarios y accesorios)",
                "·Edición profesional",
                "·DVD con las fotos digitales (editadas digitales)"
            }
        },
        new Offer { Name = "Premium", Price = 24500, Icon = "baby_changing_station", IsHighlighted = false, Features = new List<string>
            {
                "·20 fotos impresas en Photobook de 8x12",
                "·Lienzo 100 x 60 cm (Impresión de lona sobre bastidor)",
                "·Cofre para Photobook",
                "·Estilista (maquillaje y peinado)",
                "·6 escenografías (vestuarios y accesorios)",
                "·Vestuarios Familiar en Combinación",
                "·Edición profesional",
                "·DVD con las fotos digitales (editadas digitales)"
            }
        }
    };

        private List<Offer> EmbarazadasOffers = new List<Offer>
    {
        new Offer { Name = "Mínimo", Price = 7500, Icon = "pregnant_woman", IsHighlighted = false, Features = new List<string>
            {
                "·5 fotos impresas de 8x10",
                "·Lienzo 40 x 60 cm (Impresión de lona sobre bastidor)",
                "·Estilista (maquillaje y peinado)",
                "·2 escenografías (vestuarios y accesorios)",
                "·Edición profesional",
                "·DVD con las fotos digitales (editadas digitales)"
            }
        },
        new Offer { Name = "Básico", Price = 12500, Icon = "local_hospital", IsHighlighted = false, Features = new List<string>
            {
                "·10 fotos impresas de 8x10",
                "·Lienzo 40 x 60 cm (Impresión de lona sobre bastidor)",
                "·Álbum personalizado de 8x10 (o revista personalizada)",
                "·Estilista (maquillaje y peinado)",
                "·3 escenografías (vestuarios y accesorios)",
                "·Edición profesional",
                "·DVD con las fotos digitales (editadas digitales)"
            }
        },
        new Offer { Name = "Estándar", Price = 16500, Icon = "family_restroom", IsHighlighted = true, Features = new List<string>
            {
                "·15 fotos impresas de 8x12",
                "·Lienzo 50 x 75 cm (Impresión de lona sobre bastidor)",
                "·Álbum personalizado de 8x12 (o revista personalizada)",
                "·Estilista (maquillaje y peinado)",
                "·4 o 5 escenografías (vestuarios y accesorios)",
                "·Edición profesional",
                "·DVD con las fotos digitales (editadas digitales)"
            }
        },
        new Offer { Name = "Premium", Price = 24500, Icon = "baby", IsHighlighted = false, Features = new List<string>
            {
                "·20 fotos impresas en Photobook de 8x12",
                "·Lienzo 100 x 60 cm (Impresión de lona sobre bastidor)",
                "·Cofre para Photobook",
                "·Estilista (maquillaje y peinado)",
                "·6 escenografías (vestuarios y accesorios)",
                "·Vestuarios Familiar en Combinación",
                "·Edición profesional",
                "·DVD con las fotos digitales (editadas digitales)"
            }
        }
    };

        private List<Offer> QuinceanerasOffers = new List<Offer>
    {
        new Offer { Name = "Mínimo", Price = 7500, Icon = "cake", IsHighlighted = false, Features = new List<string>
            {
                "·5 fotos impresas de 8x10",
                "·Lienzo 40 x 60 cm (Impresión de lona sobre bastidor)",
                "·Estilista (maquillaje y peinado)",
                "·2 escenografías (vestuarios y accesorios)",
                "·Edición profesional",
                "·DVD con las fotos digitales (editadas digitales)"
            }
        },
        new Offer { Name = "Básico", Price = 12500, Icon = "favorite", IsHighlighted = false, Features = new List<string>
            {
                "·10 fotos impresas de 8x10",
                "·Lienzo 40 x 60 cm (Impresión de lona sobre bastidor)",
                "·Álbum personalizado de 8x10 (o revista personalizada)",
                "·Estilista (maquillaje y peinado)",
                "·3 escenografías (vestuarios y accesorios)",
                "·Edición profesional",
                "·DVD con las fotos digitales (editadas digitales)"
            }
        },
        new Offer { Name = "Estándar", Price = 16500, Icon = "event", IsHighlighted = true, Features = new List<string>
            {
                "·15 fotos impresas de 8x12",
                "·Lienzo 50 x 75 cm (Impresión de lona sobre bastidor)",
                "·Álbum personalizado de 8x12 (o revista personalizada)",
                "·Estilista (maquillaje y peinado)",
                "·4 o 5 escenografías (vestuarios y accesorios)",
                "·Edición profesional",
                "·DVD con las fotos digitales (editadas digitales)"
            }
        },
        new Offer { Name = "Premium", Price = 24500, Icon = "local_florist", IsHighlighted = false, Features = new List<string>
            {
                "·20 fotos impresas en Photobook de 8x12",
                "·Lienzo 100 x 60 cm (Impresión de lona sobre bastidor)",
                "·Cofre para Photobook",
                "·Estilista (maquillaje y peinado)",
                "·6 escenografías (vestuarios y accesorios)",
                "·Vestuarios Familiar en Combinación",
                "·Edición profesional",
                "·DVD con las fotos digitales (editadas digitales)"
            }
        }
    };

        private List<Offer> BodasOffers = new List<Offer>
    {
        new Offer { Name = "Mínimo", Price = 7500, Icon = "ring_volume", IsHighlighted = false, Features = new List<string>
            {
                "·5 fotos impresas de 8x10",
                "·Lienzo 40 x 60 cm (Impresión de lona sobre bastidor)",
                "·Estilista (maquillaje y peinado)",
                "·2 escenografías (vestuarios y accesorios)",
                "·Edición profesional",
                "·DVD con las fotos digitales (editadas digitales)"
            }
        },
        new Offer { Name = "Básico", Price = 12500, Icon = "group", IsHighlighted = false, Features = new List<string>
            {
                "·10 fotos impresas de 8x10",
                "·Lienzo 40 x 60 cm (Impresión de lona sobre bastidor)",
                "·Álbum personalizado de 8x10 (o revista personalizada)",
                "·Estilista (maquillaje y peinado)",
                "·3 escenografías (vestuarios y accesorios)",
                "·Edición profesional",
                "·DVD con las fotos digitales (editadas digitales)"
            }
        },
        new Offer { Name = "Estándar", Price = 16500, Icon = "emoji_people", IsHighlighted = true, Features = new List<string>
            {
                "·15 fotos impresas de 8x12",
                "·Lienzo 50 x 75 cm (Impresión de lona sobre bastidor)",
                "·Álbum personalizado de 8x12 (o revista personalizada)",
                "·Estilista (maquillaje y peinado)",
                "·4 o 5 escenografías (vestuarios y accesorios)",
                "·Edición profesional",
                "·DVD con las fotos digitales (editadas digitales)"
            }
        },
        new Offer { Name = "Premium", Price = 24500, Icon = "mood", IsHighlighted = false, Features = new List<string>
            {
                "·20 fotos impresas en Photobook de 8x12",
                "·Lienzo 100 x 60 cm (Impresión de lona sobre bastidor)",
                "·Cofre para Photobook",
                "·Estilista (maquillaje y peinado)",
                "·6 escenografías (vestuarios y accesorios)",
                "·Vestuarios Familiar en Combinación",
                "·Edición profesional",
                "·DVD con las fotos digitales (editadas digitales)"
            }
        }
    };

        private List<Offer> CasualOffers = new List<Offer>
    {
        new Offer { Name = "Mínimo", Price = 7500, Icon = "weekend", IsHighlighted = false, Features = new List<string>
            {
                "·5 fotos impresas de 8x10",
                "·Lienzo 40 x 60 cm (Impresión de lona sobre bastidor)",
                "·Estilista (maquillaje y peinado)",
                "·2 escenografías (vestuarios y accesorios)",
                "·Edición profesional",
                "·DVD con las fotos digitales (editadas digitales)"
            }
        },
        new Offer { Name = "Básico", Price = 12500, Icon = "local_dining", IsHighlighted = false, Features = new List<string>
            {
                "·10 fotos impresas de 8x10",
                "·Lienzo 40 x 60 cm (Impresión de lona sobre bastidor)",
                "·Álbum personalizado de 8x10 (o revista personalizada)",
                "·Estilista (maquillaje y peinado)",
                "·3 escenografías (vestuarios y accesorios)",
                "·Edición profesional",
                "·DVD con las fotos digitales (editadas digitales)"
            }
        },
        new Offer { Name = "Estándar", Price = 16500, Icon = "loyalty", IsHighlighted = false, Features = new List<string>
            {
                "·15 fotos impresas de 8x12",
                "·Lienzo 50 x 75 cm (Impresión de lona sobre bastidor)",
                "·Álbum personalizado de 8x12 (o revista personalizada)",
                "·Estilista (maquillaje y peinado)",
                "·4 o 5 escenografías (vestuarios y accesorios)",
                "·Edición profesional",
                "·DVD con las fotos digitales (editadas digitales)"
            }
        },
        new Offer { Name = "Premium", Price = 24500, Icon = "star", IsHighlighted = true, Features = new List<string>
            {
                "·20 fotos impresas en Photobook de 8x12",
                "·Lienzo 100 x 60 cm (Impresión de lona sobre bastidor)",
                "·Cofre para Photobook",
                "·Estilista (maquillaje y peinado)",
                "·6 escenografías (vestuarios y accesorios)",
                "·Vestuarios Familiar en Combinación",
                "·Edición profesional",
                "·DVD con las fotos digitales (editadas digitales)"
            }
        }
    };

        [Inject] private ITattooService _Serviceservice { get; set; }
        [Inject] private IConfigValueService _configValueService { get; set; }
        [Inject] private NotificationService _notificationService { get; set; }
        [Inject] private DialogService DialogService { get; set; }
        [Inject] private NavigationManager? NavigationManager { get; set; }
        [Inject] private GlobalState stateService { get; set; }
        [Inject] private DialogHelper DialogHelper { get; set; }
        private string classRating = "";
        private bool ActiveRatingProcess = false;
        private bool isDataLoaded = false;
        private string selectedLanguage = "es";

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            stateService.OnGlobalLanguageChanged += HandleLanguageChanged;
            var savedLanguage = stateService.GlobalLanguage;
            if (!string.IsNullOrEmpty(savedLanguage))
            {
                selectedLanguage = savedLanguage;
            }

            //var favoritesJson = await JSRuntime.InvokeAsync<string>("localStorage.getItem", "inkfavorites_tattoos") ?? "";
            //var favorites = new HashSet<long>();
            //if (!string.IsNullOrWhiteSpace(favoritesJson))
            //{
            //    favorites = JsonSerializer.Deserialize<HashSet<long>>(favoritesJson) ?? new HashSet<long>();
            //    _favorites = favorites;
            //}

            //await LoadServices();
            //await LoadTattooStyles();

            // Iniciar el temporizador para establecer isDataLoaded a true después de 6 segundos
            //var timer = new System.Timers.Timer(6000);
            //timer.Elapsed += (sender, e) => SetDataLoaded();
            //timer.AutoReset = false;
            //timer.Start();
        }

        private void HandleLanguageChanged()
        {
            var savedLanguage = stateService.GlobalLanguage;

            //var savedLanguage = await JSRuntime.InvokeAsync<string>("localStorage.getItem", "selectedLanguage");
            //if (string.IsNullOrEmpty(savedLanguage))
            //{
            //    savedLanguage = "en";
            //    await JSRuntime.InvokeVoidAsync("localStorage.setItem", "selectedLanguage", savedLanguage);
            //}

            //selectedLanguage = NormalizeLanguage(savedLanguage);


            //if (!string.IsNullOrEmpty(savedLanguage))
            //{
            //    selectedLanguage = savedLanguage;
            //    LocalizationService.SetCultureAsync(selectedLanguage);
            //}
            //isDataLoaded = false;
            //StateHasChanged();
            //isDataLoaded = true;

        }

        //private void SetDataLoaded()
        //{
        //    isDataLoaded = true;
        //    InvokeAsync(StateHasChanged); // Actualizar la UI
        //}

        //protected override bool ShouldRender()
        //{
        //    // Añade la lógica para determinar si se debe renderizar el componente
        //    // Por ejemplo, puedes usar una variable booleana para controlar esto
        //    return !isDataLoaded;
        //}

        /// <summary>
        /// methods for inside grid edition
        /// </summary>
        RadzenDataList<TattooDto> _ServicesDataGridRef;
        RadzenDataList<TattooDto> _ServicesDataGridRefMobile;
        HashSet<TattooDto> _ServicesData = new();
        FormFilterGetTattoos _formFilterGetTattoos = new FormFilterGetTattoos();
        HashSet<ConfigValueDto> _tattooStylesData = new HashSet<ConfigValueDto>();
        HashSet<long> _favorites = new HashSet<long>();
        ////TODO:combo Services for select a GaleryTattoo for X GaleryTattoo, multiple selection with all posible selection.
        //IEnumerable<TattooDto> Services; 
        private bool IsConsumedDialog = false;
        private bool moreTattoos = false;
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
        private async Task LoadServices()
        {
            InvokeAsync(async () =>
            {
                var response = await _Serviceservice.GetPrioritizedTattoosAsync(_formFilterGetTattoos);
                if (response.IsSuccess)
                {
                    var parsedResponse = response?.Data as IEnumerable<TattooDto>;
                    _ServicesData = parsedResponse?.ToHashSet() ?? new();
                    if (_ServicesData.Any())
                    {
                        _ServicesData = _ServicesData.Select(t => { var fav = t; fav.IsFavorite = _favorites.Contains(t.Id); return fav; }).OrderBy(x => x.Order).ToHashSet();

                    }

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

        private bool _dialogOpen = false;

        public void Dispose()
        {
            stateService.OnGlobalLanguageChanged -= HandleLanguageChanged;
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
        private int _activeTabIndex = 0;
        void OnChange()
        {
            //_activeTabIndex = index;
            StateHasChanged();
        }
    }

}
