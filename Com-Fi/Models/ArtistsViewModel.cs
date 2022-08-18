namespace Com_Fi.Models
{
    /// <summary>
    /// class to collect artists' data to API
    /// </summary>
    public class ArtistsViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }
        public string Email { get; set; }
        public DateTime RegistrationDate { get; set; }
    }
}
