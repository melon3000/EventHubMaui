using Microsoft.Extensions.DependencyInjection;

namespace EventHubMaui;

public static class ServiceHelper
{
    public static IServiceProvider? Services { get; set; }

    public static T GetService<T>() where T : notnull
    {
        if (Services is null)
            throw new InvalidOperationException("Teenused ei ole käivitatud.");

        return Services.GetRequiredService<T>();
    }
}
