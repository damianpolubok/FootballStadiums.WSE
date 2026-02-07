using Microsoft.EntityFrameworkCore;

namespace FootballStadiums.WSE.Data;

public class StadiumDbContextFactory
{
    private readonly DbContextOptions<StadiumDbContext> _options;

    public StadiumDbContextFactory(DbContextOptions<StadiumDbContext> options)
    {
        _options = options;
    }

    public StadiumDbContext CreateDbContext()
    {
        return new StadiumDbContext(_options);
    }
}