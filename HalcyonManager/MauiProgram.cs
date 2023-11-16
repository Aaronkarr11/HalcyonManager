using Microsoft.Extensions.Logging;
using HalcyonCore.Clients;
using HalcyonManager.Services;
using CommunityToolkit.Maui;
using SkiaSharp.Views.Maui.Controls.Hosting;
using Maui.FixesAndWorkarounds;
using HalcyonCore.Interfaces;

namespace HalcyonManager;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseSkiaSharp(true)
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureMauiWorkarounds()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.Services.AddSingleton<IAlertService, AlertService>();
        builder.Services.AddTransient<IHalcyonManagementClient, HalcyonManagementClient>();
        return builder.Build();
    }
}
