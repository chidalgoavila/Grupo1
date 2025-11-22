using Proyecto_FInal_Grupo_1.Models;

namespace Proyecto_FInal_Grupo_1.Repositories
{
    public interface ITeamCarRepository
    {
        Task<IEnumerable<TeamCar>> GetAll();
        Task<TeamCar?> GetOne(Guid id);
        Task Add(TeamCar teamCar);
        Task Update(TeamCar teamCar);
        Task Delete(TeamCar teamCar);
    }
}