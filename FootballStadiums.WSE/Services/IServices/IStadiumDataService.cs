using FootballStadiums.WSE.Models;

namespace FootballStadiums.WSE.Services.IServices
{
    public interface IStadiumDataService
    {
        Task AddStadiumAsync(Stadium stadium);
        Task DeleteStadiumAsync(int id);
        Task<IEnumerable<Stadium>> GetAllStadiumsAsync();
        Task UpdateStadiumAsync(Stadium stadium);
    }
}