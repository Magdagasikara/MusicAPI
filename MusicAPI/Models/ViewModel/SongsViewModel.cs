namespace MusicAPI.Models.ViewModel
{
    public class SongsViewModel
    {
        public string Name { get; set; }
        public ICollection<ArtistsViewModel> ArtistsViewModels { get; set; }
    }
}
