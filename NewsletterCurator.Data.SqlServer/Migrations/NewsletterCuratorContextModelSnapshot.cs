﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NewsletterCurator.Data;

namespace NewsletterCurator.Data.Migrations
{
    [DbContext(typeof(NewsletterCuratorContext))]
    partial class NewsletterCuratorContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("NewsletterCurator.Data.Category", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("ID");

                    b.ToTable("Categories");

                    b.HasData(
                        new { ID = new Guid("bbf3205e-578b-4568-9d86-7c15fceb6a4f"), Name = "DevOps" },
                        new { ID = new Guid("3f9acf3f-bf48-455d-9a3f-f660cd3a13b3"), Name = "Front End" },
                        new { ID = new Guid("57e0baf7-3b80-4866-b9ae-3a2e77ad88fb"), Name = "AI" },
                        new { ID = new Guid("12e0baf7-3b80-4866-b9ae-3a2e77ad88fb"), Name = "Space" },
                        new { ID = new Guid("40e0baf7-3b80-4866-b9ae-3a2e77ad88fb"), Name = "Security" },
                        new { ID = new Guid("44754987-6f3f-4b5e-a79d-a61b13a61647"), Name = "iGaming" },
                        new { ID = new Guid("497ff497-33d2-434c-a1db-5a722d94078f"), Name = "General Tech" },
                        new { ID = new Guid("527ff497-33d2-434c-a1db-5a722d94078f"), Name = "Infrastructure" },
                        new { ID = new Guid("317ff497-33d2-434c-a1db-5a722d94078f"), Name = "Software Development" },
                        new { ID = new Guid("e17226a6-bed1-44f5-863f-3970bb634fce"), Name = ".NET" }
                    );
                });

            modelBuilder.Entity("NewsletterCurator.Data.Newsitem", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("CategoryID");

                    b.Property<DateTimeOffset>("DateTime");

                    b.Property<string>("ImageURL");

                    b.Property<bool>("IsAlreadySent");

                    b.Property<string>("Summary")
                        .IsRequired();

                    b.Property<string>("Title")
                        .IsRequired();

                    b.Property<string>("URL")
                        .IsRequired();

                    b.HasKey("ID");

                    b.HasAlternateKey("URL");

                    b.HasIndex("CategoryID");

                    b.ToTable("Newsitems");
                });

            modelBuilder.Entity("NewsletterCurator.Data.Recipient", b =>
                {
                    b.Property<string>("Email")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("DisplayName")
                        .IsRequired();

                    b.HasKey("Email");

                    b.ToTable("Recipients");
                });

            modelBuilder.Entity("NewsletterCurator.Data.Newsitem", b =>
                {
                    b.HasOne("NewsletterCurator.Data.Category", "Category")
                        .WithMany("Newsitems")
                        .HasForeignKey("CategoryID")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
