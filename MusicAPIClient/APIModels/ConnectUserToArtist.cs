using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MusicAPIClient.APIModels
{
    public class ConnectUserToArtist
    {
        [JsonPropertyName("username")]
        public string Username { get; set; }
        [JsonPropertyName("artistid")]
        public int ArtistId { get; set; }
    }
}
