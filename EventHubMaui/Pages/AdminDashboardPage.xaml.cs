using EventHubMaui.Services;

namespace EventHubMaui.Pages;

public partial class AdminDashboardPage : ContentPage
{
    private readonly AuthService _authService;

    public AdminDashboardPage()
    {
        InitializeComponent();
        _authService = ServiceHelper.GetService<AuthService>();
    }

    private async Task<bool> EnsureAdminAsync()
    {
        if (_authService.IsAdmin)
            return true;

        bool goLogin = await DisplayAlert("Ligipääs puudub", "Admin funktsioonid vajavad admin kontot.", "Login", "Cancel");
        if (goLogin)
            await Navigation.PushAsync(new WelcomePage());

        return false;
    }

    private async void EventsButton_Clicked(object sender, EventArgs e)
    {
        if (await EnsureAdminAsync())
            await Navigation.PushAsync(new AdminEventEditPage());
    }

    private async void CategoriesButton_Clicked(object sender, EventArgs e)
    {
        if (await EnsureAdminAsync())
            await Navigation.PushAsync(new AdminCategoriesPage());
    }

    private async void ProfileButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ProfilePage());
    }
}
