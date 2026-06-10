using EventHubMaui.Models;
using EventHubMaui.Services;

namespace EventHubMaui.Pages;

public partial class AdminEventEditPage : ContentPage
{
    private readonly EventService _eventService;
    private List<EventCategory> _categories = new();
    private int _editingEventId = 0;

    public AdminEventEditPage()
    {
        InitializeComponent();
        _eventService = ServiceHelper.GetService<EventService>();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadAsync();
    }

    private async Task LoadAsync()
    {
        _categories = await _eventService.GetCategoriesAsync();

        CategoryPicker.ItemsSource = _categories.Select(c => c.Name).ToList();

        if (CategoryPicker.SelectedIndex < 0 && _categories.Count > 0)
            CategoryPicker.SelectedIndex = 0;

        EventsCollection.ItemsSource = await _eventService.GetEventsAsync(includeHidden: true);

        if (_editingEventId == 0)
        {
            DatePicker.Date = DateTime.Today.AddDays(1);
            TimePicker.Time = new TimeSpan(18, 0, 0);
        }
    }

    private void ClearForm()
    {
        _editingEventId = 0;

        FormTitleLabel.Text = "Lisa uus üritus";
        SaveButton.Text = "+ Lisa üritus";

        TitleEntry.Text = "";
        DescriptionEditor.Text = "";
        LocationEntry.Text = "";
        ImageUrlEntry.Text = "";
        MaxParticipantsEntry.Text = "";

        DatePicker.Date = DateTime.Today.AddDays(1);
        TimePicker.Time = new TimeSpan(18, 0, 0);

        if (_categories.Count > 0)
            CategoryPicker.SelectedIndex = 0;
    }

    private void ClearButton_Clicked(object sender, EventArgs e)
    {
        ClearForm();
    }

    private async void EditButton_Clicked(object sender, EventArgs e)
    {
        if (sender is not Button button || button.CommandParameter is not int eventId)
            return;

        var item = await _eventService.GetEventAsync(eventId);

        if (item is null)
        {
            await DisplayAlert("Viga", "Üritust ei leitud.", "OK");
            return;
        }

        _editingEventId = item.EventId;

        FormTitleLabel.Text = "Muuda üritust";
        SaveButton.Text = "Salvesta muudatused";

        TitleEntry.Text = item.Title;
        DescriptionEditor.Text = item.Description;
        LocationEntry.Text = item.Location;
        ImageUrlEntry.Text = item.ImageUrl;
        MaxParticipantsEntry.Text = item.MaxParticipants.ToString();

        DatePicker.Date = item.EventDate.Date;
        TimePicker.Time = item.EventDate.TimeOfDay;

        var categoryIndex = _categories.FindIndex(c => c.CategoryId == item.CategoryId);
        CategoryPicker.SelectedIndex = categoryIndex >= 0 ? categoryIndex : 0;
    }

    private async void SaveButton_Clicked(object sender, EventArgs e)
    {
        if (CategoryPicker.SelectedIndex < 0)
        {
            await DisplayAlert("Viga", "Vali kategooria.", "OK");
            return;
        }

        if (!int.TryParse(MaxParticipantsEntry.Text, out int max) || max <= 0)
        {
            await DisplayAlert("Viga", "Osalejate arv peab olema positiivne number.", "OK");
            return;
        }

        EventItem item;

        if (_editingEventId == 0)
        {
            item = new EventItem();
        }
        else
        {
            item = await _eventService.GetEventAsync(_editingEventId) ?? new EventItem();
            item.EventId = _editingEventId;
        }

        item.Title = TitleEntry.Text?.Trim() ?? "";
        item.Description = DescriptionEditor.Text?.Trim() ?? "";
        item.CategoryId = _categories[CategoryPicker.SelectedIndex].CategoryId;
        item.EventDate = DatePicker.Date.Date.Add(TimePicker.Time);
        item.Location = LocationEntry.Text?.Trim() ?? "";
        item.ImageUrl = string.IsNullOrWhiteSpace(ImageUrlEntry.Text)
            ? "https://images.unsplash.com/photo-1501281668745-f7f57925c3b4?w=1200"
            : ImageUrlEntry.Text.Trim();
        item.MaxParticipants = max;
        item.IsPublished = true;

        if (item.Title.Length < 2 || item.Location.Length < 2)
        {
            await DisplayAlert("Viga", "Pealkiri ja asukoht on kohustuslikud.", "OK");
            return;
        }

        await _eventService.SaveEventAsync(item);

        await DisplayAlert("Valmis", _editingEventId == 0 ? "Üritus lisatud." : "Üritus muudetud.", "OK");

        ClearForm();
        await LoadAsync();
    }

    private async void DeleteButton_Clicked(object sender, EventArgs e)
    {
        if (sender is not Button button || button.CommandParameter is not int eventId)
            return;

        bool ok = await DisplayAlert("Kinnitus", "Kas kustutada üritus?", "Jah", "Ei");

        if (!ok)
            return;

        await _eventService.DeleteEventAsync(eventId);

        if (_editingEventId == eventId)
            ClearForm();

        await LoadAsync();
    }
}
