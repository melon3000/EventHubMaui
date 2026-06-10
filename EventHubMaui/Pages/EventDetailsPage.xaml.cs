using EventHubMaui.Models;
using EventHubMaui.Services;

namespace EventHubMaui.Pages;

public partial class EventDetailsPage : ContentPage
{
    private readonly int _eventId;
    private readonly EventService _eventService;
    private readonly AuthService _authService;
    private EventCard? _event;

    public EventDetailsPage(int eventId)
    {
        InitializeComponent();
        _eventId = eventId;
        _eventService = ServiceHelper.GetService<EventService>();
        _authService = ServiceHelper.GetService<AuthService>();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadAsync();
    }

    private async Task LoadAsync()
    {
        _event = (await _eventService.GetEventsAsync(includeHidden: true)).FirstOrDefault(e => e.EventId == _eventId);
        if (_event is null)
        {
            await DisplayAlert("Viga", "Üritust ei leitud.", "OK");
            await Navigation.PopAsync();
            return;
        }

        EventImage.Source = _event.ImageUrl;
        TitleLabel.Text = _event.Title;
        CategoryLabel.Text = _event.CategoryName;
        DescriptionLabel.Text = _event.Description;
        DateLabel.Text = _event.DateText;
        ParticipantsLabel.Text = _event.ParticipantsText;
        LocationLabel.Text = _event.Location;
    }

    private async void RegisterButton_Clicked(object sender, EventArgs e)
    {
        if (!_authService.IsLoggedIn || _authService.CurrentUser is null)
        {
            bool goLogin = await DisplayAlert("Login required", "Registreerimiseks pead sisse logima.", "Login", "Cancel");
            if (goLogin)
                await Navigation.PushAsync(new WelcomePage());
            return;
        }

        var result = await _eventService.RegisterToEventAsync(_authService.CurrentUser.UserId, _eventId);
        await DisplayAlert(result.Success ? "Success" : "Info", result.Message, "OK");
        await LoadAsync();
    }
}
