namespace MusicAPI.Models.ViewModel
{
    public class GenresViewModel
    {
        public string Title { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
