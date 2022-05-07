using Com_Fi.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Com_Fi.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options){ }
        // Define tables on the database
        public DbSet<Artists> Artists { get; set; }
        public DbSet<Albums> Albums { get; set; }
        public DbSet<Musics> Musics { get; set; }
        public DbSet<Genres> Genres { get; set; }
        public DbSet<Comments> Comments { get; set; }

    }
}