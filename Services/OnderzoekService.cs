using MyApplication.Data;
using Microsoft.EntityFrameworkCore;
using AccessibilityModels;
using MyApplication.Dto;

namespace MyApplication.Services;
public class OnderzoekService
{


    private readonly ApplicationDbContext _databaseContext;

    public OnderzoekService(ApplicationDbContext databaseContext)
    {
        _databaseContext = databaseContext;
    }


    public async Task<Onderzoek> CreateOnderzoek(OnderzoekDto onderzoekDto)
    {

Bedrijf bedrijf = null;

    if (!string.IsNullOrEmpty(onderzoekDto.uitvoerendbedrijfemail))
    {
        bedrijf = await _databaseContext.Bedrijven.FirstOrDefaultAsync(b => b.Email == onderzoekDto.uitvoerendbedrijfemail);
    }
    else
    {
        bedrijf = await _databaseContext.Bedrijven.FindAsync(onderzoekDto.uitvoerendbedrijf);
    }        var onderzoek = new Onderzoek
        {
            Titel = onderzoekDto.titel,
            KorteBeschrijving = onderzoekDto.korteBeschrijving,
            Datum = onderzoekDto.datum,
            Beloning = onderzoekDto.beloning,
            SoortOnderzoek = onderzoekDto.soortOnderzoek,
            UitvoerendBedrijf = bedrijf,
            UitvoerendBedrijfNaam = bedrijf?.Bedrijfsnaam,
            beperking = await _databaseContext.Beperkingen.FindAsync(onderzoekDto.typebeperking),
            Locatie =  string.IsNullOrEmpty(onderzoekDto.locatie) ? "N/A" : onderzoekDto.locatie
        };

        await _databaseContext.Onderzoeken.AddAsync(onderzoek);
        await _databaseContext.SaveChangesAsync();
        return onderzoek;
    }

    public async Task DeleteOnderzoek(int id)
    {
        var onderzoek = await _databaseContext.Onderzoeken.FindAsync(id);
        _databaseContext.Onderzoeken.Remove(onderzoek);
        await _databaseContext.SaveChangesAsync();
    }

    public async Task<Onderzoek> GetOnderzoek(int id)
    {
        return await _databaseContext.Onderzoeken.FindAsync(id);
    }

    public async Task<List<Onderzoek>> GetOnderzoeken()
    {
        return await _databaseContext.Onderzoeken.ToListAsync();
    }
    public async Task<Onderzoek> ClearOnderzoekDB()
    {
        var onderzoeken = await _databaseContext.Onderzoeken.ToListAsync();
        foreach (var onderzoek in onderzoeken)
        {
            _databaseContext.Onderzoeken.Remove(onderzoek);
        }
        await _databaseContext.SaveChangesAsync();
        return null;
    }
    public async Task<List<Onderzoek>> GetOnderzoekenByBeperking(Beperking beperking)
    {
        var onderzoeken = await _databaseContext.Onderzoeken
            .Where(o => o.beperking == beperking)
            .ToListAsync();
        foreach (var onderzoek in onderzoeken)
        {
            Console.WriteLine(onderzoek);
        }
        return onderzoeken;
    }
    public async Task CreateUserOnderzoekAsync(string GebruikerId, int OnderzoekId)
    {
        ErvaringsdeskundigenOnderzoeken gebruikerOnderzoek = new ErvaringsdeskundigenOnderzoeken
        {
            ErvaringsdeskundigeId = GebruikerId,
            OnderzoekId = OnderzoekId
        };
        _databaseContext.ErvaringsdeskundigeOnderzoeken.Add(gebruikerOnderzoek);
        await _databaseContext.SaveChangesAsync();
    }
    public async Task<List<OnderzoekCount>> GetCountAanmeldingForEachOnderzoek()
    {
        var onderzoeken = await _databaseContext.Onderzoeken.ToListAsync();
        var result = new List<OnderzoekCount>();

        foreach (var onderzoek in onderzoeken)
        {
            var count = await _databaseContext.ErvaringsdeskundigeOnderzoeken.Where(e => e.OnderzoekId == onderzoek.OnderzoekId).CountAsync();
            result.Add(new OnderzoekCount { Titel = onderzoek.Titel, Count = count,onderzoekId = onderzoek.OnderzoekId });
        }

        return result;
    }
    public async Task<List<Onderzoek>> GetOnderzoekenFromBedrijf(string email){
        var bedrijf = await _databaseContext.Users.FirstOrDefaultAsync(b => b.Email == email);
         var onderzoeken = await _databaseContext.Onderzoeken.Where(o => o.UitvoerendBedrijf == bedrijf).ToListAsync();
        return onderzoeken;
    }

    

}
