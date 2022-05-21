using Com_Fi.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Com_Fi.Data
{
    public class ApplicationDbContext : IdentityDbContext {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options){}

        /// <summary>
        /// Executes this chunk of code before the creation of the models.
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            // imports the previous execution of this method
            base.OnModelCreating(modelBuilder);

            // create the seed of Genres table
            modelBuilder.Entity<Genres>().HasData(
               new Genres { Id = 1, Title = "Jazz" },
               new Genres { Id = 2, Title = "Blues" },
               new Genres { Id = 3, Title = "Rock" },
               new Genres { Id = 4, Title = "Folk" },
               new Genres { Id = 5, Title = "Classical" }
            );
        }

        // Define tables on the database
        public DbSet<Artists> Artists { get; set; }
        public DbSet<Albums> Albums { get; set; }
        public DbSet<Musics> Musics { get; set; }
        public DbSet<Genres> Genres { get; set; }
        public DbSet<Comments> Comments { get; set; }

    }
}