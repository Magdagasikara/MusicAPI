using MusicAPI.Models.Dtos;
using MusicAPI.Data;
using MusicAPI.Models.ViewModel;
using MusicAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace MusicAPI.Repositories
{
    public interface IArtistRepository
    {
        public void AddArtist(ArtistDto artistDto);
        public void AddSong(SongDto songDto, int artistId, int genreId);
        public void AddGenre(GenreDto genreDto);

        public List<ArtistsViewModel> GetArtistsForUser(string username);
        public List<GenresViewModel> GetGenresForUser(string username);
        public List<SongsViewModel> GetSongsForUser(string username);

        public List<ArtistsWithIdViewModel> GetArtists(string? name, string? amountPerPage, string? pageNumber);
        public List<GenresWithIdViewModel> GetGenres(string? title, string? amountPerPage, string? pageNumber);
        public List<SongsWithIdViewModel> GetSongs(string? name, string? amountPerPage, string? pageNumber);
        public Task AddArtistsGenresAndTracksFromSpotify(ArtistDto artistDto, GenreDto genreDto, List<SongDto> songDtos);
    }

    public class DbArtistRepository : IArtistRepository
    {
        public ApplicationContext _context;

        public DbArtistRepository(ApplicationContext context)
        {
            _context = context;
        }

        public void AddGenre(GenreDto genreDto)
        {
            if (_context.Genres.Any(g => g.Title == genreDto.Title))
            {
                throw new Exception($"Genre with Title {genreDto.Title} already exists");
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

        public List<ArtistsViewModel> GetArtistsForUser(string username)
        {
            if (_context == null)
            {
                throw new ArgumentNullException(nameof(_context), "DbContext is not initialized");
            }

            var user = _context.Users.Include(u => u.Artists)
                .SingleOrDefault(u => u.Name.ToUpper() == username.ToUpper());

            if (user == null)
            {
                throw new UserNotFoundException();
            }

            if (user.Artists == null || user.Artists.Count == 0)
            {
                throw new Exception($"User {username} has no artists saved");
            }

            var userArtists = _context.Users
                .Where(u => u.Name == username)
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

        public List<GenresViewModel> GetGenresForUser(string username)
        {
            if (_context == null)
            {
                throw new ArgumentNullException(nameof(_context), "DbContext is not initialized");
            }

            var user = _context.Users
                .Include(u => u.Genres)
                .SingleOrDefault(u => u.Name.ToUpper() == username.ToUpper());

            if (user == null)
            {
                throw new UserNotFoundException();
            }

            if (user.Genres.Count == 0)
            {
                throw new Exception($"User {username} has no genres saved");
            }

            var userGenres = _context.Users
                .Where(u => u.Name == username)
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

        public List<SongsViewModel> GetSongsForUser(string username)
        {
            var user = _context.Users
                .Include(u => u.Songs)
                    .ThenInclude(s => s.Artist)
                .Include(u => u.Songs)
                    .ThenInclude(s => s.Genre)
                .SingleOrDefault(u => u.Name.ToUpper() == username.ToUpper());

            if (user == null)
            {
                throw new Exception($"No user {username}");
            }

            List<SongsViewModel> songs = user
                .Songs
                .Select(s => new SongsViewModel
                {
                    Name = s.Name,
                    Artist = s.Artist.Name,
                    Genre = s.Genre.Title,
                })
                .ToList();


            return songs;

        }

        public List<ArtistsWithIdViewModel> GetArtists(string? name, string? amountPerPage, string? pageNumber)
        {

            List<ArtistsWithIdViewModel> artists = _context.Artists
                .Select(a => new ArtistsWithIdViewModel
                {
                    Id = a.Id,
                    Name = a.Name,
                    Description = a.Description
                })
                .OrderBy(a => a.Name)
                .ToList();


            if (artists == null || artists.Count == 0)
            {
                throw new Exception($"There are no available artists");
            }

            // Show all artists
            if (name is null && amountPerPage is null && pageNumber is null)
            {
                return artists;
            }

            // Show a filtered list       
            if (name is not null)
            {
                artists = artists
                        .Where(a => a.Name.ToUpper().StartsWith(name.ToUpper()))
                        .ToList();
            }

            // Pagination
            // check if amountPerPage & pageNumber are integers, otherwise return bad request
            // if pageNumber = 0 we assume user meant = 1
            // if amountPerPage = 0 we set default = 10
            if (pageNumber == "0") pageNumber = "1";
            if (amountPerPage == "0") amountPerPage = "10"; 
            if (amountPerPage is not null || pageNumber is not null)
            {
                int parsedAmountPerPage = 10; //sets default value if only pageNumber not null
                if (amountPerPage is not null && !int.TryParse(amountPerPage.ToString(), out parsedAmountPerPage))
                {
                    throw new Exception($"{nameof(amountPerPage)} måste vara en integer");
                }
                int parsedPageNumber = 1; //sets default value if only amountPerPage not null
                if (pageNumber is not null && !int.TryParse(pageNumber.ToString(), out parsedPageNumber))
                {
                    throw new Exception($"{nameof(pageNumber)} måste vara en integer");
                }

                int skipAmount = parsedAmountPerPage * (parsedPageNumber - 1);
                int numberOfPages = (int)Math.Ceiling((decimal)artists.Count / parsedAmountPerPage);
                int amountOnThisPage = parsedPageNumber < numberOfPages ? parsedAmountPerPage : artists.Count % parsedAmountPerPage;

                artists = artists
                 .Skip(skipAmount)
                 .Take(amountOnThisPage)
                 .ToList();

            }

            return artists;

        }

        public List<GenresWithIdViewModel> GetGenres(string? title, string? amountPerPage, string? pageNumber)
        {
            List<GenresWithIdViewModel> genres = _context.Genres
                .Select(g => new GenresWithIdViewModel
                {
                    Id = g.Id,
                    Title = g.Title,
                })
                .OrderBy(g => g.Title)
                .ToList();

            if (genres == null || genres.Count == 0)
            {
                throw new Exception($"There are no available genres");
            }

            // Show all genres
            if (title is null && amountPerPage is null && pageNumber is null)
            {
                return genres;
            }


            // Show a filtered list       
            if (title is not null)
            {
                genres = genres
                        .Where(a => a.Title.ToUpper().Contains(title.ToUpper()))
                        .ToList();
            }

            // Pagination
            // check if amountPerPage & pageNumber are integers, otherwise return bad request
            // if pageNumber = 0 we assume user meant = 1
            // if amountPerPage = 0 we set default = 10
            if (pageNumber == "0") pageNumber = "1";
            if (amountPerPage == "0") amountPerPage = "10";
            if (amountPerPage is not null || pageNumber is not null)
            {
                int parsedAmountPerPage = 10; //sets default value if only pageNumber not null
                if (amountPerPage is not null && !int.TryParse(amountPerPage.ToString(), out parsedAmountPerPage))
                {
                    throw new Exception($"{nameof(amountPerPage)} måste vara en integer");
                }
                int parsedPageNumber = 1; //sets default value if only amountPerPage not null
                if (pageNumber is not null && !int.TryParse(pageNumber.ToString(), out parsedPageNumber))
                {
                    throw new Exception($"{nameof(pageNumber)} måste vara en integer");
                }

                int skipAmount = parsedAmountPerPage * (parsedPageNumber - 1);
                int numberOfPages = (int)Math.Ceiling((decimal)genres.Count / parsedAmountPerPage);
                int amountOnThisPage = parsedPageNumber < numberOfPages ? parsedAmountPerPage : genres.Count % parsedAmountPerPage;

                genres = genres
                 .Skip(skipAmount)
                 .Take(amountOnThisPage)
                 .ToList();

            }
            return genres;
        }

        public List<SongsWithIdViewModel> GetSongs(string? name, string? amountPerPage, string? pageNumber)
        {
            var songs = _context.Songs
                            .Include(s => s.Artist)
                            .Include(s => s.Genre)
                            .Select(s => new SongsWithIdViewModel
                            {
                                Id = s.Id,
                                Name = s.Name,
                                Artist = s.Artist.Name,
                                Genre = s.Genre.Title,
                            })
                            .OrderBy(s => s.Name)
                            .ToList();

            if (songs == null || songs.Count() == 0)
            {
                throw new Exception($"There are no available songs");
            }

            // Show all songs
            if (name is null && amountPerPage is null && pageNumber is null)
            {
                return songs;
            }

            // Show a filtered list       
            if (name is not null)
            {
                songs = songs
                        .Where(a => a.Name.ToUpper().Contains(name.ToUpper()))
                        .ToList();
            }

            // Pagination
            // check if amountPerPage & pageNumber are integers, otherwise return bad request
            // if pageNumber = 0 we assume user meant = 1
            // if amountPerPage = 0 we set default = 10
            if (pageNumber == "0") pageNumber = "1";
            if (amountPerPage == "0") amountPerPage = "10";
            if (!string.IsNullOrEmpty(amountPerPage) || !string.IsNullOrEmpty(pageNumber))
            {
                int parsedAmountPerPage = 10; //sets default value if only pageNumber not null
                if (amountPerPage is not null && !int.TryParse(amountPerPage.ToString(), out parsedAmountPerPage))
                {
                    throw new Exception($"{nameof(amountPerPage)} måste vara en integer");
                }
                int parsedPageNumber = 1; //sets default value if only amountPerPage not null
                if (pageNumber is not null && !int.TryParse(pageNumber.ToString(), out parsedPageNumber))
                {
                    throw new Exception($"{nameof(pageNumber)} måste vara en integer");
                }

                int skipAmount = parsedAmountPerPage * (parsedPageNumber - 1);
                int numberOfPages = (int)Math.Ceiling((decimal)songs.Count / parsedAmountPerPage);
                int amountOnThisPage = parsedPageNumber < numberOfPages ? parsedAmountPerPage : songs.Count % parsedAmountPerPage;

                songs = songs
                 .Skip(skipAmount)
                 .Take(amountOnThisPage)
                 .ToList();

            }

            return songs;

        }

        public async Task AddArtistsGenresAndTracksFromSpotify(ArtistDto artistDto, GenreDto genreDto, List<SongDto> songDtos)
        {
            var genreInDb = await _context.Genres.FirstOrDefaultAsync(g => g.Title == genreDto.Title);
            var artistInDb = await _context.Artists.FirstOrDefaultAsync(a => a.Name == artistDto.Name);

            var songNamesToCheck = songDtos.Select(songDto => songDto.Name).ToList();
            var songInDb = await _context.Songs
                .Where(song => songNamesToCheck.Contains(song.Name))
                .ToListAsync();

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
                        if (!songInDb.Any(s => s.Name == songDto.Name))
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
                }
                else
                {
                    foreach (var songDto in songDtos)
                    {
                        if (!songInDb.Any(s => s.Name == songDto.Name))
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
            }
            else
            {
                if (genreInDb == null)
                {
                    await Console.Out.WriteLineAsync("inside genreInDb");
                    Genre genre = new Genre()
                    {
                        Title = genreDto.Title
                    };

                    foreach (var songDto in songDtos)
                    {
                        if (!songInDb.Any(s => s.Name == songDto.Name))
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
                }
                else
                {
                    foreach (var songDto in songDtos)
                    {
                        if (!songInDb.Any(s => s.Name == songDto.Name))
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
