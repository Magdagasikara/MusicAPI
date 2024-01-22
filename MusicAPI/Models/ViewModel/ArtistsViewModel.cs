namespace MusicAPI.Models.ViewModel
{
    public class ArtistsViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
