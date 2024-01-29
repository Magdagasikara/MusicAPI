using MusicAPI.Models.Dtos;
using MusicAPI.Data;
using MusicAPI.Models.ViewModel;
using MusicAPI.Models;
using Microsoft.EntityFrameworkCore;

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
        public Task AddArtistsGenresAndTracksFromSpotify(ArtistDto artistDto, GenreDto genreDto, List<SongDto> songDtos);
    };

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
            Artist? newArtist = new Artist
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
            var artist = _context.Artists.SingleOrDefault(a => a.Id == artistId)
            ?? throw new ArtistNotFoundException();

            var genre = _context.Genres.SingleOrDefault(g => g.Id == genreId)
            ?? throw new GenreNotFoundException();

            var newSong = new Song
            {
                Name = songDto.Name,
                Artist = artist,
                Genre = genre
            };

            _context.Songs.Add(newSong);

            try
            {
                _context.SaveChanges();
            }
            catch
            {
                throw new Exception("Unable to save Genre to database");
            }
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

        public async Task AddArtistsGenresAndTracksFromSpotify(ArtistDto artistDto, GenreDto genreDto, List<SongDto> songDtos)
        {
            var genreInDb = await _context.Genres.FirstOrDefaultAsync(g => g.Title == genreDto.Title);
            var artistInDb = await _context.Artists.FirstOrDefaultAsync(a => a.Name == artistDto.Name);
            
            if (artistInDb == null)
            {
                Artist artist = new Artist()
                {
                    Name = artistDto.Name,
                    Description = ""
                };

                if (genreInDb == null)
                {
                    Genre genre = new Genre()
                    {
                        Title = genreDto.Title
                    };

                    foreach (var songDto in songDtos)
                    {
                        Song song = new Song
                        {
                            Name = songDto.Name,
                            Artist = artist,
                            Genre = genre
                        };

                        _context.Add(song);
                    }
                }
                else
                {
                    foreach (var songDto in songDtos)
                    {
                        Song song = new Song
                        {
                            Name = songDto.Name,
                            Artist = artist,
                            Genre = genreInDb
                        };

                        _context.Add(song);
                    }
                }
            }
            else
            {
                if (genreInDb == null)
                {
                    Genre genre = new Genre()
                    {
                        Title = genreDto.Title
                    };

                    foreach (var songDto in songDtos)
                    {
                        Song song = new Song
                        {
                            Name = songDto.Name,
                            Artist = artistInDb,
                            Genre = genre
                        };

                        _context.Add(song);
                    }
                }
                else
                {
                    foreach (var songDto in songDtos)
                    {
                        Song song = new Song
                        {
                            Name = songDto.Name,
                            Artist = artistInDb,
                            Genre = genreInDb
                        };

                        _context.Add(song);
                    }
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            {
                throw new Exception("Unable to save songs to database");
            }
        }
    }
}
