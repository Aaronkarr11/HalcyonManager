using CommunityToolkit.Maui;
using HalcyonCore.Clients;
using HalcyonCore.Interfaces;
using HalcyonManager.Services;
using Maui.FixesAndWorkarounds;
using SkiaSharp.Views.Maui.Controls.Hosting;

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
