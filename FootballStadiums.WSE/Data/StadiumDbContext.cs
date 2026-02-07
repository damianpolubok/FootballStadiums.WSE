using FootballStadiums.WSE.Models;
using Microsoft.EntityFrameworkCore;

namespace FootballStadiums.WSE.Data;

public class StadiumDbContext : DbContext
{
    public StadiumDbContext(DbContextOptions<StadiumDbContext> options) : base(options)
    {
    }

    public DbSet<Stadium> Stadiums { get; set; }
    public DbSet<Club> Clubs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Stadium>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired();

            entity.OwnsOne(e => e.Address, address =>
            {
                address.Property(a => a.Street).HasColumnName("Address_Street");
                address.Property(a => a.City).HasColumnName("Address_City");
                address.Property(a => a.Country).HasColumnName("Address_Country");
            });

            entity.HasMany(e => e.Clubs)
                  .WithOne(c => c.Stadium)
                  .HasForeignKey(c => c.StadiumId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}