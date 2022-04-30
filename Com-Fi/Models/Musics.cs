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
        public string Title { get; set; }

        /// <summary>
        /// The Albums' release year
        /// </summary>
        public int ReleaseYear { get; set; }

        //Navigation Properties
        public ICollection<AlbumMusics> AlbumMusics { get; set; }

        public ICollection<MusicGenres> MusicGenres { get; set; }

    }
}
