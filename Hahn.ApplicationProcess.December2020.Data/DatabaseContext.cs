using Hahn.ApplicationProcess.December2020.Domain;
using Microsoft.EntityFrameworkCore;

namespace Hahn.ApplicationProcess.December2020.Data
{
    public sealed class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions options) : base(options) { }

        public DbSet<Applicant> Applicants => Set<Applicant>();
    }
}