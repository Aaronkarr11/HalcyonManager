#if WINDOWS
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Windows.Graphics;
#endif
using HalcyonSoft.Interfaces;
using HalcyonSoft.Clients;
using HalcyonManager.Services;
using HalcyonSoft.SharedEntities;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;


namespace HalcyonManager;

public partial class App : Application
{
    public HttpClient Client { get; set; }

    public static IServiceProvider _services;
    public static IAlertService _alertSvc;
    const int WindowWidth = 1000;
    const int WindowHeight = 800;

    public App(IServiceProvider provider)
    {
        InitializeComponent();
        Microsoft.Maui.Handlers.WindowHandler.Mapper.AppendToMapping(nameof(IWindow), (handler, view) =>
        {
#if WINDOWS
            var mauiWindow = handler.VirtualView;
            var nativeWindow = handler.PlatformView;
            nativeWindow.Activate();
            IntPtr windowHandle = WinRT.Interop.WindowNative.GetWindowHandle(nativeWindow);
            WindowId windowId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(windowHandle);
            AppWindow appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(windowId);
            appWindow.Resize(new SizeInt32(WindowWidth, WindowHeight));
#endif
        });
        Client = new HttpClient();
        _services = provider;
        Client.Timeout = TimeSpan.FromSeconds(10);
        var appService = new HalcyonManagementClient(Client);
        _alertSvc = _services.GetService<IAlertService>();
        DependencyService.RegisterSingleton<IHalcyonManagementClient>(appService);
        MainPage = new AppShell();

        LiveCharts.Configure(config =>
        config
        .AddSkiaSharp()
        .AddDefaultMappers()
        .AddLightTheme()
        .HasMap<LineGraphModelItem>((completed, point) =>
        {
            // use the city Population property as the primary value
            point.PrimaryValue = (int)completed.TotalCompleted;
            // and the index of the city in our cities array as the secondary value
            point.SecondaryValue = point.Context.Index;
        }));
    }
}
