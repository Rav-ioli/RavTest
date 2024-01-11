using MyApplication.Data;
using Microsoft.EntityFrameworkCore;
using AccessibilityModels;
using MyApplication.Dto;

namespace MyApplication.Services
{
    public class BeperkingService
    {
        private readonly ApplicationDbContext _databaseContext;

        public BeperkingService(ApplicationDbContext databaseContext)
        {
            _databaseContext = databaseContext;
        }
        public async Task<Beperking> CreateBeperking(BeperkingDto beperkingDto)
        {
            var beperking = new Beperking
            {
                BeperkingNaam = beperkingDto.beperkingsnaam
            };

            await _databaseContext.Beperkingen.AddAsync(beperking);
            await _databaseContext.SaveChangesAsync();
            return beperking;
        }
        public async Task<List<Beperking>> GetBeperkingen()
        {
            return await _databaseContext.Beperkingen.ToListAsync();
        }
        public async Task<int> GetBeperkingId(string beperkingNaam)
        {
            var expectedBeperking = await _databaseContext.Beperkingen.FirstOrDefaultAsync(b => b.BeperkingNaam == beperkingNaam);
            return expectedBeperking.BeperkingId;
        }
        public async Task<Onderzoek> ClearBeperkingDB()
        {
            var beperkingen = await _databaseContext.Beperkingen.ToListAsync();
            foreach (var beperking in beperkingen)
            {
                _databaseContext.Beperkingen.Remove(beperking);
            }
            await _databaseContext.SaveChangesAsync();
            return null;
        }

        public async Task CreateUserBeperkingAsync(string GebruikerId, int BeperkingId)
        {
            GebruikerBeperkingen gebruikerBeperkingen = new GebruikerBeperkingen
            {
                ErvaringsdeskundigeId = GebruikerId,
                BeperkingId = BeperkingId
            };
            _databaseContext.GebruikerBeperkingen.Add(gebruikerBeperkingen);

            await _databaseContext.SaveChangesAsync();
        }
    }
}
