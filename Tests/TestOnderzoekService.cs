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
                soortOnderzoek = "Fysiek"
            };

            var expectedOnderzoek = new Onderzoek
            {
                Titel = onderzoekDto.titel,
                KorteBeschrijving = onderzoekDto.korteBeschrijving,
                Beloning = onderzoekDto.beloning,
                SoortOnderzoek = onderzoekDto.soortOnderzoek,

            };
            // Act
            var result = await onderzoekService.CreateOnderzoek(onderzoekDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedOnderzoek.Titel, result.Titel);

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

        },
        new Onderzoek
        {
            Titel = "Onderzoek (2)",
            KorteBeschrijving = "Korte beschrijving 2",
            Beloning = "Beloning 2",
            SoortOnderzoek = "Soort 2"

        }

    };

            context.Onderzoeken.AddRange(expectedOnderzoeken);
            context.SaveChanges();

            // Act
            var result = await onderzoekService.GetOnderzoeken();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedOnderzoeken.Count, result.Count);



            foreach (var expectedOnderzoek in expectedOnderzoeken)
            {
                Assert.Contains(result, actualOnderzoek =>
                    actualOnderzoek.Titel == expectedOnderzoek.Titel

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
            var onderzoekId = 1;

            var onderzoekService = new OnderzoekService(context);

            var onderzoekToRemove = new Onderzoek
            {
                OnderzoekId = onderzoekId,
                Titel = "Test Onderzoek",
                KorteBeschrijving = "Dit is een testonderzoek",
                Beloning = "Test Beloning",
                SoortOnderzoek = "Fysiek"
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



