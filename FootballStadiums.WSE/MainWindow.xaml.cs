using FootballStadiums.WSE.Models;
using FootballStadiums.WSE.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;

namespace FootballStadiums.WSE;

public partial class MainWindow : Window
{
    private readonly MainViewModel _viewModel;

    public MainWindow(MainViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        DataContext = _viewModel;

        EditFormView.OnSaveRequested += EditFormView_OnSaveRequested;
        EditFormView.OnCancelRequested += EditFormView_OnCancelRequested;

        Loaded += async (s, e) => await _viewModel.LoadStadiumsAsync();
    }

    private async void Button_Refresh_Click(object sender, RoutedEventArgs e)
    {
        await _viewModel.LoadStadiumsAsync();
    }

    private void Button_Edit_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.DataContext is Stadium stadiumToEdit)
        {
            EditFormView.LoadStadium(stadiumToEdit);
            OverlayContainer.Visibility = Visibility.Visible;
        }
    }

    private async void Button_Delete_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.DataContext is Stadium stadiumToDelete)
        {
            var result = MessageBox.Show(
                $"Are you sure you want to delete {stadiumToDelete.Name}?",
                "Confirm Delete",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    await _viewModel.DeleteStadiumAsync(stadiumToDelete.Id);
                    MessageBox.Show("Stadium deleted.", "Deleted", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting stadium: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }

    private async void EditFormView_OnSaveRequested(object? sender, Stadium updatedStadium)
    {
        try
        {
            await _viewModel.UpdateStadiumAsync(updatedStadium);
            OverlayContainer.Visibility = Visibility.Collapsed;
            MessageBox.Show("Stadium updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Update failed: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void EditFormView_OnCancelRequested(object? sender, EventArgs e)
    {
        OverlayContainer.Visibility = Visibility.Collapsed;
    }

    private async void Button_Add_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(AddNameTxt.Text) || string.IsNullOrWhiteSpace(AddCityTxt.Text))
        {
            MessageBox.Show("Name and City are required.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        try
        {
            var stadium = new Stadium
            {
                Name = AddNameTxt.Text,
                ImageUrl = AddImageTxt.Text,
                Address = new Address
                {
                    City = AddCityTxt.Text,
                    Country = AddCountryTxt.Text,
                    Street = AddStreetTxt.Text
                }
            };

            if (!string.IsNullOrWhiteSpace(AddClubsTxt.Text))
            {
                foreach (var c in AddClubsTxt.Text.Split(','))
                {
                    if (!string.IsNullOrWhiteSpace(c))
                        stadium.Clubs.Add(new Club { Name = c.Trim() });
                }
            }

            await _viewModel.AddStadiumAsync(stadium);
            MessageBox.Show("Stadium added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            AddNameTxt.Text = "";
            AddCityTxt.Text = "";
            AddCountryTxt.Text = "";
            AddStreetTxt.Text = "";
            AddImageTxt.Text = "";
            AddClubsTxt.Text = "";

            MyTabControl.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error adding stadium: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}