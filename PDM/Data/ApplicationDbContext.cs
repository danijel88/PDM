using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PDM.Models;

namespace PDM.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<ItemType> ItemTypes { get; set; }
        public DbSet<MachineType> MachineTypes { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<ItemImage> ItemImages { get; set; }
        public DbSet<Pdm> Pdm { get; set; }
        public DbSet<ItemHist> ItemHists { get; set; }
        public DbSet<Proposal> Propospals { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ItemType>(entity =>
            {
                entity.HasIndex(item => new { item.Name })
                .IsUnique(true);
            });

            builder.Entity<MachineType>(entity =>
            {
                entity.HasIndex(item => new { item.Name })
                .IsUnique(true);
            });

            builder.Entity<Item>(entity =>
            {
                entity.HasIndex(item => new { item.InternalCode })
                .IsUnique(true);

            });

            builder.Entity<ItemHist>(entity =>
            {
                entity
                  .HasOne(h => h.Item)
                  .WithMany(i => i.History)
                  .HasForeignKey("ItemTypeId")
                  .OnDelete(DeleteBehavior.Restrict);
            });



            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
