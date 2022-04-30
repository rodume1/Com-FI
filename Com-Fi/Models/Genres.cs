namespace Com_Fi.Models
{
    /// <summary>
    /// Represents the Genres' structure
    /// </summary>
    public class Genres
    {
        /// <summary>
        /// Primary key for the Genres' table
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The Genres' Title
        /// </summary>
        public string Title { get; set; }

        //Navigation Properties
        public ICollection<MusicGenres> MusicGenres { get; set; }
    }
}
