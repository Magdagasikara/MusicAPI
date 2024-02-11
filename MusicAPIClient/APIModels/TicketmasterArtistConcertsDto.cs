using System.Text.Json.Serialization;

namespace MusicAPIClient.APIModels
{
    public class TicketmasterArtistConcertsDto
    {
        [JsonPropertyName("artistName")]
        public string ArtistName { get; set; }
        [JsonPropertyName("concerts")]
        public List<TicketmasterConcertsDto> Concerts { get; set; }
        [JsonPropertyName("message")]
        public string Message { get; set; }

    }

    public class TicketmasterConcertsDto
    {
        [JsonPropertyName("date")]
        public string Date { get; set; }
        [JsonPropertyName("city")]
        public string City { get; set; }
        [JsonPropertyName("country")]
        public string Country { get; set; }
        [JsonPropertyName("website")]
        public string Website { get; set; }
    }
}
