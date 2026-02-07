using FootballStadiums.WSE.Models;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace FootballStadiums.WSE.Views;

public partial class StadiumFormView : UserControl
{
    public event EventHandler<Stadium>? OnSaveRequested;
    public event EventHandler? OnCancelRequested;

    private int _currentId;

    public StadiumFormView()
    {
        InitializeComponent();
    }

    public void LoadStadium(Stadium stadium)
    {
        _currentId = stadium.Id;
        NameTxt.Text = stadium.Name;
        CityTxt.Text = stadium.Address.City;
        CountryTxt.Text = stadium.Address.Country;
        StreetTxt.Text = stadium.Address.Street;
        ImageTxt.Text = stadium.ImageUrl;
        ClubsTxt.Text = string.Join(", ", stadium.Clubs.Select(c => c.Name));

        HeaderTxt.Text = "Edit Stadium";
    }

    private void Save_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(NameTxt.Text) || string.IsNullOrWhiteSpace(CityTxt.Text))
        {
            MessageBox.Show("Name and City are required.");
            return;
        }

        var updatedStadium = new Stadium
        {
            Id = _currentId,
            Name = NameTxt.Text,
            ImageUrl = ImageTxt.Text,
            Address = new Address
            {
                Street = StreetTxt.Text,
                City = CityTxt.Text,
                Country = CountryTxt.Text
            }
        };

        if (!string.IsNullOrWhiteSpace(ClubsTxt.Text))
        {
            var clubs = ClubsTxt.Text.Split(',');
            foreach (var club in clubs)
            {
                updatedStadium.Clubs.Add(new Club { Name = club.Trim() });
            }
        }

        OnSaveRequested?.Invoke(this, updatedStadium);
    }

    private void Cancel_Click(object sender, RoutedEventArgs e)
    {
        OnCancelRequested?.Invoke(this, EventArgs.Empty);
    }
}