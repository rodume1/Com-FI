namespace Com_Fi.Models
{
    /// <summary>
    /// class to collect musics' data to API
    /// </summary>
    public class MusicsViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int ReleaseYear { get; set; }
        public Genres Genre { get; set; }
    }
}
