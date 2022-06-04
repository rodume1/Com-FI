namespace Com_Fi.Models
{
    /// <summary>
    /// Represents the Artists' structure
    /// </summary>
    public class Artists
    {
        // Instance the list of album artist
        public Artists() { 
            AlbumArtists = new HashSet<Albums>();
            CommentArtists = new HashSet<Comments>();
        }

        /// <summary>
        /// Primary key for the Artists' table
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The Artists' name
        /// </summary>
        public string Name { get; set; }

        // Navigation properties
        public ICollection<Albums> AlbumArtists { get; set; }
        public ICollection<Comments> CommentArtists { get; set; }

        /// <summary>
        /// identificação a autenticação do artista
        /// </summary>
        public string UserId { get; set; }
    }
}
