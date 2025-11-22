using Microsoft.EntityFrameworkCore;
using Proyecto_FInal_Grupo_1.Data;
using Proyecto_FInal_Grupo_1.Models;

namespace Proyecto_FInal_Grupo_1.Repositories
{
    public class TeamCarRepository : ITeamCarRepository
    {
        private readonly AppDbContext _db;
        public TeamCarRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task Add(TeamCar teamCar)
        {
            await _db.TeamCars.AddAsync(teamCar);
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<TeamCar>> GetAll()
        {
            return await _db.TeamCars.ToListAsync();
        }

        public async Task<TeamCar?> GetOne(Guid id)
        {
            return await _db.TeamCars.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task Update(TeamCar teamCar)
        {
            _db.TeamCars.Update(teamCar);
            await _db.SaveChangesAsync();
        }

        public async Task Delete(TeamCar teamCar)
        {
            _db.TeamCars.Remove(teamCar);
            await _db.SaveChangesAsync();
        }
    }
}