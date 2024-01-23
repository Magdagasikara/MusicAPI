using MusicAPI.Models.Dtos;
using MusicAPI.Data;
using MusicAPI.Models.ViewModel;
using MusicAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace MusicAPI.Services
{
    public interface IUserHelper
    {
        public List<User> GetAllUsers();
        public User GetUser(int userId);
        public void AddUser(UserDto userDto);
        public void ConnectSongToUser(int userId, int songId);
        public void ConnectArtistToUser(int userId, int artistId);
        public void ConnectGenreToUser(int userId, int genreId);
    }

    public class UserHelper : IUserHelper
    {
        private ApplicationContext _context;
        public UserHelper(ApplicationContext context)
        {
            _context = context;
        }

        public List<User> GetAllUsers()
        {
            List<User> viewUsers = _context.Users.ToList();

            return viewUsers;
        }

        public User GetUser(int userId)
        {
            throw new NotImplementedException();
        }
        public void AddUser(UserDto user)
        {
            
            if (_context.Users.Any(u => u.Name == user.Name))
                throw new Exception($"User with name {user.Name} already exists in the database");

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


        public void ConnectSongToUser(int userId, int songId)
        {
            User? user = _context.Users
                .Include(u => u.Songs)
                .SingleOrDefault(u => u.Id == userId);
            
            if (user is null) 
                throw new ArgumentNullException($"User {userId} not found");

            Song? song = _context.Songs
                .SingleOrDefault(s => s.Id == songId);
            
            if (song is null)
                throw new ArgumentNullException($"Song {songId} not found");

            user.Songs.Add(song);

            try
            {
                _context.SaveChanges();
            }
            catch
            {
                throw new Exception($"Unable to connect userId {userId} with songId {songId}");
            }
        }

        public void ConnectArtistToUser(int userId, int artistId)
        {

            User? user = _context.Users
                .Include(u => u.Artists)
                .SingleOrDefault(u => u.Id == userId);

            if (user is null)
                throw new ArgumentNullException($"User {userId} not found");

            Artist? artist= _context.Artists
                .SingleOrDefault(a => a.Id == artistId);

            if (artist is null)
                throw new ArgumentNullException($"Artist {artistId} not found");

            user.Artists.Add(artist);

            try
            {
                _context.SaveChanges();
            }
            catch
            {
                throw new Exception($"Unable to connect userId {userId} with artistId {artistId}");
            }
        }

        public void ConnectGenreToUser(int userId, int genreId)
        {
            throw new NotImplementedException();
        }

    }
}
