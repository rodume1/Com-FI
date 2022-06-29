using Com_Fi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using System.ComponentModel.DataAnnotations;

namespace Com_Fi.Data
{
    /// <summary>
    /// class that represents new User data
    /// </summary>
    public class ApplicationUser : IdentityUser
    {

        /// <summary>
        /// personal name of user to be used at interface
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// registration date
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime RegistrationDate { get; set; }


    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options){ }

        /// <summary>
        /// it executes code before the creation of model
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // imports the previous execution of this method
            base.OnModelCreating(modelBuilder);
            //*****************************************
            // add, at this point, your new code

            // create the seed of Genres table
            modelBuilder.Entity<Genres>().HasData(
               new Genres { Id = 1, Title = "Jazz" },
               new Genres { Id = 2, Title = "Blues" },
               new Genres { Id = 3, Title = "Rock" },
               new Genres { Id = 4, Title = "Folk" },
               new Genres { Id = 5, Title = "Classical" }
            );

            // seed the Roles data
            modelBuilder.Entity<IdentityRole>().HasData(
              new IdentityRole { Id = "u", Name = "User", NormalizedName = "USER" },
              new IdentityRole { Id = "a", Name = "Artist", NormalizedName = "ARTIST" }              
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