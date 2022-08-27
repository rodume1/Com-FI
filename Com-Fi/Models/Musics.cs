using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Com_Fi.Models
{
    /// <summary>
    /// Represents the Musics' structure
    /// </summary>
    public class Musics
    {
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
    }
}
