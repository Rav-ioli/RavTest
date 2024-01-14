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

        var bedrijf = await _databaseContext.Bedrijven.FindAsync(onderzoekDto.uitvoerendbedrijf);
        var onderzoek = new Onderzoek
        {
            Titel = onderzoekDto.titel,
            KorteBeschrijving = onderzoekDto.korteBeschrijving,
            Datum = onderzoekDto.datum,
            Beloning = onderzoekDto.beloning,
            SoortOnderzoek = onderzoekDto.soortOnderzoek,
            UitvoerendBedrijf = bedrijf,
            UitvoerendBedrijfNaam = bedrijf?.Bedrijfsnaam
        };

        await _databaseContext.Onderzoeken.AddAsync(onderzoek);
        await _databaseContext.SaveChangesAsync();
        return onderzoek;
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
}