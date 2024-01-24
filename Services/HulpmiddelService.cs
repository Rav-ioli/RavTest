using MyApplication.Data;
using Microsoft.EntityFrameworkCore;
using AccessibilityModels;
using MyApplication.Dto;

namespace MyApplication.Services;
public class HulpmiddelService

{


    private readonly ApplicationDbContext _databaseContext;

    public HulpmiddelService(ApplicationDbContext databaseContext)
    {
        _databaseContext = databaseContext;
    }


    public async Task<Hulpmiddel> CreateHulpmiddel(HulpmiddelDto hulpmiddelDto)
    {


        var hulpmiddel = new Hulpmiddel
        {
            HulpmiddelNaam = hulpmiddelDto.hulpmiddelnaam
        };

        await _databaseContext.Hulpmiddelen.AddAsync(hulpmiddel);
        await _databaseContext.SaveChangesAsync();
        return hulpmiddel;

    }
    // public async Task<Onderzoek> GetOnderzoek(int id)
    // {
    //     return await _databaseContext.Onderzoeken.FindAsync(id);
    // }

    public async Task<List<Hulpmiddel>> GetHulpmiddelen()
    {
        return await _databaseContext.Hulpmiddelen.ToListAsync();
    }

    public async Task<int> GetHulpmiddelId(string hulpmiddelNaam)
    {
        var expectedUser = await _databaseContext.Hulpmiddelen.FirstOrDefaultAsync(b => b.HulpmiddelNaam == hulpmiddelNaam);
        return expectedUser.HulpmiddelId;
    }
    public async Task CreateUserHulpmiddelAsync(string GebruikerId, int HulpmiddelId)
    {
        GebruikerHulpmiddel gebruikerHulpmiddelen = new GebruikerHulpmiddel
        {
            ErvaringsdeskundigeId = GebruikerId,
            HulpmiddelId = HulpmiddelId
        };
        _databaseContext.GebruikerHulpmiddelen.Add(gebruikerHulpmiddelen);

        await _databaseContext.SaveChangesAsync();
    }
    public async Task<Onderzoek> ClearHulpmiddelDB()
    {
        var hulpmiddelen = await _databaseContext.Hulpmiddelen.ToListAsync();
        foreach (var hulpmiddel in hulpmiddelen)
        {
            _databaseContext.Hulpmiddelen.Remove(hulpmiddel);
        }
        await _databaseContext.SaveChangesAsync();
        return null;
    }
}