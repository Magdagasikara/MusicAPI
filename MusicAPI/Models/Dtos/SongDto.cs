namespace MusicAPI.Models.Dtos
{
    public class SongDto
    {
        //change to artists if several?
        public List<ArtistDto> Artists { get; set; }
        public string Name { get; set; }
        public string SpotifyId { get; set;}
    }
}
