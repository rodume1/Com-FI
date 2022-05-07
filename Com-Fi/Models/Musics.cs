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
            MusicGenres = new HashSet<Genres>();
        }

        /// <summary>
        /// Primary key for the Musics' table
        /// </summary>
        public int Id { get; set; } 

        /// <summary>
        /// The Musics' Title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The Albums' release year
        /// </summary>
        public int ReleaseYear { get; set; }

        // Navigation Properties
        public ICollection<Albums> AlbumMusics { get; set; }

        public ICollection<Genres> MusicGenres { get; set; }

    }
}
