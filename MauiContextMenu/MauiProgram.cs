#if IOS
using MauiContextMenu.Platforms.iOS.CustomRenderers;
using MauiContextMenu.Platforms.iOS.Handler;
#endif
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls.Compatibility.Hosting;
using XamContextMenu.CustomControl;

namespace MauiContextMenu;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			}).ConfigureMauiHandlers((handlers) =>
			{
#if IOS
                handlers.AddHandler(typeof(ContextMenuView), typeof(ContextMenuViewRenderer));

				//Comment the above line and uncomment the below line when you want to try Handlers
                //handlers.AddHandler(typeof(ContextMenuView), typeof(ContextMenuViewHandler));
#endif
            });

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}

