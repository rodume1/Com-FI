using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Com_Fi.Models
{
    /// <summary>
    /// Represents the Musics' structure
    /// </summary>
    public class Musics
    {
        // Instance the lists of music genres & album musics
        public Musics() { 
            AlbumMusics = new HashSet<Albums>();
            // MusicGenres = new HashSet<Genres>();
        }

        /// <summary>
        /// Primary key for the Musics' table
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The Musics' Title
        /// </summary>
        [Display(Name = "Título")]
        public string Title { get; set; }

        /// <summary>
        /// The Albums' release year
        /// </summary>
        [Display(Name = "Ano de lançamento")]
        public int ReleaseYear { get; set; }

        [ForeignKey(nameof(Genres))]
        public int GenreFK { get; set; }
        public Genres Genre { get; set; }

        // Navigation Properties
        public ICollection<Albums> AlbumMusics { get; set; }

        // public ICollection<Genres> MusicGenres { get; set; }

    }
}
