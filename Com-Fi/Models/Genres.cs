using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Com_Fi.Models
{
    /// <summary>
    /// Represents the Genres' structure
    /// </summary>
    public class Genres
    {
        // Instance the list of music genres
        // public Genres() { 
        //     MusicGenres = new HashSet<Musics>();
        // }

        /// <summary>
        /// Primary key for the Genres' table
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The Genres' Title
        /// </summary>
        [Display(Name = "Título")]
        public string Title { get; set; }

        // Navigation Properties
        public ICollection<Musics> MusicGenres { get; set; }
    }
}
