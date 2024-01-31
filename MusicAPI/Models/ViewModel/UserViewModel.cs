namespace MusicAPI.Models.ViewModel
{
    public class UserViewModel
    {
        public string Name { get; set; }
        public List<ArtistsViewModel> Artists { get; set; }
        public List<GenresViewModel> Genres { get; set; }
        public List<SongsViewModel> Songs { get; set; }
    }
}
