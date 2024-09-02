using Microsoft.EntityFrameworkCore;
using Web_API_Formatter.Entities;

namespace Web_API_Formatter.Data
{
    public class SchoolDbContext:DbContext
    {
        public SchoolDbContext(DbContextOptions<SchoolDbContext> options):base(options)
        {
            
        }

        public DbSet<Student> Students { get; set; }

    }
}
