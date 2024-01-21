using System.Threading.Tasks;
using Xunit;
using Moq;
using MyApplication.Data;
using MyApplication.Services;
using MyApplication.Dto;
using AccessibilityModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

public class OnderzoekServiceTests
{

    [Fact]
    public async Task CreateOnderzoek_ShouldCreateOnderzoekInDatabase()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        using (var context = new ApplicationDbContext(options))
        {

            context.Database.EnsureDeleted();

            var onderzoekService = new OnderzoekService(context);

            var onderzoekDto = new OnderzoekDto
            {
                titel = "Test Onderzoek",
                korteBeschrijving = "Dit is een testonderzoek",
                beloning = "Test Beloning",
                soortOnderzoek = "Fysiek" // Vul de juiste waarde in voor SoortOnderzoek
                                          // Vul andere velden in zoals nodig
            };

            var expectedOnderzoek = new Onderzoek
            {
                Titel = onderzoekDto.titel,
                KorteBeschrijving = onderzoekDto.korteBeschrijving,
                Beloning = onderzoekDto.beloning,
                SoortOnderzoek = onderzoekDto.soortOnderzoek,
                // Vul andere velden in zoals nodig
            };
            // Act
            var result = await onderzoekService.CreateOnderzoek(onderzoekDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedOnderzoek.Titel, result.Titel);
            // Voeg meer assertions toe op basis van je datamodel
        }
    }

    [Fact]
    public async Task GetOnderzoeken_ShouldReturnListOfOnderzoeken()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        using (var context = new ApplicationDbContext(options))
        {

            context.Database.EnsureDeleted();

            var onderzoekService = new OnderzoekService(context);

            var expectedOnderzoeken = new List<Onderzoek>
    {
        new Onderzoek
        {
            Titel = "Onderzoek (1)",
            KorteBeschrijving = "Korte beschrijving 1",
            Beloning = "Beloning 1",
            SoortOnderzoek = "Soort 1"
            // Vul andere vereiste velden in
        },
        new Onderzoek
        {
            Titel = "Onderzoek (2)",
            KorteBeschrijving = "Korte beschrijving 2",
            Beloning = "Beloning 2",
            SoortOnderzoek = "Soort 2"
            // Vul andere vereiste velden in
        }
        // Voeg meer onderzoeken toe indien nodig
    };

            // Voeg testdata toe aan de in-memory database
            context.Onderzoeken.AddRange(expectedOnderzoeken);
            context.SaveChanges();

            // Act
            var result = await onderzoekService.GetOnderzoeken();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedOnderzoeken.Count, result.Count);
            // Voeg meer assertions toe op basis van je datamodel

            // Voeg een extra assertie toe om de details van het onderzoek te controleren
            foreach (var expectedOnderzoek in expectedOnderzoeken)
            {
                Assert.Contains(result, actualOnderzoek =>
                    actualOnderzoek.Titel == expectedOnderzoek.Titel
                // Voeg andere veldvergelijkingen toe indien nodig
                );
            }
        }
    }

    [Fact]
    public async Task DeleteOnderzoek_ShouldRemoveOnderzoekFromDatabase()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;


        using (var context = new ApplicationDbContext(options))
        {
            context.Database.EnsureDeleted();
            var onderzoekId = 1; // ID van het onderzoek dat je wilt verwijderen

            var onderzoekService = new OnderzoekService(context);

            var onderzoekToRemove = new Onderzoek
            {
                OnderzoekId = onderzoekId,
                Titel = "Test Onderzoek",
                KorteBeschrijving = "Dit is een testonderzoek",
                Beloning = "Test Beloning",
                SoortOnderzoek = "Fysiek" // Vul de juiste waarde in voor SoortOnderzoek
                                          // Vul andere vereiste velden in
            };

            context.Onderzoeken.Add(onderzoekToRemove);
            context.SaveChanges();

            // Act
            await onderzoekService.DeleteOnderzoek(onderzoekId);

            // Assert
            var deletedOnderzoek = context.Onderzoeken.FirstOrDefault(o => o.OnderzoekId == onderzoekId);
            Assert.Null(deletedOnderzoek);
        }
    }


}



