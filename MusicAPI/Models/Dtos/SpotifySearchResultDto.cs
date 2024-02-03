using System.Text.Json.Serialization;

namespace MusicAPI.Models.Dtos
{
    //used to handle response to GetTop100MostPopularArtists method.
    public class PlaylistResponse
    {
        [JsonPropertyName("items")]
        public List<Item> Items { get; set; }

    }

    //used to handle response to GetTopTracksByArtist method
    public class TopTracksReponse
    {
        [JsonPropertyName("tracks")]
        public List<Track> Tracks { get; set; }
    }

    public class SpotifySearchResultDto
    {
        public SpotifyArtists artists { get; set; }
        public Tracks tracks { get; set; }
    }

    public class SpotifyArtists
    {
        public string href { get; set; }
        public Item[] items { get; set; }
        public int limit { get; set; }
        public string next { get; set; }
        public int offset { get; set; }
        public object previous { get; set; }
        public int total { get; set; }
    }

    //used to check each item in playlist
    public class Item
    {
        public External_Urls external_urls { get; set; }
        public Followers followers { get; set; }
        public string[] genres { get; set; }
        public string href { get; set; }
        public string id { get; set; }
        public Image[] images { get; set; }
        public string name { get; set; }
        public int popularity { get; set; }
        public string type { get; set; }
        public string uri { get; set; }
        public Track track { get; set; }

        public Artist1[] artists { get; set; }


    }

    public class External_Urls
    {
        public string spotify { get; set; }
    }

    public class Followers
    {
        public object href { get; set; }
        public int total { get; set; }
    }

    public class Image
    {
        public int height { get; set; }
        public string url { get; set; }
        public int width { get; set; }
    }

    public class Tracks
    {
        public string href { get; set; }
        public Item1[] items { get; set; }
        public int limit { get; set; }
        public string next { get; set; }
        public int offset { get; set; }
        public object previous { get; set; }
        public int total { get; set; }


    }

    //used to get information about each track within each item and for each top 10 track.
    public class Track
    {
        public Album album { get; set; }
        public Artist1[] artists { get; set; }
        public int disc_number { get; set; }
        public int duration_ms { get; set; }
        public bool episode { get; set; }
        public bool _explicit { get; set; }
        public External_Ids external_ids { get; set; }
        public External_Urls3 external_urls { get; set; }
        public string href { get; set; }
        public string id { get; set; }
        public bool is_local { get; set; }
        public bool is_playable { get; set; }
        public string name { get; set; }
        public int popularity { get; set; }
        public string preview_url { get; set; }
        public bool track { get; set; }
        public int track_number { get; set; }
        public string type { get; set; }
        public string uri { get; set; }
    }

    public class Item1
    {
        public Album album { get; set; }
        public Artist1[] artists { get; set; }
        public string[] available_markets { get; set; }
        public int disc_number { get; set; }
        public int duration_ms { get; set; }
        public bool _explicit { get; set; }
        public External_Ids external_ids { get; set; }
        public External_Urls3 external_urls { get; set; }
        public string href { get; set; }
        public string id { get; set; }
        public bool is_local { get; set; }
        public string name { get; set; }
        public int popularity { get; set; }
        public string preview_url { get; set; }
        public int track_number { get; set; }
        public string type { get; set; }
        public string uri { get; set; }
    }

    public class Album
    {
        public string album_type { get; set; }
        public Artist[] artists { get; set; }
        public string[] available_markets { get; set; }
        public External_Urls1 external_urls { get; set; }
        public string href { get; set; }
        public string id { get; set; }
        public Image1[] images { get; set; }
        public string name { get; set; }
        public string release_date { get; set; }
        public string release_date_precision { get; set; }
        public int total_tracks { get; set; }
        public string type { get; set; }
        public string uri { get; set; }
    }

    public class External_Urls1
    {
        public string spotify { get; set; }
    }

    public class SpotifyArtist
    {
        public External_Urls2 external_urls { get; set; }
        public string href { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string uri { get; set; }
    }

    public class External_Urls2
    {
        public string spotify { get; set; }
    }

    public class Image1
    {
        public int height { get; set; }
        public string url { get; set; }
        public int width { get; set; }
    }

    public class External_Ids
    {
        public string isrc { get; set; }
    }

    public class External_Urls3
    {
        public string spotify { get; set; }
    }

    //used to get info about artists.
    public class Artist1
    {
        public External_Urls4 external_urls { get; set; }

        public string href { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string uri { get; set; }
        public string[] genres { get; set; }
    }

    public class External_Urls4
    {
        public string spotify { get; set; }
    }

}
