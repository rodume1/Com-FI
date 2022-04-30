namespace Com_Fi.Models
{
    /// <summary>
    /// Represents the Albums' structure
    /// </summary>
    public class Albums
    {
        /// <summary>
        /// Primary key for the Albums' table
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The Albums' title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The Albums' release year
        /// </summary>
        public int ReleaseYear { get; set; }

        /// <summary>
        /// The image of the Albums' cover 
        /// </summary>
        public String Cover { get; set; }

        //Navigation Properties
        public ICollection<AlbumMusics> AlbumMusics { get; set; }

    }
}
