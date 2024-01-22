using MusicAPI.Models.Dtos;
using MusicAPI.Data;
using MusicAPI.Models.ViewModel;
using MusicAPI.Models;

namespace MusicAPI.Services
{
    public interface IArtistHelper
    {
        public void AddArtist(ArtistDto artistDto, int genreId);
        public void AddSong(SongDto songDto, int artistId, int genreId);
        public void AddGenre(GenreDto genreDto);
        public List<ArtistsViewModel> GetArtists(int userId);
        public List<GenresViewModel> GetGenres(int userId);
        public List<SongsViewModel> GetSongs(int userId);
    }

    public class ArtistHelper : IArtistHelper
    {
        private ApplicationContext _context;
        public ArtistHelper(ApplicationContext context)
        {
            _context = context;
        }

        public void AddGenre(GenreDto genreDto)
        {
            throw new NotImplementedException();
        }

        public void AddArtist(ArtistDto artistDto, int genreId)
        {
            throw new NotImplementedException();
        }

        public void AddSong(SongDto songDto, int artistId, int genreId)
        {
            var artist = _context.Artists.SingleOrDefault(a => a.Id == artistId)
            ?? throw new Exception($"Artist with Id {artistId} could not be found");

            var genre = _context.Genres.SingleOrDefault(g => g.Id == genreId)
            ?? throw new Exception($"Genre with id {genreId} could not be found");

            var newSong = new Song
            {
                Name = songDto.Name,
                Artist = artist,
                Genre = genre
            };

            _context.Songs.Add(newSong);
            _context.SaveChanges();
        }

        public List<ArtistsViewModel> GetArtists(int userId)
        {
            if (_context == null)
            {
                throw new ArgumentNullException(nameof(_context), "DbContext is not initialized");
            }

            var user = _context.Users.SingleOrDefault(u => u.Id  == userId);

            if (user == null)
            {
                throw new Exception($"No user with Id: {userId}");
            }

            if (user.Artists == null || user.Artists.Count == 0)
            {
                throw new Exception($"User with Id: {userId} has no artists saved");
            }

            var userArtists = _context.Users
                .Where(u => u.Id == userId)
                .SelectMany(u => u.Artists)
                .ToList();

            List<ArtistsViewModel> artists = userArtists
                .Select(a => new ArtistsViewModel
                {
                    Name = a.Name,
                    Description = a.Description,
                    Users = a.Users
                })
                .ToList();

            return artists;
        }

        public List<GenresViewModel> GetGenres(int userId)
        {
            if (_context == null)
            {
                throw new ArgumentNullException(nameof(_context), "DbContext is not initialized");
            }

            var user = _context.Users.SingleOrDefault(u => u.Id == userId);

            if (user == null)
            {
                throw new Exception($"No user with Id: {userId}");
            }

            if (user.Genres.Count == 0)
            {
                throw new Exception($"User with Id: {userId} has no genres saved");
            }

            var userGenres = _context.Users
                .Where(u => u.Id == userId)
                .SelectMany(u => u.Genres)
                .ToList();

            List<GenresViewModel> genres = userGenres
                .Select(g => new GenresViewModel
                {
                    Title = g.Title,
                    Users = g.Users
                })
                .ToList();

            return genres;
        }

        public List<SongsViewModel> GetSongs(int userId)
        {
            throw new NotImplementedException();
        }
    }
}
