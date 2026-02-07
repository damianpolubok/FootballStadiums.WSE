using FootballStadiums.WSE.Data;
using FootballStadiums.WSE.Services;
using FootballStadiums.WSE.Services.IServices;
using FootballStadiums.WSE.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Windows;

namespace FootballStadiums.WSE;

public partial class App : Application
{
    public static IHost? AppHost { get; private set; }

    public App()
    {
        AppHost = Host.CreateDefaultBuilder()
            .ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton(new DbContextOptionsBuilder<StadiumDbContext>()
                    .UseSqlServer("Server=localhost;Database=FootballStadiumsDb;Trusted_Connection=True;TrustServerCertificate=True")
                    .Options);

                services.AddSingleton<StadiumDbContextFactory>();
                services.AddTransient<IStadiumDataService, StadiumDataService>();

                services.AddTransient<MainViewModel>();

                services.AddSingleton<MainWindow>();
            })
            .Build();
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        await AppHost!.StartAsync();

        var startupForm = AppHost.Services.GetRequiredService<MainWindow>();
        startupForm.Show();

        base.OnStartup(e);
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        await AppHost!.StopAsync();
        base.OnExit(e);
    }
}