﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MyApplication.Data;

#nullable disable

namespace WDPR_dotnet_new_webapi.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240110214906_uabwhew")]
    partial class uabwhew
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("AccessibilityModels.Beperking", b =>
                {
                    b.Property<int>("BeperkingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BeperkingId"));

                    b.Property<string>("BeperkingNaam")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("BeperkingId");

                    b.ToTable("Beperkingen");
                });

            modelBuilder.Entity("AccessibilityModels.Gebruiker", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasMaxLength(21)
                        .HasColumnType("nvarchar(21)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Postcode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);

                    b.HasDiscriminator<string>("Discriminator").HasValue("Gebruiker");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("AccessibilityModels.GebruikerBeperkingen", b =>
                {
                    b.Property<string>("ErvaringsdeskundigeId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("BeperkingId")
                        .HasColumnType("int");

                    b.HasKey("ErvaringsdeskundigeId", "BeperkingId");

                    b.HasIndex("BeperkingId");

                    b.ToTable("GebruikerBeperkingen");
                });

            modelBuilder.Entity("AccessibilityModels.GebruikerHulpmiddel", b =>
                {
                    b.Property<string>("ErvaringsdeskundigeId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("HulpmiddelId")
                        .HasColumnType("int");

                    b.HasKey("ErvaringsdeskundigeId", "HulpmiddelId");

                    b.HasIndex("HulpmiddelId");

                    b.ToTable("GebruikerHulpmiddelen");
                });

            modelBuilder.Entity("AccessibilityModels.Hulpmiddel", b =>
                {
                    b.Property<int>("HulpmiddelId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("HulpmiddelId"));

                    b.Property<string>("HulpmiddelNaam")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("HulpmiddelId");

                    b.ToTable("Hulpmiddelen");
                });

            modelBuilder.Entity("AccessibilityModels.Onderzoek", b =>
                {
                    b.Property<int>("OnderzoekId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("OnderzoekId"));

                    b.Property<string>("Beloning")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Datum")
                        .HasColumnType("datetime2");

                    b.Property<string>("KorteBeschrijving")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Locatie")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("MaxLeeftijd")
                        .HasColumnType("int");

                    b.Property<int?>("MinLeeftijd")
                        .HasColumnType("int");

                    b.Property<string>("PostcodeCriteria")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SoortOnderzoek")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StatusDeelname")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Titel")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TypeBeperking")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("OnderzoekId");

                    b.ToTable("Onderzoeken");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Name")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("AccessibilityModels.Ervaringsdeskundige", b =>
                {
                    b.HasBaseType("AccessibilityModels.Gebruiker");

                    b.Property<string>("Achternaam")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BeschikbareTijden")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EmailVoogd")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("GeboorteDatum")
                        .HasColumnType("datetime2");

                    b.Property<bool>("MagCommercieelBenaderdWorden")
                        .HasColumnType("bit");

                    b.Property<string>("NaamVoogd")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TelefoonnummerVoogd")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("VoorkeurBenadering")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Voornaam")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasDiscriminator().HasValue("Ervaringsdeskundige");
                });

            modelBuilder.Entity("AccessibilityModels.GebruikerBeperkingen", b =>
                {
                    b.HasOne("AccessibilityModels.Beperking", "Beperking")
                        .WithMany("GebruikerBeperkingen")
                        .HasForeignKey("BeperkingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AccessibilityModels.Ervaringsdeskundige", "Ervaringsdeskundige")
                        .WithMany("GebruikerBeperkingen")
                        .HasForeignKey("ErvaringsdeskundigeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Beperking");

                    b.Navigation("Ervaringsdeskundige");
                });

            modelBuilder.Entity("AccessibilityModels.GebruikerHulpmiddel", b =>
                {
                    b.HasOne("AccessibilityModels.Ervaringsdeskundige", "Ervaringsdeskundige")
                        .WithMany("GebruikerHulpmiddelen")
                        .HasForeignKey("ErvaringsdeskundigeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AccessibilityModels.Hulpmiddel", "Hulpmiddel")
                        .WithMany("GebruikerHulpmiddelen")
                        .HasForeignKey("HulpmiddelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Ervaringsdeskundige");

                    b.Navigation("Hulpmiddel");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("AccessibilityModels.Gebruiker", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("AccessibilityModels.Gebruiker", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AccessibilityModels.Gebruiker", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("AccessibilityModels.Gebruiker", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AccessibilityModels.Beperking", b =>
                {
                    b.Navigation("GebruikerBeperkingen");
                });

            modelBuilder.Entity("AccessibilityModels.Hulpmiddel", b =>
                {
                    b.Navigation("GebruikerHulpmiddelen");
                });

            modelBuilder.Entity("AccessibilityModels.Ervaringsdeskundige", b =>
                {
                    b.Navigation("GebruikerBeperkingen");

                    b.Navigation("GebruikerHulpmiddelen");
                });
#pragma warning restore 612, 618
        }
    }
}