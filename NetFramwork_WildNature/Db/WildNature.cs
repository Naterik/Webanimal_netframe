using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace NetFramwork_WildNature.Db
{
    public partial class WildNature : DbContext
    {
        public WildNature()
            : base("name=WildNature")
        {
        }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Animal> Animals { get; set; }
        public virtual DbSet<AnimalDetail> AnimalDetails { get; set; }
        public virtual DbSet<Area> Areas { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Color> Colors { get; set; }
        public virtual DbSet<Conservation> Conservations { get; set; }
        public virtual DbSet<Donate> Donates { get; set; }
        public virtual DbSet<FavoriteAnimal> FavoriteAnimals { get; set; }
        public virtual DbSet<Image> Images { get; set; }
        public virtual DbSet<News> News { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Specie> Species { get; set; }
        public virtual DbSet<Volunteer> Volunteers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>()
                .Property(e => e.Code)
                .IsFixedLength();

            modelBuilder.Entity<Account>()
                .HasMany(e => e.Donates)
                .WithRequired(e => e.Account)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Animal>()
                .HasMany(e => e.AnimalDetails)
                .WithRequired(e => e.Animal)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Animal>()
                .HasMany(e => e.FavoriteAnimals)
                .WithRequired(e => e.Animal)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Animal>()
                .HasMany(e => e.News)
                .WithRequired(e => e.Animal)
                .HasForeignKey(e => e.ẠnimalID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AnimalDetail>()
                .HasMany(e => e.Images)
                .WithRequired(e => e.AnimalDetail)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Area>()
                .HasMany(e => e.Animals)
                .WithRequired(e => e.Area)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Category>()
                .HasMany(e => e.Animals)
                .WithRequired(e => e.Category)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Color>()
                .HasMany(e => e.AnimalDetails)
                .WithRequired(e => e.Color)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Specie>()
                .HasMany(e => e.AnimalDetails)
                .WithRequired(e => e.Specie)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Volunteer>()
                .HasMany(e => e.FavoriteAnimals)
                .WithRequired(e => e.Volunteer)
                .WillCascadeOnDelete(false);
        }
    }
}
