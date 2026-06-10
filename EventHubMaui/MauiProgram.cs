using EventHubMaui.Services;
using EventHubMaui.Pages;

namespace EventHubMaui;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder.UseMauiApp<App>();

        builder.Services.AddSingleton<DatabaseService>();
        builder.Services.AddSingleton<AuthService>();
        builder.Services.AddSingleton<EventService>();

        builder.Services.AddTransient<WelcomePage>();
        builder.Services.AddTransient<EventsPage>();
        builder.Services.AddTransient<EventDetailsPage>();
        builder.Services.AddTransient<MyRegistrationsPage>();
        builder.Services.AddTransient<ProfilePage>();
        builder.Services.AddTransient<AdminDashboardPage>();
        builder.Services.AddTransient<AdminEventEditPage>();
        builder.Services.AddTransient<AdminCategoriesPage>();

        var app = builder.Build();
        ServiceHelper.Services = app.Services;
        return app;
    }
}
