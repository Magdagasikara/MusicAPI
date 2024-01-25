namespace MusicAPI.Models.Dtos
{
    public class SpotifySearchResultDto
    {
        public SpotifyArtistDto Artist { get; set; }
        public SpotifyTrackDto Tracks { get; set; }
    }

    public class SpotifyArtistDto
    {
        public string[] genres { get; set; }
        public string href { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public int popularity { get; set; }
        public string type { get; set; }
        public string uri { get; set; }
    }

    public class SpotifyTrackDto
    {
        public object[] available_markets { get; set; }
        public int disc_number { get; set; }
        public int duration_ms { get; set; }
        public bool _explicit { get; set; }
        public string href { get; set; }
        public string id { get; set; }
        public bool is_local { get; set; }
        public string name { get; set; }
        public int popularity { get; set; }
        public object preview_url { get; set; }
        public int track_number { get; set; }
        public string type { get; set; }
        public string uri { get; set; }
    }
}
