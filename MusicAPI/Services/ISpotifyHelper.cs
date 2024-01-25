using MusicAPI.Data;
using MusicAPI.Models;
using MusicAPI.Models.Dtos;
using System.Net.Http.Headers;
using System.Text.Json;

namespace MusicAPI.Services
{
    public interface ISpotifyHelper
    {
        Task<SpotifyTrackArtistGenreDto> GetArtistGenreAndTrack(string accessToken, string searchQuery);
    }

    public class SpotifyHelper : ISpotifyHelper
    {
        private readonly HttpClient _httpClient;
        private readonly ApplicationContext _context;

        public SpotifyHelper(HttpClient httpClient, ApplicationContext context)
        {
            _httpClient = httpClient;
            _context = context;
        }

        public async Task SaveArtistGenreAndTrackToDatabase(string accessToken, string searchQuery)
        {
            if (string.IsNullOrWhiteSpace(searchQuery))
            {
                return;
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await _httpClient.GetAsync($"https://api.spotify.com/v1/search?q={Uri.EscapeDataString(searchQuery)}&type=track,artist");

            response.EnsureSuccessStatusCode();

            using var responseStream = await response.Content.ReadAsStreamAsync();
            var responseObject = await JsonSerializer.DeserializeAsync<SpotifySearchResultDto>(responseStream);

            if (responseObject.Tracks != null && responseObject.Artist != null)
            {
                var artist = _context.Artists.FirstOrDefault(a => a.Name == responseObject.Artist.name);

                if (artist == null)
                {
                    artist = new Artist
                    {
                        Name = responseObject.Artist.name
                    };
                }

                var genre = _context.Genres.FirstOrDefault(g => g.Title == responseObject.Artist.genres[0]);

                if (genre == null)
                {
                    genre = new Genre
                    {
                        Title = responseObject.Artist.genres[0]
                    };
                }

                var track = new Song
                {
                    Name = responseObject.Tracks.name,
                    Artist = artist,
                    Genre = genre
                };

                artist.Songs.Add(track);
                await _context.SaveChangesAsync();
            }
        }
    }
}
