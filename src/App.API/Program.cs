using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using System.IO.Compression;
using App.API.Foundation.Exceptions;
using App.Application.Abstractions;
using App.Application.Cache;
using App.Application.Mapper;
using App.Application.Services.Command;
using App.Application.Services.Query;
using App.Domain.Interfases;
using App.Infrastructure.Repositories;
using App.API.Foundation.Validations;
using NLog.Web;
using NLog;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;
using App.Application.DTOs;
using App.Application.Abstractions.Services;
using Microsoft.Extensions.DependencyInjection;
using App.Infrastructure;
using App.Application.Services;
using Autofac.Core;
using App.Application.Abstractions.Implementations;
using System;


var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
try
{
    logger.Debug("Iniciando aplicación");

    var builder = WebApplication.CreateBuilder(args);
    //var environment = builder.Environment;
    //builder.Configuration
    //.SetBasePath(environment.ContentRootPath)
    //.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    //.AddJsonFile($"appsettings.{environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    //.AddEnvironmentVariables();

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowSpecificOrigins",
            builder =>
            {
                builder//.WithOrigins("https://localhost:7257", "http://localhost:7290")
                       .AllowAnyHeader()
                       .AllowAnyMethod()
                       .AllowCredentials();
            });
    });

    // Agregar NLog a ASP.NET Core
    builder.Logging.ClearProviders();
    builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
    builder.Host.UseNLog(); // Esto agrega NLog al ASP.NET Core
                            // Cargar configuración
    var telegramSettings = builder.Configuration.GetSection("TelegramSettings").Get<TelegramSettings>();
        
    // Add services to the container
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    //builder.Services.AddSwaggerGen();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "App API", Version = "v1" });

        // Define los encabezados de seguridad
        c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
        {
            Description = "API Key needed to access the endpoints. X-Api-Key: {apiKey}, X-Timestamp: {timestamp}, X-Nonce: {nonce}",
            In = ParameterLocation.Header,
            Name = "X-Api-Key",
            Type = SecuritySchemeType.ApiKey,
            Scheme = "ApiKeyScheme"
        });

        c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "ApiKey"
                }
            },
            new List<string>()
        }
    });

        c.OperationFilter<AddRequiredHeadersOperationFilter>();
    });

    //var redisConnection = builder.Configuration.GetConnectionString("RedisConnection");
    //var multiplexer = ConnectionMultiplexer.Connect(redisConnection);

    //builder.Services.AddSingleton<IConnectionMultiplexer>(multiplexer);

    // Agregar StackExchangeRedisCache
    //builder.Services.AddStackExchangeRedisCache(options =>
    //{
    //    options.Configuration = redisConnection;
    //    options.InstanceName = "app_tattoos";
    //});

    // Registrar RedisCacheManager
    //builder.Services.AddSingleton<RedisCacheManager>();

    builder.Services.AddResponseCompression(opts =>
    {
        opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "application/octet-stream" });
    });
    
    builder.Services.Configure<BrotliCompressionProviderOptions>(options =>
    {
        options.Level = CompressionLevel.Fastest;
    });
    
    builder.Services.Configure<GzipCompressionProviderOptions>(options =>
    {
        options.Level = CompressionLevel.SmallestSize;
    });

    //FTP
    builder.Services.Configure<FTPSettings>(builder.Configuration.GetSection("FTPSettings"));

    builder.Services.AddAutoMapper(typeof(DeltaMappingProfile));

    // Registrar DbContext antes de otros servicios que lo usan

    //builder.Services.AddDbContext<SQLSDBContext>(options =>
    //{
    //    options.UseSqlServer(builder.Configuration.GetConnectionString("SQLServerConnection"));
    //});
    builder.Services.AddDbContext<SQLSDBContext>(options =>
    {
        // No necesitas configurar la cadena de conexión aquí, se manejará en OnConfiguring.
    }, ServiceLifetime.Scoped);

    //Generic Repository and UOW
    builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
    builder.Services.AddScoped(typeof(IReadRepository<>), typeof(ReadRepository<>));
    builder.Services.AddScoped(typeof(IWriteRepository<>), typeof(WriteRepository<>));

    ////Handler
    builder.Services.AddScoped<IEventPublisher, EventPublisher>();
    builder.Services.AddScoped<IEventHandler<AppointmentCreatedEvent>, AppointmentCreatedEventHandler>();

    //// Services
    builder.Services.AddScoped<IConfigValueQueryService, ConfigValueQueryService>();
    builder.Services.AddScoped<IConfigValueCommandService, ConfigValueCommandService>();
    builder.Services.AddScoped<IReservationQueryService, ReservationQueryService>();
    builder.Services.AddScoped<IReservationCommandService, ReservationCommandService>();
    builder.Services.AddScoped<ITattooQueryService, TattooQueryService>();
    builder.Services.AddScoped<IProductQueryService, ProductQueryService>();
    builder.Services.AddScoped<IProductCommandService, ProductCommandService>();
    builder.Services.AddScoped<ITransactionQueryService, TransactionQueryService>();
    builder.Services.AddScoped<ITransactionCommandService, TransactionCommandService>();
    builder.Services.AddScoped<ITattooCommandService, TattooCommandService>();
    builder.Services.AddTransient<INotificationCommandServices, NotificationCommandServices>(provider =>
    {
        return new NotificationCommandServices(telegramSettings.BotToken, telegramSettings.ChatId);
    });
    builder.Services.AddScoped<IEmailService, EmailService>();
    builder.Services.AddScoped<IEmailTemplateService, EmailTemplateService>();

    //if (!builder.Environment.IsProduction())
    //{
        //builder.Services.AddHostedService<AppointmentNotificationService>();
    //}

    var app = builder.Build();
    // app.UseHttpLogging();
    logger.Debug($"Configuration Time: {DateTime.UtcNow} UTC");

    foreach (var c in builder.Configuration.AsEnumerable())
    {
        logger.Debug(c.Key + " = " + c.Value);
    }
    app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true) // Allow any origin
    .AllowCredentials());               // Allow credentials
    app.UseResponseCompression();

    //app.UseCors("AllowSpecificOrigins");

    app.UseMiddleware<ErrorHandlingMiddleware>();

    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        //app.UseSwaggerUI();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "App API v1"));
    }
    else 
    {
        app.UseSwagger();
        //app.UseSwaggerUI();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "App API v1"));
    }

    //app.UseHttpsRedirection();
    app.UseApiKeyMiddleware();
    //app.UseAuthorization();
    app.MapControllers();

    // Initialize the database
    //using (var scope = app.Services.CreateScope())
    //{
    //    var dbContext = scope.ServiceProvider.GetRequiredService<SQLSDBContext>();
    //    dbContext.Database.Migrate();
    //}

    app.Run();
}
catch (Exception ex)
{
    // NLog: captura la excepción justo antes de cerrar la aplicación
    logger.Error(ex, "Error inesperado al iniciar la aplicación");
throw;
}
finally
{
    // Asegúrate de limpiar los recursos de NLog al cerrar la aplicación
    NLog.LogManager.Shutdown();
}