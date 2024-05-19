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
        public virtual DbSet<ConservationStatu> ConservationStatus { get; set; }
        public virtual DbSet<Donate> Donates { get; set; }
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

            modelBuilder.Entity<Animal>()
                .HasMany(e => e.News)
                .WithOptional(e => e.Animal)
                .HasForeignKey(e => e.ẠnimalID);

            modelBuilder.Entity<ConservationStatu>()
                .HasMany(e => e.Animals)
                .WithOptional(e => e.ConservationStatu)
                .HasForeignKey(e => e.ConservationStatusID);
        }
    }
}
