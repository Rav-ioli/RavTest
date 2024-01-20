using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AccessibilityModels;
using Microsoft.Identity.Client;



namespace MyApplication.Data;

public class ApplicationDbContext : IdentityDbContext<Gebruiker>
{
    public DbSet<Onderzoek> Onderzoeken { get; set; }
    public DbSet<Gebruiker> Gebruikers { get; set; }
    public DbSet<Hulpmiddel> Hulpmiddelen { get; set; }
    public DbSet<GebruikerHulpmiddel> GebruikerHulpmiddelen { get; set; }
    public DbSet<GebruikerBeperkingen> GebruikerBeperkingen { get; set; }
    public DbSet<Beperking> Beperkingen { get; set; }
    public DbSet<Ervaringsdeskundige> Ervaringsdeskundigen { get; set; }
    public DbSet<Bedrijf> Bedrijven { get; set; }
    public DbSet<ErvaringsdeskundigenOnderzoeken> ErvaringsdeskundigeOnderzoeken { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);

        //  builder.Entity<GebruikerHulpmiddel>()
        //     .HasKey(gh => new { gh.GebruikerId, gh.HulpmiddelId });

        // builder.Entity<GebruikerHulpmiddel>()
        //     .HasOne(gh => gh.Gebruiker)
        //     .WithMany(g => g.GebruikerHulpmiddelen)
        //     .HasForeignKey(gh => gh.GebruikerId);

        // builder.Entity<GebruikerHulpmiddel>()
        //     .HasOne(gh => gh.Hulpmiddel)
        //     .WithMany(h => h.GebruikerHulpmiddelen)
        //     .HasForeignKey(gh => gh.HulpmiddelId);

        //      builder.Entity<GebruikerBeperkingen>()
        //     .HasKey(gh => new { gh.GebruikerId, gh.BeperkingId });

        // builder.Entity<GebruikerBeperkingen>()
        //     .HasOne(gh => gh.Gebruiker)
        //     .WithMany(g => g.GebruikerBeperkingen)
        //     .HasForeignKey(gh => gh.GebruikerId);

        // builder.Entity<GebruikerBeperkingen>()
        //     .HasOne(gh => gh.Beperking)
        //     .WithMany(h => h.GebruikerBeperkingen)
        //     .HasForeignKey(gh => gh.BeperkingId);

        builder.Entity<GebruikerHulpmiddel>()
           .HasKey(gh => new { gh.ErvaringsdeskundigeId, gh.HulpmiddelId });

        builder.Entity<GebruikerHulpmiddel>()
            .HasOne(gh => gh.Ervaringsdeskundige)
            .WithMany(e => e.GebruikerHulpmiddelen)
            .HasForeignKey(gh => gh.ErvaringsdeskundigeId);

        builder.Entity<GebruikerHulpmiddel>()
            .HasOne(gh => gh.Hulpmiddel)
            .WithMany(h => h.GebruikerHulpmiddelen)
            .HasForeignKey(gh => gh.HulpmiddelId);

        builder.Entity<GebruikerBeperkingen>()
            .HasKey(gb => new { gb.ErvaringsdeskundigeId, gb.BeperkingId });

        builder.Entity<GebruikerBeperkingen>()
            .HasOne(gb => gb.Ervaringsdeskundige)
            .WithMany(e => e.GebruikerBeperkingen)
            .HasForeignKey(gb => gb.ErvaringsdeskundigeId);

        builder.Entity<GebruikerBeperkingen>()
            .HasOne(gb => gb.Beperking)
            .WithMany(b => b.GebruikerBeperkingen)
            .HasForeignKey(gb => gb.BeperkingId);


        builder.Entity<ErvaringsdeskundigenOnderzoeken>()
            .HasKey(eo => new { eo.ErvaringsdeskundigeId, eo.OnderzoekId });

        builder.Entity<ErvaringsdeskundigenOnderzoeken>()
            .HasOne(eo => eo.Ervaringsdeskundige)
            .WithMany(e => e.ErvaringsdeskundigenOnderzoeken)
            .HasForeignKey(eo => eo.ErvaringsdeskundigeId);

        builder.Entity<ErvaringsdeskundigenOnderzoeken>()
            .HasOne(eo => eo.Onderzoek)
            .WithMany(o => o.ErvaringsdeskundigenOnderzoeken)
            .HasForeignKey(eo => eo.OnderzoekId);

            
    }
}
