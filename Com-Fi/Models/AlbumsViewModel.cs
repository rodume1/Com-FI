namespace Com_Fi.Models
{
    /// <summary>
    /// class to collect albums' data to API
    /// </summary>
    public class AlbumsViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int ReleaseYear { get; set; }
        public List<Musics> AlbumMusics { get; set; }
    }
}
