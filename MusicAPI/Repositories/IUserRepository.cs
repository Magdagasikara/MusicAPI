using MusicAPI.Models.Dtos;
using MusicAPI.Data;
using MusicAPI.Models.ViewModel;
using MusicAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace MusicAPI.Repositories
{
    public interface IUserRepository
    {
        public List<UsersViewModel> GetAllUsers();
        public UserViewModel GetUser(string username);
        public void AddUser(UserDto userDto);
        public void ConnectSongToUser(string username, int songId);
        public void ConnectArtistToUser(string username, int artistId);
        public void ConnectGenreToUser(string username, int genreId);
    }

    public class DbUserRepository : IUserRepository
    {
        private ApplicationContext _context;
        public DbUserRepository(ApplicationContext context)
        {
            _context = context;
        }

        public List<UsersViewModel> GetAllUsers()
        {
            var viewUsers = _context.Users
                 .Select(u => new UsersViewModel
                 {
                     Name = u.Name,
                 })
                .ToList();

            if (viewUsers == null)
                throw new ArgumentNullException($"No users found");

            return viewUsers;
        }

        public UserViewModel GetUser(string username)
        {
            var user = _context.Users
            .Include(u => u.Songs)
            .ThenInclude(s => s.Artist)
            .Include(u => u.Songs)
            .ThenInclude(s => s.Genre)
            .Include(u => u.Artists)
            .Include(u => u.Genres)
            .FirstOrDefault(u => u.Name.ToUpper() == username.ToUpper());

            if (user == null)
                throw new UserNotFoundException();

            var userViewModel = new UserViewModel
            {
                Name = user.Name,
                Artists = user.Artists.Select(a => new ArtistsViewModel()
                {
                    Name = a.Name,
                    Description = a.Description
                }).ToList(),

                Genres = user.Genres.Select(g => new GenresViewModel()
                {
                    Title = g.Title
                }).ToList(),

                Songs = user.Songs.Select(s => new SongsViewModel()
                {
                    Name = s.Name,
                    Artist = s.Artist.Name,
                    Genre = s.Genre.Title
                }).ToList()
            };

            return userViewModel;
        }
        public void AddUser(UserDto user)
        {

            if (_context.Users.Any(u => u.Name.ToUpper() == user.Name.ToUpper()))
                throw new Exception($"User with name {user.Name} already exists in the database.");

            _context.Users
                .Add(new User()
                {
                    Name = user.Name,
                });

            try
            {
                _context.SaveChanges();
            }
            catch
            {
                throw new Exception("Unable to save user to database");
            }
        }

        public void ConnectSongToUser(string username, int songId)
        {
            User? user = _context.Users
                .Include(u => u.Songs)
                .SingleOrDefault(u => u.Name.ToUpper() == username.ToUpper());

            if (user is null)
                throw new UserNotFoundException();

            Song? song = _context.Songs
                .SingleOrDefault(s => s.Id == songId);

            if (song is null)
                throw new SongNotFoundException();

            user.Songs.Add(song);

            try
            {
                _context.SaveChanges();
            }
            catch
            {
                throw new Exception($"Unable to connect user {username} with songId {songId}");
            }
        }

        public void ConnectArtistToUser(string username, int artistId)
        {

            User? user = _context.Users
                .Include(u => u.Artists)
                .SingleOrDefault(u => u.Name.ToUpper() == username.ToUpper());

            if (user is null)
                throw new UserNotFoundException();

            Artist? artist = _context.Artists
                .SingleOrDefault(a => a.Id == artistId);

            if (artist is null)
                throw new ArtistNotFoundException();

            user.Artists.Add(artist);

            try
            {
                _context.SaveChanges();
            }
            catch
            {
                throw new Exception($"Unable to connect user {username} with artistId {artistId}");
            }
        }

        public void ConnectGenreToUser(string username, int genreId)
        {
            User? user = _context.Users
                .Include(u => u.Genres)
                .SingleOrDefault(u => u.Name.ToUpper() == username.ToUpper());

            if (user == null)
                throw new UserNotFoundException();

            Genre? genre = _context.Genres
                .SingleOrDefault(g => g.Id == genreId);

            if (genre == null)
                throw new GenreNotFoundException();

            try
            {
                user.Genres.Add(genre);
                _context.SaveChanges();
            }
            catch
            {
                throw new Exception($"Unable to connect user {username} with GenreId {genreId}");
            }
        }
    }
}
