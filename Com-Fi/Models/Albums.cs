﻿using System.ComponentModel.DataAnnotations;

namespace Com_Fi.Models
{
    /// <summary>
    /// Represents the Albums' structure
    /// </summary>
    public class Albums
    {
        // Instance the list of album musics
        public Albums() { 
            AlbumMusics = new HashSet<Musics>();
            AlbumArtists = new HashSet<Artists>();
        }

        /// <summary>
        /// Primary key for the Albums' table
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The Albums' title
        /// </summary>
        [Display(Name = "Título")]
        public string Title { get; set; }

        /// <summary>
        /// The Albums' release year
        /// </summary>
        [Display(Name = "Ano de lançamento")]
        public int ReleaseYear { get; set; }

        /// <summary>
        /// The image of the Albums' cover 
        /// </summary>
        [Display(Name = "Capa")]
        public string Cover { get; set; }

        // Navigation Properties
        [Display(Name = "Músicas associadas ao álbum")]
        public ICollection<Musics> AlbumMusics { get; set; }
        public ICollection<Artists> AlbumArtists { get; set; }

    }
}
