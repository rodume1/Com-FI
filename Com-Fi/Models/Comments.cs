using System.ComponentModel.DataAnnotations.Schema;

namespace Com_Fi.Models
{
    /// <summary>
    /// Represents the Comments' structure
    /// </summary>
    public class Comments
    {
        /// <summary>
        /// Primary key for the Comments' table
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The Artists' ForeignKey
        /// </summary>
        [ForeignKey(nameof(Artists))]
        public int ArtistFK { get; set; }
        public Artists Artist { get; set; }

        /// <summary>
        /// The Comments' Body
        /// </summary>
        public string CommentBody { get; set; }

        /// <summary>
        /// The Comments' creation date
        /// </summary>
        public DateTime DateAdded { get; set; }

        /// <summary>
        /// The Comments' visibility
        /// </summary>
        public bool Visible { get; set; }

    }
}
