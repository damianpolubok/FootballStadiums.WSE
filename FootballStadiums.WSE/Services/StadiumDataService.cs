using FootballStadiums.WSE.Data;
using FootballStadiums.WSE.Models;
using FootballStadiums.WSE.Services.IServices;
using Microsoft.EntityFrameworkCore;

namespace FootballStadiums.WSE.Services;

public class StadiumDataService : IStadiumDataService
{
    private readonly StadiumDbContextFactory _contextFactory;

    public StadiumDataService(StadiumDbContextFactory contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<IEnumerable<Stadium>> GetAllStadiumsAsync()
    {
        using var context = _contextFactory.CreateDbContext();
        return await context.Stadiums
            .Include(s => s.Clubs)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task AddStadiumAsync(Stadium stadium)
    {
        using var context = _contextFactory.CreateDbContext();
        context.Stadiums.Add(stadium);
        await context.SaveChangesAsync();
    }

    public async Task UpdateStadiumAsync(Stadium stadium)
    {
        using var context = _contextFactory.CreateDbContext();
        var existingStadium = await context.Stadiums
            .Include(s => s.Clubs)
            .FirstOrDefaultAsync(s => s.Id == stadium.Id);

        if (existingStadium != null)
        {
            existingStadium.Name = stadium.Name;
            existingStadium.ImageUrl = stadium.ImageUrl;
            existingStadium.Address.Street = stadium.Address.Street;
            existingStadium.Address.City = stadium.Address.City;
            existingStadium.Address.Country = stadium.Address.Country;

            existingStadium.Clubs.Clear();
            foreach (var club in stadium.Clubs)
            {
                existingStadium.Clubs.Add(club);
            }
            await context.SaveChangesAsync();
        }
    }

    public async Task DeleteStadiumAsync(int id)
    {
        using var context = _contextFactory.CreateDbContext();
        var stadium = await context.Stadiums.FindAsync(id);
        if (stadium != null)
        {
            context.Stadiums.Remove(stadium);
            await context.SaveChangesAsync();
        }
    }
}