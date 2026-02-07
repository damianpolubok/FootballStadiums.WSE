using FootballStadiums.WSE.Models;
using FootballStadiums.WSE.Services.IServices;
using System.Collections.ObjectModel;

namespace FootballStadiums.WSE.ViewModels;

public class MainViewModel : ViewModelBase
{
    private readonly IStadiumDataService _stadiumDataService;

    public ObservableCollection<Stadium> Stadiums { get; } = new();

    public MainViewModel(IStadiumDataService stadiumDataService)
    {
        _stadiumDataService = stadiumDataService;
    }

    public async Task LoadStadiumsAsync()
    {
        var data = await _stadiumDataService.GetAllStadiumsAsync();
        Stadiums.Clear();
        foreach (var stadium in data)
        {
            Stadiums.Add(stadium);
        }
    }

    public async Task AddStadiumAsync(Stadium stadium)
    {
        await _stadiumDataService.AddStadiumAsync(stadium);
        await LoadStadiumsAsync();
    }

    public async Task UpdateStadiumAsync(Stadium stadium)
    {
        await _stadiumDataService.UpdateStadiumAsync(stadium);
        await LoadStadiumsAsync();
    }

    public async Task DeleteStadiumAsync(int id)
    {
        await _stadiumDataService.DeleteStadiumAsync(id);
        await LoadStadiumsAsync();
    }
}