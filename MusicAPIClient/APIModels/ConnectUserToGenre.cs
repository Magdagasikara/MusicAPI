using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MusicAPIClient.APIModels
{
    internal class ConnectUserToGenre
    {
        [JsonPropertyName("username")]
        public string Username { get; set; }
        [JsonPropertyName("genreid")]
        public int GenreId { get; set; }
    }
}
