using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Supabase;
using UWO_DailyCustodian.Model;

namespace UWO_DailyCustodian
{
    public static class MauiProgram
    {
        public static IBusinessLogic BusinessLogic = new BusinessLogic();
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
