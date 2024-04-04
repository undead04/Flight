
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Flight.Data
{
    public class MyDbContext : IdentityDbContext<ApplicationUser>
    {
        public MyDbContext(DbContextOptions options) : base(options) { }
        public DbSet<Flight> flight { get; set; }
        public DbSet<Route> routes { get; set; }
        public DbSet<DocumentType> documentTypes { get; set; }
        public DbSet<DocumentFlight> documentFlight { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {

            base.OnModelCreating(builder);
            builder.Entity<Flight>(e =>
            {
                e.HasMany(e => e.DocumentFlight)
                .WithOne(e => e.Flight)
                .HasForeignKey(e => e.FlightId)
                .OnDelete(DeleteBehavior.Cascade);
                e.HasOne(e=>e.Route)
                .WithMany(e=>e.Flights)
                .HasForeignKey(e=>new {e.PoinOfLoading})
                .OnDelete(DeleteBehavior.NoAction);
                e.HasOne(e => e.Route)
                .WithMany(e => e.Flights)
                .HasForeignKey(e => new { e.PoinOfUnLoad })
                .OnDelete(DeleteBehavior.NoAction);
            });
            builder.Entity<DocumentType>(e =>
            {
                e.HasMany(e => e.DocumentFlights)
                .WithOne(e => e.DocumentType)
                .HasForeignKey(e => e.DocumentTypeId)
                .OnDelete(DeleteBehavior.NoAction);
                e.HasOne(e => e.ApplicationUser)
                .WithMany(e => e.DocumentTypes)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.NoAction);
                e.HasMany(e=>e.PermissionDocuments)
                .WithOne(e=>e.DocumentType)
                .HasForeignKey(e=>e.DocumnetTypeId)
                .OnDelete(DeleteBehavior.Cascade);
            });

        }
    }
}
