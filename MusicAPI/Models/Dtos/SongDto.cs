namespace MusicAPI.Models.Dtos
{
    public class SongDto
    {
        //change to artists if several?
        public string Artist { get; set; }
        public string Name { get; set; }

        public string SpotifyId { get; set;}
    }
}
