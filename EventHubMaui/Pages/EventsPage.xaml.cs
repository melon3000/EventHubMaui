using EventHubMaui.Models;
using EventHubMaui.Services;

namespace EventHubMaui.Pages;

public partial class EventsPage : ContentPage
{
    private readonly EventService _eventService;
    private List<EventCategory> _categories = new();

    public EventsPage()
    {
        InitializeComponent();
        _eventService = ServiceHelper.GetService<EventService>();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadCategoriesAsync();
        await LoadEventsAsync();
    }

    private async Task LoadCategoriesAsync()
    {
        _categories = await _eventService.GetCategoriesAsync();
        CategoryPicker.ItemsSource = new List<string> { "Kõik kategooriad" }.Concat(_categories.Select(c => c.Name)).ToList();

        if (CategoryPicker.SelectedIndex < 0)
            CategoryPicker.SelectedIndex = 0;
    }

    private async Task LoadEventsAsync()
    {
        int? categoryId = null;
        if (CategoryPicker.SelectedIndex > 0)
            categoryId = _categories[CategoryPicker.SelectedIndex - 1].CategoryId;

        EventsCollection.ItemsSource = await _eventService.GetEventsAsync(categoryId);
    }

    private async void CategoryPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        await LoadEventsAsync();
    }

    private async void RefreshButton_Clicked(object sender, EventArgs e)
    {
        await LoadEventsAsync();
    }

    private async void EventsCollection_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is EventCard selected)
        {
            EventsCollection.SelectedItem = null;
            await Navigation.PushAsync(new EventDetailsPage(selected.EventId));
        }
    }

    private async void MyButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MyRegistrationsPage());
    }

    private async void AdminButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AdminDashboardPage());
    }
}
