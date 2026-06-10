using EventHubMaui.Models;
using EventHubMaui.Services;

namespace EventHubMaui.Pages;

public partial class AdminCategoriesPage : ContentPage
{
    private readonly EventService _eventService;
    private List<EventCategory> _categories = new();
    private int _editingCategoryId = 0;

    public AdminCategoriesPage()
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
        CategoriesCollection.ItemsSource = _categories;
    }

    private void ClearForm()
    {
        _editingCategoryId = 0;
        FormTitleLabel.Text = "Lisa kategooria";
        SaveButton.Text = "+ Lisa kategooria";
        NameEntry.Text = "";
        DescriptionEntry.Text = "";
    }

    private void ClearButton_Clicked(object sender, EventArgs e)
    {
        ClearForm();
    }

    private void EditButton_Clicked(object sender, EventArgs e)
    {
        if (sender is not Button button || button.CommandParameter is not int categoryId)
            return;

        var category = _categories.FirstOrDefault(c => c.CategoryId == categoryId);

        if (category is null)
            return;

        _editingCategoryId = category.CategoryId;
        FormTitleLabel.Text = "Muuda kategooriat";
        SaveButton.Text = "Salvesta muudatused";

        NameEntry.Text = category.Name;
        DescriptionEntry.Text = category.Description;
    }

    private async void SaveButton_Clicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(NameEntry.Text))
        {
            await DisplayAlert("Viga", "Sisesta kategooria nimi.", "OK");
            return;
        }

        EventCategory category;

        if (_editingCategoryId == 0)
        {
            category = new EventCategory();
        }
        else
        {
            category = _categories.FirstOrDefault(c => c.CategoryId == _editingCategoryId) ?? new EventCategory();
            category.CategoryId = _editingCategoryId;
        }

        category.Name = NameEntry.Text.Trim();
        category.Description = DescriptionEntry.Text?.Trim() ?? "";

        await _eventService.SaveCategoryAsync(category);

        await DisplayAlert("Valmis", _editingCategoryId == 0 ? "Kategooria lisatud." : "Kategooria muudetud.", "OK");

        ClearForm();
        await LoadAsync();
    }

    private async void DeleteButton_Clicked(object sender, EventArgs e)
    {
        if (sender is not Button button || button.CommandParameter is not int categoryId)
            return;

        bool ok = await DisplayAlert("Kinnitus", "Kas kustutada kategooria?", "Jah", "Ei");

        if (!ok)
            return;

        await _eventService.DeleteCategoryAsync(categoryId);

        if (_editingCategoryId == categoryId)
            ClearForm();

        await LoadAsync();
    }
}
