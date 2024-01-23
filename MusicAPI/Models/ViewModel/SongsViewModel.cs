namespace MusicAPI.Models.ViewModel
{
    public class SongsViewModel
    {
        public string Name { get; set; }
        public string Artist { get; set; }
        public string Genre { get; set; }
        public ICollection<ArtistsViewModel> ArtistsViewModels { get; set; }
    }
}
