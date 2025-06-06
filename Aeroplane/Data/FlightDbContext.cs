using Microsoft.EntityFrameworkCore;
using Aeroplane.Models;


// FlightDbContext links the Flight model and the database.
// It allows querying, inserting, updating, and deleting flight records using Entity Framework Core.

namespace Aeroplane.Data
{
    public class FlightDbContext : DbContext
    {
        public FlightDbContext(DbContextOptions<FlightDbContext> options)
            : base(options)
        {
        }

        public DbSet<Flight> Flights { get; set; }
    }
}
