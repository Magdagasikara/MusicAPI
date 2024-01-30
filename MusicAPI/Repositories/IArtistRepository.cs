using MusicAPI.Models.Dtos;
using MusicAPI.Data;
using MusicAPI.Models.ViewModel;
using MusicAPI.Models;
using Artist = MusicAPI.Models.Artist;

namespace MusicAPI.Repositories
{
    public interface IArtistRepository
    {
        public void AddArtist(ArtistDto artistDto);
        public void AddSong(SongDto songDto, int artistId, int genreId);
        public void AddGenre(GenreDto genreDto);
        public List<ArtistsViewModel> GetArtists(int userId);
        public List<GenresViewModel> GetGenres(int userId);
        public List<SongsViewModel> GetSongs(int userId);
    }

    public class DbArtistRepository : IArtistRepository
    {
        private ApplicationContext _context;
        public DbArtistRepository(ApplicationContext context)
        {
            _context = context;
        }

        public void AddGenre(GenreDto genreDto)
        {
            if (_context.Genres.Any(g => g.Title == genreDto.Title))
            {
                throw new Exception($"Genre with Title {genreDto.Title} allready exists");
            }

            else
            {
                Genre? newGenre = new Genre
                {
                    Title = genreDto.Title
                };

                _context.Genres.Add(newGenre);
                
                try
                {
                    _context.SaveChanges();
                }
                catch
                {
                    throw new Exception("Unable to save Genre to database");
                }
            }
        }

        public void AddArtist(ArtistDto artistDto)
        {
            //check if duplicate by name

            Artist newArtist = new Artist
            {
                Name = artistDto.Name,
                Description = artistDto.Description
            };

            _context.Artists.Add(newArtist);

            try
            {
                _context.SaveChanges();
            }
            catch
            {
                throw new Exception("Unable to save Genre to database");
            }
        }

        public void AddSong(SongDto songDto, int artistId, int genreId)
        {
            /* remove to check if artists and genre exists else add before adding song in order artist -> genre -> song //Stina
             var artist = _context.Artists.SingleOrDefault(a => a.Id == artistId)
            ?? throw new ArtistNotFoundException();

            var genre = _context.Genres.SingleOrDefault(g => g.Id == genreId)
            ?? throw new GenreNotFoundException();
            */

            var newSong = new Song
            {
                Name = songDto.Name
            };

           foreach (ArtistDto art in songDto.Artists)
            {
                //if artist allready exists, dont create a new, else do.
                if (_context.Artists.Any(a => a.SpotifyId == art.SpotifyId))
                {
                    
                }



                 //if artist doesn't exist, add it to
            }

           
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
                throw new UserNotFoundException();
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
                    Description = a.Description
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
                throw new UserNotFoundException();
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
                    Title = g.Title
                })
                .ToList();

            return genres;
        }

        public List<SongsViewModel> GetSongs(int userId)
        {
            var user = _context.Users.SingleOrDefault(u => u.Id == userId);

            if (user == null)
            {
                throw new Exception($"No user with Id: {userId}");
            }

            List<Song>? userSongs = _context.Users
                .Where(u => u.Id == userId)
                .SelectMany(u => u.Songs)
                .ToList();

            List<SongsViewModel> songs = userSongs
                .Select(s => new SongsViewModel
                {
                    Name = s.Name
                }).ToList();

            return songs;

            throw new NotImplementedException();
        }
    }
}
